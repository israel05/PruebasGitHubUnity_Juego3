using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody; //para el cohete
    AudioSource audioSource; //para el sonido del misisl
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip colisionFatal;
    [SerializeField] AudioClip sonidoVictoria;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;


    //estados posibles del jugador, lo normal es estar vivp
    enum State { Alive, Dying, Trasncending}
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

    
    private void ProcessInput()
    {

        // si su estado es vivo, puede hacer todo eso, si esta muerto no
        if (state.Equals(State.Alive)) {
            ResponderAEmpuje();
            ResponderARotacion();
        }
        if (state == State.Dying) {
        }
    }
        
    


    private void ResponderAEmpuje()
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

    private void AplicarEmpuje()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void ResponderARotacion()
    {

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; //para que se quede bloqueada
        if (Input.GetKey(KeyCode.A))
        {            
            transform.Rotate(Vector3.forward * rotationThisFrame);          
        }
        else if (Input.GetKey(KeyCode.D))
        {           
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; //para que se quede libre la rotracion y no siga girando como loco

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive) { return; } //sal de la función, no detectes nada, estas meurto
               
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

    private void ComienzoTransicionVictoria()
    {
        state = State.Trasncending;
        audioSource.Stop();
        audioSource.PlayOneShot(sonidoVictoria); //sonido de victoria
        successParticles.Play();
        Invoke("LoadNextLevel", 2f); //llamala a la función después de un esperar un segundo
    }

    private void ComienzoTransicionMuerte()
    {
        audioSource.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(colisionFatal);
        state = State.Dying;
        
        Invoke("LoadFirstLevel", 1f);
    }

   

    private void LoadNextLevel()
    {
        
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
