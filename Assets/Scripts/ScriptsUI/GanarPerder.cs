using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private GameObject winPopup;
    [SerializeField] private GameObject losePopup;

    public int totalPreguntas = 20;
    private int preguntasRespondidas = 0;

    void Update()
    {
        // Derrota por tiempo
        if (TiempoJuego.Instance != null && TiempoJuego.Instance.TiempoRestante <= 0)
        {
            losePopup.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RegistrarPregunta(bool respuestaCorrecta)
    {
        preguntasRespondidas++;

        // Cuando se terminan las preguntas
        if (preguntasRespondidas >= totalPreguntas)
        {
            if (PuntajedePregunta.Instance.PuntosActuales >= 100)
            {
                // Victoria
                winPopup.SetActive(true);
            }
            else
            {
                // Derrota por puntaje insuficiente
                losePopup.SetActive(true);
            }

            Time.timeScale = 0f; // Pausar juego
        }
    }
}


