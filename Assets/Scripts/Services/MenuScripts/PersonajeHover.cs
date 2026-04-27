using UnityEngine;

public class PersonajeHover : MonoBehaviour
{
    public GameObject burbuja;
    private PersonajeMov movimiento; 

    void Start()
    {
        movimiento = GetComponent<PersonajeMov>();
        if (burbuja != null)
        {
            burbuja.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        movimiento.PausarMovimiento(true);

        if (burbuja != null)
        {
            burbuja.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        movimiento.PausarMovimiento(false);
        if (burbuja != null)
        {
            burbuja.SetActive(false);
        }
    }
}
