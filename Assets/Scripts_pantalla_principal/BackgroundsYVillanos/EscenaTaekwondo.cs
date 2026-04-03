using UnityEngine;

public class EscenaTaekwondo : MonoBehaviour
{
    public float speed = 0.5f; //Velocidad a la que se mueve el fondo
    private float width; //Ancho del fondo

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x; //Obtiene el ancho del fondo a partir del tamaño del sprite, para saber cuándo reiniciar la posición
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime); //Mueve el fondo hacia la izquierda, multiplicada por Time.deltaTime para que el movimiento sea suave

        if (transform.position.x <= -width) //Si la posición del fondo es menor o igual a su ancho negativo, significa que ha salido completamente de la pantalla
        {
            transform.position += new Vector3(width * 2, 0, 0); //Para que vuelva a aparecer en la pantalla y crear un efecto de movimiento continuo (loop)
        }
    }
}
