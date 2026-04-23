using UnityEngine;

public class MovimientoBackground : MonoBehaviour
{
    public float velocidad = 0.5f; //Movemet speed of the bakcground
    public float desplazamientoMax = 5f; //Amount of movement in the background
    public SpriteRenderer sr; //Reference to the SpriteRenderer component of the background

    private Vector3 posicionInicial;
    private int direccion = 1; //Direction of movement, 1 for right and -1 for left
    void Start()
    {
        posicionInicial = transform.position; //Store the initial position of the background
    }

    void Update()
    {
        transform.position += Vector3.right * direccion * velocidad * Time.deltaTime; //move the background in the current p, dir, sp and t
        float distancia = transform.position.x - posicionInicial.x; //Calculate the distance moved from the initial position
        if (distancia <= -desplazamientoMax)
        {
            direccion = 1; //Move to the right
        }
        else if (distancia >= 0f)
        {
            direccion = -1; //Move to the left
        }
    }
}

