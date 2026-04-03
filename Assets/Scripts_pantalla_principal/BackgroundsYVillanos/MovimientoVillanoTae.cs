using UnityEngine;

public class MovimientoVillanoTae : MonoBehaviour
{
    public float velocidad = 1f;
    public float limite = 5f;
    private Vector3 posicionInicial;
    private int direccion = 1;
    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direccion * velocidad * Time.deltaTime);

        if (transform.position.x >= posicionInicial.x + limite)
        {
            direccion = -1;
        }

        else if (transform.position.x <= posicionInicial.x - limite)
        {
            direccion = 1;
        }
    }
}
