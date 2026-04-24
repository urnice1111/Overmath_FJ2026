using System;
using UnityEngine;
using DG.Tweening;


public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private GameObject WinCanvas; // Pop-up de victoria
    [SerializeField] private GameObject LoseCanvas; // Pop-up de derrota
    [SerializeField] private resultadosUI resultadosUI;
    [SerializeField] private FrasesCanvaPerdiste frases;
    private GameObject tiempoymenu;
    private GameObject villano;

    private bool gameEnded = false;

    void Start()
    {
        WinCanvas.SetActive(false);
        LoseCanvas.SetActive(false);
        Time.timeScale = 1f;
        PuntajedePregunta.Instance.ReiniciarPuntaje();
    }

    void Update()
    {
        if (gameEnded)
        {
            tiempoymenu = GameObject.Find("TiempoYMenu");
            tiempoymenu.SetActive(false);
            villano = GameObject.Find("FakeObjective");
            villano.SetActive(false); }

        ;
        
        float tiemporestante = TiempoJuego.Instance.TiempoRestante;

        int puntos = PuntajedePregunta.Instance.PuntosActuales;
        float tiempo = TiempoJuego.Instance.TiempoJugado;
        int contestadas = PuntajedePregunta.Instance.TotalContestadas;
        int correctas = PuntajedePregunta.Instance.TotalCorrectas;

        if (puntos >= 100)
        {
            //MostrarDerrota();
            MostrarVictoria(puntos, tiempo, contestadas, correctas);
        }
        else if (tiemporestante <= 0 && puntos < 100)
        {
            //MostrarVictoria(puntos, tiempo, contestadas, correctas);
            MostrarDerrota(puntos);
        }
    }

    private void MostrarVictoria(int puntos, float tiempo, int contestadas, int correctas)
    {
        gameEnded = true;
        WinCanvas.SetActive(true);
        Time.timeScale = 0f;

        // Pasar datos al panel de resultados
        resultadosUI.MostrarResultados(puntos, tiempo, contestadas, correctas);
    }

    private void MostrarDerrota(int puntos)
    {
        // Ocultar mesa creacion si activa
        ActiveMesaCreacion mesa = null;
        mesa = UnityEngine.Object.FindAnyObjectByType<ActiveMesaCreacion>();
        mesa.Cerrar();

        gameEnded = true;
        LoseCanvas.GetComponent<LosePopupUI>().Show();
        frases.MostrarPuntaje(puntos);
        Time.timeScale = 0f;
        Debug.Log("perdiste nub");
        
    }
}
