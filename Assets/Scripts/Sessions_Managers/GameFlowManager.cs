//using System;
using UnityEngine;
//using DG.Tweening;

namespace Sessions_Managers
{
    public class GameFlowManager : MonoBehaviour
    {
        [SerializeField] private GameObject winCanvas; // Pop-up de victoria
        [SerializeField] private GameObject loseCanvas; // Pop-up de derrota
        [SerializeField] private resultadosUI resultadosUI;
        [SerializeField] private FrasesCanvaPerdiste frases;
        private GameObject tiempoymenu;
        private GameObject villano;

        private bool gameEnded;

        void Start()
        {
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
            Time.timeScale = 1f;
            PuntajedePregunta.Instance.ReiniciarPuntaje();
        }

        void Update()
        {
            if (gameEnded) return;
            /*{
                tiempoymenu = GameObject.Find("TiempoYMenu");
                tiempoymenu.SetActive(false);
                villano = GameObject.Find("FakeObjective");
                villano.SetActive(false);
            }*/

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
            winCanvas.SetActive(true);
            Time.timeScale = 0f;
            
            if (tiempoymenu != null) tiempoymenu.SetActive(false);
            if (villano != null) villano.SetActive(false);

            // Pasar datos al panel de resultados
            resultadosUI.MostrarResultados(puntos, tiempo, contestadas, correctas);
        }

        private void MostrarDerrota(int puntos)
        {
            // Ocultar mesa creacion si activa
            /*ActiveMesaCreacion mesa;
            mesa = UnityEngine.Object.FindAnyObjectByType<ActiveMesaCreacion>();
            mesa.Cerrar();*/
            
            ActiveMesaCreacion mesa = UnityEngine.Object.FindAnyObjectByType<ActiveMesaCreacion>();
            if (mesa != null) mesa.Cerrar();

            gameEnded = true;
            loseCanvas.GetComponent<LosePopupUI>().Show();
            frases.MostrarPuntaje(puntos);
            
            if (tiempoymenu != null) tiempoymenu.SetActive(false);
            if (villano != null) villano.SetActive(false);
            
            Time.timeScale = 0f;
            Debug.Log("perdiste nub");

        }
    }
};