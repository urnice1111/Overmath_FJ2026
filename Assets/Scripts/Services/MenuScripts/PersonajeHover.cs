using UnityEngine;

public class PersonajeHover : MonoBehaviour
{
    public GameObject burbuja;
    private PersonajeMov movimiento; 

    void Start()
    {
        movimiento = GetComponent<PersonajeMov>();
        burbuja.SetActive(false);
    }

    void OnMouseEnter()
    {
        movimiento.PausarMovimiento(true);
        burbuja.SetActive(true);
    }

    void OnMouseExit()
    {
        movimiento.PausarMovimiento(false);
        burbuja.SetActive(false);
    }
}
