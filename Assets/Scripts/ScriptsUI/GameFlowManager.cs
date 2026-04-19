using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private GameObject winPopup;   // Pop-up de victoria
    [SerializeField] private GameObject losePopup;  // Pop-up de derrota

    void Update()
    {
        // Cuando se acaba el tiempo
        if (TiempoJuego.Instance != null && TiempoJuego.Instance.TiempoRestante <= 0)
        {
            if (PuntajedePregunta.Instance.PuntosActuales >= 100)
            {
                // Victoria
                winPopup.SetActive(true);
            }
            else
            {
                // Derrota
                losePopup.SetActive(true);
            }

            Time.timeScale = 0f; // Pausa el juego
        }

        // Victoria inmediata si alcanza el puntaje antes de que se acabe el tiempo
        if (PuntajedePregunta.Instance.PuntosActuales >= 100)
        {
            winPopup.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
