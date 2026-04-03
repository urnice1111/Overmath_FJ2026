using UnityEngine;
using System.Collections;

public class ControlVillano : MonoBehaviour
{
    public GameObject villano1Base;
    public GameObject villano1Golpe;
    public GameObject villano1Risa;
    public GameObject pow;
    public GameObject hahaha;

    public float tiempoResultado = 2f; //Tiempo que se muestra la imagen del resultado a la pregunta antes de volver a la imagen base

    void Start()
    {
        MostrarVillanoBase();
    }
    public void MostrarVillanoBase() //Muestra la imagen del villano en su estado base
    {
        villano1Base.SetActive(true); //Solo se muestra la imagen del villano en base, el resto de las imágenes se ocultan
        villano1Golpe.SetActive(false);
        villano1Risa.SetActive(false);
        pow.SetActive(false);
        hahaha.SetActive(false);
    }

    public void RespuestaCorrecta()
    {
        StopAllCoroutines(); //Hace que se detengan las acciones anteriores, para que no se mezclen con las nuevas

        villano1Base.SetActive(false);
        villano1Golpe.SetActive(true); //Solo se muestra la imagen del villano "recibiendo" el golpe, el resto de las imágenes se ocultan
        villano1Risa.SetActive(false);
        pow.SetActive(true); //Efecto de golpe
        hahaha.SetActive(false);
        StartCoroutine(RegresarVillanoBase()); //Inicia la corrutina para volver a la imagen base después del tiempo
    }

    public void RespuestaIncorrecta()
    {
        StopAllCoroutines(); //Hace que se detengan las acciones anteriores, para que no se mezclen con las nuevas

        villano1Base.SetActive(false);
        villano1Golpe.SetActive(false);
        villano1Risa.SetActive(true); //Solo se muestra la imagen del villano riendo, el resto de las imágenes se ocultan
        pow.SetActive(false);
        hahaha.SetActive(true); //Y también el efecto de risa
        StartCoroutine(RegresarVillanoBase()); //Inicia la corrutina para volver a la imagen base después del tiempo
    }

    private IEnumerator RegresarVillanoBase() //Corrutina para volver a la imagen base después del tiempo establecido
    {
        yield return new WaitForSeconds(tiempoResultado); //Espera el tiempo establecido antes de ejecutar la siguiente línea de código
        MostrarVillanoBase(); //Vuelve a mostrar la imagen del villano en su estado base
    }
}
