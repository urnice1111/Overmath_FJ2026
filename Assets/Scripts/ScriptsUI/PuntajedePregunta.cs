using UnityEngine;

public class PuntajedePregunta : MonoBehaviour
{
    public static PuntajedePregunta Instance { get; private set; }

    [SerializeField] private int limitePuntos = 100;
    [SerializeField] private int puntosPorAcierto = 5;

    private const string PlayerPrefsKey = "PuntajeActual";

    public int PuntosActuales{get; private set;}


    //Calcula el porcentaje actual basado en los puntos actuales y el límite de puntos
    public int PorcentajeActual => limitePuntos <= 0 ? 0 : Mathf.RoundToInt((PuntosActuales / (float)limitePuntos) * 100f); //Redondea el porcentaje y el porcentaje no puede ser negativo ni mayor a 100


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
    public void RegistrarResultado(bool respuestaCorrecta)
    {
        if (respuestaCorrecta)
        {
            SumarPuntos(puntosPorAcierto);
            //TiempoJuego.AjustarTiempo(10f);
        }
        else
        {
            //TiempoJuego.AjustarTiempo(30f);
        }
    }

    //Reinicia el puntaje actual a cero y guarda el cambio
    public void ReiniciarPuntaje()
    {
        PuntosActuales = 0;
        GuardarPuntaje();
    }


    //Suma una cantidad específica de puntos al puntaje actual
    private void SumarPuntos(int cantidad)
    {
        PuntosActuales = Mathf.Clamp(PuntosActuales + cantidad, 0, limitePuntos);//Suma los puntos actuales con la cantidad de puntos, asegurándose de que el resultado esté entre 0 y el límite de puntos
        GuardarPuntaje();
        Debug.Log("PuntajedePregunta: " + PuntosActuales + "/" + limitePuntos + " (" + PorcentajeActual + "%)");
    }

    //Carga el puntaje actual desde PlayerPrefs
    private void CargarPuntaje()
    {
        PuntosActuales = Mathf.Clamp(PlayerPrefs.GetInt(PlayerPrefsKey, 0), 0, limitePuntos);
    }

    private void GuardarPuntaje()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, PuntosActuales);//Guarda el puntaje actual en PlayerPrefs
        PlayerPrefs.Save();
    }
}
