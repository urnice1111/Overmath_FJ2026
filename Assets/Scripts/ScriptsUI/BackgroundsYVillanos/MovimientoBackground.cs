using UnityEngine;

public class MovimientoBackground : MonoBehaviour
{
    public float velocidad = 0.05f;
    public float velocidadRotacion = 180f; //rotation speed in degrees per second
    private float ancho;
    private Vector3 posicionInicial;

    void Start()
    {
        ancho = GetComponent<SpriteRenderer>().bounds.size.x;
        posicionInicial = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.left * velocidad * Time.deltaTime;
        transform.Rotate(0f, 0f, velocidadRotacion * Time.deltaTime);

        if (transform.position.x <= -ancho)
        {
            transform.position = posicionInicial;
            transform.rotation = Quaternion.identity;
        }
    }
}
