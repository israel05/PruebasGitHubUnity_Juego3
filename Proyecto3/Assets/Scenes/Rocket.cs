using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody; //para el cohete
    AudioSource audioSource; //para el sonido del misisl

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

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
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

        /*
         Puede rotar pero solo hacia un lado u otro
         */
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
            print("rotacion A");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotacion D");
            transform.Rotate(-Vector3.forward);
        }
    }
}
