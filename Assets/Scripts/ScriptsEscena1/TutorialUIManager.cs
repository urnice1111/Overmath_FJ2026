using UnityEngine;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    public GameObject textoObjetivo;
    public GameObject flecha;

    public float delayMensaje = 6f; 

    void Start()
    {
        textoObjetivo.SetActive(false);
        flecha.SetActive(false);

        Invoke("MostrarMensajeInicial", delayMensaje);
    }

    void MostrarMensajeInicial()
    {
        textoObjetivo.SetActive(true);
    }

    public void MostrarFlecha()
    {
        textoObjetivo.SetActive(false);
        flecha.SetActive(true);
    }
}
