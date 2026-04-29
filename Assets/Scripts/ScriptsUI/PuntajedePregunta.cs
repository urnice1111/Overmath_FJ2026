using UnityEngine;
using System.Collections.Generic;

public class PuntajedePregunta : MonoBehaviour
{
    [SerializeField] private mostrarPuntaje mostrarPuntajeUI;
    public static PuntajedePregunta Instance { get; private set; }

    [SerializeField] private int MinimoDePuntos = 100;
    [SerializeField] private int puntosPorAcierto = 1;
    [SerializeField] private int bonusPorNumeroUsado = 2;
    [SerializeField] private int bonusPorOperadorUsado = 2;
    public int TotalContestadas { get; private set; }
    public int TotalCorrectas { get; private set; }

    // private const string PlayerPrefsKey = "PuntajeActual";

    public int PuntosActuales{get; private set;}
    
    //lista para acumular los intentos de cada pregunta, con información sobre si fue correcta o no, y cuántos números y operadores se usaron
    public List<ProgressHandler.IntentoPregunta> ListaIntentos { get; private set; } = new List<ProgressHandler.IntentoPregunta>();

    //Calcula el porcentaje actual basado en los puntos actuales y el límite de puntos
    public int PorcentajeActual => MinimoDePuntos <= 0 ? 0 : Mathf.RoundToInt((PuntosActuales / (float)MinimoDePuntos) * 100f); //Redondea el porcentaje y el porcentaje no puede ser negativo ni mayor a 100


    //Hace que solo exista una instancia de PuntajedePregunta
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CargarPuntaje();
    }

    //Registra el resultado de una pregunta, sumando puntos solo si la respuesta es correcta
    public void RegistrarResultado(bool respuestaCorrecta,
        int numerosUsados = 0,
        int operadoresUsados = 0,
        int idPregunta = 0,
        string respuestaUsuario = "",
        float tiempoRespuesta = 0f)
    {
        TotalContestadas++; // siempre suma una pregunta contestada
        
        if (respuestaCorrecta)
        {
            int bonus = Mathf.Max(0, numerosUsados) * bonusPorNumeroUsado
                      * Mathf.Max(0, operadoresUsados) * bonusPorOperadorUsado;

            SumarPuntos(puntosPorAcierto + bonus);
            
            TiempoJuego.Instance.AjustarTiempo(+10f); // aumenta tiempo
            
            TotalCorrectas++; // solo suma si fue correcta
        }
        else
        {
            TiempoJuego.Instance.AjustarTiempo(-10f); // disminuye tiempo
        } 
        
        // Guardar intento en la lista
        ListaIntentos.Add(new ProgressHandler.IntentoPregunta {
            id_pregunta = idPregunta,
            respuesta_usuario = respuestaUsuario,
            es_correcto = respuestaCorrecta,
            tiempo_respuesta_seg = tiempoRespuesta});
    }

    //Reinicia el puntaje actual a cero y guarda el cambio
    public void ReiniciarPuntaje()
    {
        PuntosActuales = 0;
        GuardarPuntaje();
        ListaIntentos.Clear(); // Limpia la lista de intentos para la nueva partida
    }


    //Suma una cantidad específica de puntos al puntaje actual
    private void SumarPuntos(int cantidad)
    {
        PuntosActuales = Mathf.Clamp(PuntosActuales + cantidad, 0, MinimoDePuntos);//Suma los puntos actuales con la cantidad de puntos, asegurándose de que el resultado esté entre 0 y el límite de puntos
        GuardarPuntaje();
        
        // Actualizar UI
        if (mostrarPuntajeUI != null)
        {
            mostrarPuntajeUI.MostrarPuntajee(PuntosActuales);
        }
        
        Debug.Log("PuntajedePregunta: " + PuntosActuales + "/" + MinimoDePuntos + " (" + PorcentajeActual + "%)");
    }

    //Carga el puntaje actual desde PlayerPrefs
    private void CargarPuntaje()
    {
        PuntosActuales = 0;
    }

    private void GuardarPuntaje()
    {
        // PlayerPrefs.SetInt(PlayerPrefsKey, PuntosActuales);//Guarda el puntaje actual en PlayerPrefs
        // PlayerPrefs.Save();
    }
}
