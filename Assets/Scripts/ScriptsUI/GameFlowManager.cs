using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private GameObject WinCanvas;   // Pop-up de victoria
    [SerializeField] private GameObject LoseCanvas;  // Pop-up de derrota

     void Update()
     {
         // Cuando se acaba el tiempo
         if (TiempoJuego.Instance && TiempoJuego.Instance.TiempoRestante <= 0)
         {
             if (PuntajedePregunta.Instance.PuntosActuales >= 100)
             {
                 // Victoria
                 WinCanvas.SetActive(true);
             }
             else
             {
                 // Derrota
                 LoseCanvas.SetActive(true);
             }

             Time.timeScale = 0f; // Pausa el juego
         }

         // Victoria inmediata si alcanza el puntaje antes de que se acabe el tiempo
         if (PuntajedePregunta.Instance.PuntosActuales >= 100)
         {
             WinCanvas.SetActive(true);
             Time.timeScale = 0f;
         }
    }
}
