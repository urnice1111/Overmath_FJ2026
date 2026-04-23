using UnityEngine;

public class MovimientoBackground : MonoBehaviour
{
    public float velocidad = 0.05f;
    private float ancho;

    void Start()
    {
        ancho = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        transform.position += Vector3.left * velocidad * Time.deltaTime;
        if (transform.position.x <= -ancho)
        {
            transform.position += new Vector3(ancho * 2f, 0f, 0f);
        }
    }
}
