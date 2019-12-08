using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody; //para el cohete
    AudioSource audioSource; //para el sonido del misisl


    [SerializeField] AudioClip cancioncillaDeFondo;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] float levelLoadDealy = 3f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip colisionFatal;
    [SerializeField] AudioClip sonidoVictoria;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;


    //activar las colisiobnes
    bool ColisionesDesactivadas = false;

    //Nivel en el que estamos
    int nivelActual = 0; //empezamos en el nivel 0


    //estados posibles del jugador, lo normal es estar vivp
    enum State { Alive, Dying, Trasncending }
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }


    //activa el modo depuración y permite saltar de nivel

    private void entrarEnModoDepuracion()
    {
        if (Input.GetKeyDown(KeyCode.L))

        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //deshabilitar las colisiones
            //cambia el estado entre las colisiones
            ColisionesDesactivadas = !ColisionesDesactivadas;
        }

    }
    void ProcessInput()
    {

        // si su estado es vivo, puede hacer todo eso, si esta muerto no
        if (state.Equals(State.Alive)) {
            ResponderAEmpuje();
            ResponderARotacion();
        }
        if (state == State.Dying) {
        }
        //entrar en modo depuracion
        if (Debug.isDebugBuild)
        {
            entrarEnModoDepuracion();
        }
        

    }
        
    


    void ResponderAEmpuje()
    {


        if (Input.GetKey(KeyCode.Space))
        {
            AplicarEmpuje();

            //  print("espaciejo");
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void AplicarEmpuje()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            audioSource.PlayOneShot(cancioncillaDeFondo);
        }
        mainEngineParticles.Play();
    }

    void ResponderARotacion()
    {                       
        if (Input.GetKey(KeyCode.A))
        {
            RotacionManual(rcsThrust * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotacionManual(-rcsThrust * Time.deltaTime); 
        }
    }

    /**
     * Permite la rotación de un objeto al que se le pasa un fotograma concreto
     **/
    private void RotacionManual(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true; //para que se quede bloqueada
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false; //para que se quede libre la rotracion y no siga girando como loco
    }

    void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive || ColisionesDesactivadas) { return; } //sal de la función, no detectes nada, estas meurto
               
        switch (collision.gameObject.tag)
        {
            case "Friendly":                
                break;

            case "Launchpad":                
                break;

            case "Finishing":
                ComienzoTransicionVictoria();
                break;

            default:
                ComienzoTransicionMuerte();
                break;
        }
    }

    void ComienzoTransicionVictoria()
    {
        state = State.Trasncending;
        audioSource.Stop();
        audioSource.PlayOneShot(sonidoVictoria); //sonido de victoria
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDealy); //llamala a la función después de un esperar un segundo
    }

    void ComienzoTransicionMuerte()
    {
        audioSource.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(colisionFatal);
        state = State.Dying;
        
        Invoke("LoadFirstLevel", levelLoadDealy);
    }



    void LoadNextLevel()
    {
        nivelActual = nivelActual + SceneManager.GetActiveScene().buildIndex;
        

        if (nivelActual > (SceneManager.sceneCountInBuildSettings-2))
        {
            nivelActual = 0;
        } else
        {
            nivelActual++;
        }
        SceneManager.LoadScene(nivelActual);





    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
