//using System;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
//using DG.Tweening;

namespace Sessions_Managers
{

    
    public class GameFlowManager : MonoBehaviour
    {
        [SerializeField] private ProgressHandler progressHandler;

        [SerializeField] private GameObject winCanvas; // Pop-up de victoria
        [SerializeField] private GameObject loseCanvas; // Pop-up de derrota
        [SerializeField] private resultadosUI resultadosUI;
        [SerializeField] private UIDocument HUD;
        [SerializeField] private GameObject tiempoYMenu;
        [SerializeField] private FrasesCanvaPerdiste frases;
        [SerializeField] private AudioSource music;
        private GameObject tiempoymenu;
        private GameObject villano;

        [Header("Audio de victoria/derrota")]
        [SerializeField] private AudioSource gameAudioSource;
        [SerializeField] private AudioClip winSound;
        [SerializeField] private AudioClip loseSound;
        [SerializeField, Range(0f, 1f)] private float gameAudioVolume = 1f;

        private bool gameEnded;

        void Start()
        {
            music.Play();
            HUD.gameObject.SetActive(true);
            winCanvas.SetActive(false);
            loseCanvas.SetActive(false);
            tiempoYMenu.SetActive(true);
            Time.timeScale = 1f;
            PuntajedePregunta.Instance.ReiniciarPuntaje();

            if (gameAudioSource == null)
            {
                gameAudioSource = GetComponent<AudioSource>();
                if (gameAudioSource == null)
                {
                    Debug.LogWarning($"GameFlowManager: AudioSource NOT found. Win/Lose sounds won't play.");
                }
            }
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
                MostrarDerrota(puntos, tiempo, contestadas, correctas);
            }
        }

        private void MostrarVictoria(int puntos, float tiempo, int contestadas, int correctas)
        {
            gameEnded = true;
            music.Stop();
            HUD.gameObject.SetActive(false);
            winCanvas.SetActive(true);
            tiempoYMenu.SetActive(false);
            Time.timeScale = 0f;

            if (tiempoymenu != null) tiempoymenu.SetActive(false);
            if (villano != null) villano.SetActive(false);

            PlayWinSound();

            // Pasar datos al panel de resultados
            resultadosUI.MostrarResultados(puntos, tiempo, contestadas, correctas);
            EnviarProgreso(puntos, tiempo);
        }

        private void MostrarDerrota(int puntos, float tiempo, int contestadas, int correctas)
        {
            // Ocultar mesa creacion si activa
            /*ActiveMesaCreacion mesa;
            mesa = UnityEngine.Object.FindAnyObjectByType<ActiveMesaCreacion>();
            mesa.Cerrar();*/
            tiempoYMenu.SetActive(false);
            music.Stop();
            ActiveMesaCreacion mesa = UnityEngine.Object.FindAnyObjectByType<ActiveMesaCreacion>();
            if (mesa != null) mesa.Cerrar();

            gameEnded = true;
            HUD.gameObject.SetActive(false);
            loseCanvas.GetComponent<LosePopupUI>().Show();
            frases.MostrarPuntaje(puntos);

            if (tiempoymenu != null) tiempoymenu.SetActive(false);
            if (villano != null) villano.SetActive(false);

            Time.timeScale = 0f;
            PlayLoseSound();
            Debug.Log("perdiste nub");
            EnviarProgreso(puntos, tiempo);

        }
        
        private void EnviarProgreso(int puntos, float tiempo)
        {
            ProgressHandler.PartidaData partida = new ProgressHandler.PartidaData
            {
                id_cuenta = GameSession.Instance.userId,
                nombreIsla = GameSession.Instance.IslaActual,
                dificultad = GameSession.Instance.DificultadActual.ToString(),
                score_max = puntos,
                tiempo_seg = Mathf.RoundToInt(tiempo),
                intentos = new List<ProgressHandler.IntentoPregunta>()
            };

            foreach (var intento in PuntajedePregunta.Instance.ListaIntentos)
            {
                partida.intentos.Add(new ProgressHandler.IntentoPregunta
                {
                    id_pregunta = intento.id_pregunta,
                    respuesta_usuario = intento.respuesta_usuario,
                    es_correcto = intento.es_correcto,
                    tiempo_respuesta_seg = intento.tiempo_respuesta_seg
                });
            }

            progressHandler.GuardarPartida(partida);
        }

        private void PlayWinSound()
        {
            if (gameAudioSource == null || winSound == null)
            {
                return;
            }
            gameAudioSource.PlayOneShot(winSound, gameAudioVolume);
        }

        private void PlayLoseSound()
        {
            if (gameAudioSource == null || loseSound == null)
            {
                return;
            }
            gameAudioSource.PlayOneShot(loseSound, gameAudioVolume);
        }
    }
};

