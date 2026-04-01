using UnityEngine;

public class PuntajedePregunta : MonoBehaviour
{
    public static PuntajedePregunta Instance { get; private set; }

    [SerializeField] private int limitePuntos = 100;
    [SerializeField] private int puntosPorAcierto = 5;

    private const string PlayerPrefsKey = "PuntajeActual";

    public int PuntosActuales{get; private set;}

    public int PorcentajeActual => limitePuntos <= 0 ? 0 : Mathf.RoundToInt((PuntosActuales / (float)limitePuntos) * 100f);

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

    public void RegistrarResultado(bool respuestaCorrecta)
    {
        if(!respuestaCorrecta)
        {
            return;
        }

        SumarPuntos(puntosPorAcierto);
    }

    public void ReiniciarPuntaje()
    {
        PuntosActuales = 0;
        GuardarPuntaje();
    }

    private void SumarPuntos(int cantidad)
    {
        PuntosActuales = Mathf.Clamp(PuntosActuales + cantidad, 0, limitePuntos);
        GuardarPuntaje();
        Debug.Log("PuntajedePregunta: " + PuntosActuales + "/" + limitePuntos + " (" + PorcentajeActual + "%)");
    }

    private void CargarPuntaje()
    {
        PuntosActuales = Mathf.Clamp(PlayerPrefs.GetInt(PlayerPrefsKey, 0), 0, limitePuntos);
    }

    private void GuardarPuntaje()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, PuntosActuales);
        PlayerPrefs.Save();
    }
}
