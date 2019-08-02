using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

        if (Input.GetKey(KeyCode.Space)) {
            print("espaciejo");
        }

        /*
         Puede rotar pero solo hacia un lado u otro
         */
        if (Input.GetKey(KeyCode.A))
        {
            print("rotacion A");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotacion D");
        }
    }
}
