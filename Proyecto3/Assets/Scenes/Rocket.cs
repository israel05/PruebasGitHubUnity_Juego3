using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody; //para el cohete
    AudioSource audioSource; //para el sonido del misisl
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
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
        /*
         Puedo empujar siempre
         */
        Empuje();

        /*
         Puede rotar pero solo hacia un lado u otro
         */
        Rotacion();
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
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //nada
                break;
            case "Launchpad":
                //nada
                break;
            case "Finishing":
                print("he llegado a la meta");
                //carga la uno
                SceneManager.LoadScene(1);
                break;
            default:
                print("has muerto!");
                SceneManager.LoadScene(0);
                //nada
                break;

        }
    }

}
