using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody; //para el cohete
    AudioSource audioSource; //para el sonido del misisl
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

     
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
            Empuje();
            Rotacion();
        }
        if (state == State.Dying) {
            audioSource.Stop(); //como ha muerto, no hay sonidito de motor
        }
    }
        
    


    private void Empuje()
    {


        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            //  print("espaciejo");
        }
        else
        {
            audioSource.Stop();
        }
    }


    private void Rotacion()
    {

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; //para que se quede bloqueada
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
            print("rotacion A");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotacion D");
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
                //nada
                break;
            case "Launchpad":
                //nada
                break;
            case "Finishing":

                //carga la uno
                state = State.Trasncending;
                Invoke("LoadNextLevel", 1f); //llamala a la función después de un esperar un segundo
                break;
            default:
                state = State.Dying;   
                Invoke("LoadFirstLevel", 1f);                
                break;
        }
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
