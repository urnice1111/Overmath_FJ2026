using UnityEngine;
using TMPro;

public class PreguntaManager : MonoBehaviour
{
    public static PreguntaManager Instance {get; private set;}

    [SerializeField] private BancoPreguntasSO banco;

    [SerializeField] private TextMeshProUGUI textoPregunta;

    public PreguntaSO PreguntaActual {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CargarPreguntaAleatoria();
    }
    public void CargarPreguntaAleatoria()
    {
        if (banco == null || banco.preguntas.Count == 0)
        {
            Debug.LogError("PreguntaManager: banco vacío o no asignado.");
            return;
        }
        PreguntaActual = banco.preguntas[Random.Range(0, banco.preguntas.Count)];
        if (textoPregunta != null)
            textoPregunta.text = PreguntaActual.textoPregunta;
    }

    public bool VerificarRespuesta(int resultado)
    {
        if (PreguntaActual == null) return false;
        return resultado == PreguntaActual.respuestaCorrecta;
    }



}
