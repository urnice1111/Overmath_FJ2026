// Manages the questions, including fetching them from the Lambda function.
using UnityEngine;
using TMPro;  
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PreguntaManager : MonoBehaviour
{
    [System.Serializable]
    public struct Pregunta
    {
        public int respuesta_correcta;
        public string problema;
    }

    [System.Serializable]
    public struct listaPreguntas
    {
        // 1. FIXED: Renamed to 'items' to match the JSON key from Express
        public Pregunta[] items; 
    }

    public static PreguntaManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textoPregunta;

    public List<Pregunta> preguntasUnity = new List<Pregunta>();
    public Pregunta PreguntaActual;

    private int tutorialIndex;

    private static readonly Pregunta[] preguntasTutorial = new Pregunta[]
    {
        new Pregunta { problema = "2 + 3", respuesta_correcta = 7 },
        new Pregunta { problema = "4 + 1", respuesta_correcta = 5 },
        new Pregunta { problema = "3 * 4", respuesta_correcta = 12 },
    };

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
        if (GameSession.Instance != null && GameSession.Instance.IsTutorial)
        {
            CargarPreguntasTutorial();
            return;
        }

        RequestQuestions();
    }

    private void RequestQuestions()
    {
        StartCoroutine(GetQuestions());
    }
    IEnumerator GetQuestions()
    {

        string url = GameSession.Instance.GetEndpoint();

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Connection success");
                string textoPlano = www.downloadHandler.text;
                Debug.Log("Downloaded JSON: " + textoPlano);

                listaPreguntas preguntasLista = JsonUtility.FromJson<listaPreguntas>(textoPlano);

                // Check if items were actually found to prevent crashes
                if (preguntasLista.items != null)
                {
                    // 2. FIXED: Changed <= to < to prevent IndexOutOfRangeException
                    for (int i = 0; i < preguntasLista.items.Length; i++)
                    {
                        preguntasUnity.Add(preguntasLista.items[i]);
                    }

                    // 3. FIXED: Now we load the question AFTER the list is populated
                    CargarPreguntaAleatoria();
                }
                else
                {
                    Debug.LogError("JSON was parsed, but 'items' array was null. Check your JSON format!");
                }
            }
            else
            {
                Debug.LogError("Network Error: " + www.error);
            }
        } // The 'using' statement automatically handles www.Dispose() for you
    }

    public void CargarPreguntaAleatoria()
    {
        // if (preguntasUnity == null || preguntasUnity.Count == 0)
        // {
        //     Debug.LogError("PreguntaManager: banco vacío o no asignado.");
        //     return;
        // }
        
        PreguntaActual = preguntasUnity[Random.Range(0, preguntasUnity.Count)]; 
        
        if (textoPregunta != null)
        {
            textoPregunta.text = PreguntaActual.problema;
        }
    }

    public bool VerificarRespuesta(int resultado)
    {
        if (string.IsNullOrEmpty(PreguntaActual.problema)) return false;
        return resultado == PreguntaActual.respuesta_correcta;
    }

    private void CargarPreguntasTutorial()
    {
        preguntasUnity.Clear();
        preguntasUnity.AddRange(preguntasTutorial);
        tutorialIndex = 0;
        CargarPreguntaTutorial(0);
    }

    public void CargarPreguntaTutorial(int index)
    {
        tutorialIndex = Mathf.Clamp(index, 0, preguntasTutorial.Length - 1);
        PreguntaActual = preguntasTutorial[tutorialIndex];

        if (textoPregunta != null)
            textoPregunta.text = PreguntaActual.problema;
    }
}