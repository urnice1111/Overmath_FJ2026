using UnityEngine;

public class SubirBarco : MonoBehaviour
{
    public GameObject personaje;
    public GameObject barco;

    public MonoBehaviour controlPersonaje;
    public MonoBehaviour controlBarco;

    public Transform puntoSubida;

    public GameObject botonSubir;

    public void Subir()
    {
        
        barco.transform.position = puntoSubida.position;

        personaje.SetActive(false);

        barco.SetActive(true);
    
        controlPersonaje.enabled = false;
        controlBarco.enabled = true;

        botonSubir.SetActive(false);
    }
}