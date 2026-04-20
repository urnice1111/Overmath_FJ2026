using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private GameObject WinCanvas; // Pop-up de victoria
    [SerializeField] private GameObject LoseCanvas; // Pop-up de derrota
    [SerializeField] private resultadosUI resultadosUI;

    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded) return;
        
        float tiemporestante = TiempoJuego.Instance.TiempoRestante;

        int puntos = PuntajedePregunta.Instance.PuntosActuales;
        float tiempo = TiempoJuego.Instance.TiempoJugado;
        int contestadas = PreguntaManager.Instance.TotalContestadas;
        int correctas = PreguntaManager.Instance.TotalCorrectas;

        if (puntos >= 100)
        {
            //MostrarDerrota();
            MostrarVictoria(puntos, tiempo, contestadas, correctas);
        }
        else if (tiemporestante <= 0 && puntos < 100)
        {
            //MostrarVictoria(puntos, tiempo, contestadas, correctas);
            MostrarDerrota();
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

    private void MostrarDerrota()
    {
        gameEnded = true;
        LoseCanvas.SetActive(true);
        Time.timeScale = 0f;

    }
}
