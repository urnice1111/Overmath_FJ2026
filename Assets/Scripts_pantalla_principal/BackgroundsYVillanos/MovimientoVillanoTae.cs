using UnityEngine;

public class MovimientoVillanoTae : MonoBehaviour
{
    public float velocidad = 2f; //Velocidad a la que se mueve el villano
    public float limite = 3f; //Distancia máxima que el villano se moverá desde su posición inicial antes de cambiar de dirección
    private Vector3 posicionInicial; //Almacena la posición inicial del villano para calcular los límites de movimiento
    private int direccion = 1; //Variable para controlar la dirección del movimiento, 1 para derecha y -1 para izquierda
    void Start()
    {
        posicionInicial = transform.position; //Guarda la posición inicial del villano al comenzar el juego, para usarla como referencia para los límites de movimiento
        limiteIzquierdo = posicionInicial.x - limite; //Calcula el límite izquierdo
        limiteDerecho = posicionInicial.x + limite; //Calcula el límite derecho
    }

    void Update()
    {
        transform.Translate(Vector3.right * direccion * velocidad * Time.deltaTime); //Mueve el villano en la dirección actual, para que el movimiento sea suave

        if (transform.position.x >= posicionInicial.x + limite) //Si la posición del villano es mayor o igual a su posición inicial más el límite, significa que ha alcanzado el límite derecho de su movimiento
        {
            direccion = -1; //Cambia la dirección a la izquierda
        }

        else if (transform.position.x <= posicionInicial.x - limite)
        {
            direccion = 1; //Cambia la dirección a derecha
        }
    }
}
