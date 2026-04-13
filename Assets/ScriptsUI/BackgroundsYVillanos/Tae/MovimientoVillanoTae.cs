using UnityEngine;

public class MovimientoVillanoTae : MonoBehaviour
{
    public float velocidad = 2f; //Velocidad a la que se mueve el villano
    public float limite = 3f; //Distancia máxima que el villano se moverá desde su posición inicial antes de cambiar de dirección
    private Vector3 posicionInicial; //Almacena la posición inicial del villano para calcular los límites de movimiento
    private float limiteIzquierdo; //Límite izquierdo de movimiento en X
    private float limiteDerecho; //Límite derecho de movimiento en X
    private int direccion = 1; //Variable para controlar la dirección del movimiento, 1 para derecha y -1 para izquierda
    void Start()
    {
        posicionInicial = transform.position; //Guarda la posición inicial del villano al comenzar el juego, para usarla como referencia para los límites de movimiento
        limiteIzquierdo = posicionInicial.x - limite; //Calcula el límite izquierdo
        limiteDerecho = posicionInicial.x + limite; //Calcula el límite derecho
    }

    void Update()
    {
        Vector3 nuevaPosicion = transform.position; //Crea una variable para almacenar la nueva posición del villano.
        nuevaPosicion.x += direccion * velocidad * Time.deltaTime; //Calcula la nueva posición del villano en el eje x.

        if (nuevaPosicion.x >= limiteDerecho) //Si el villano alcanza o supera el límite derecho, cambia la dirección a la izquierda
        {
            nuevaPosicion.x = limiteDerecho;
            direccion = -1; //Cambia la dirección a la izquierda
        }

        else if (nuevaPosicion.x <= limiteIzquierdo)
        {
            nuevaPosicion.x = limiteIzquierdo;
            direccion = 1; //Cambia la dirección a derecha
        }
        transform.position = nuevaPosicion;
    }
}
