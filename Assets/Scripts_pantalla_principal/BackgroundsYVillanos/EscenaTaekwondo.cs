using UnityEngine;

public class EscenaTaekwondo : MonoBehaviour
{
    public float velocidad  = 0.5f; //Velocidad a la que se mueve el fondo
    private float ancho; //Ancho del fondo

    void Start()
    {
        ancho = GetComponent<SpriteRenderer>().bounds.size.x; //Obtiene el ancho del fondo a partir del tamaño del sprite, para saber cuándo reiniciar la posición
    }

    void Update()
    {
        transform.Translate(Vector3.left * velocidad * Time.deltaTime); //Mueve el fondo hacia la izquierda, multiplicada por Time.deltaTime para que el movimiento sea suave

        if (transform.position.x <= -ancho) //Si la posición del fondo es menor o igual a su ancho negativo, significa que ha salido completamente de la pantalla
        {
            transform.position += new Vector3(ancho * 2, 0, 0); //Para que vuelva a aparecer en la pantalla y crear un efecto de movimiento continuo (loop)
        }
    }
}
