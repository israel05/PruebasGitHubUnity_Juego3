using UnityEngine;
using UnityEngine.SceneManagement;

//para que no tenga más de uno de estos scripts a la vez
//no tiene sentido tener varios osculadores
[DisallowMultipleComponent]


public class oscilator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10);
    [SerializeField] float period = 2f;

    [Range(0, 1)] [SerializeField] float movementFactor; //0 no se mueve, 1 a saco

    // Start is called before the first frame update

    Vector3 startingPos; 

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //correción de movementFactor para que oscile seno
        if (period == Mathf.Epsilon) { period += 0.1f; };

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2; //solo el valor de tau, 6....
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
