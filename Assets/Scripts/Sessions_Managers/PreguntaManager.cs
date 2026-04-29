// Manages the questions, including fetching them from the Lambda function.
using UnityEngine;
using TMPro;  
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PreguntaManager : MonoBehaviour
{
    [SerializeField] private UIDocument questionUIDocument;
    private Label questionLabel;
    [System.Serializable]
    public struct Pregunta
    {
        public int id_pregunta;
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

    private List<int> shuffledIndices = new List<int>();
    private int shufflePosition;
    public int PreguntaIndexActual => shufflePosition;
    private int tutorialIndex;

    private static readonly Pregunta[] preguntasTutorial = new Pregunta[]
    {
        new Pregunta { problema = "2 + 5", respuesta_correcta = 7 },
        new Pregunta { problema = "3 * 4", respuesta_correcta = 12 },
    };
    
    public List<ProgressHandler.IntentoPregunta> intentos = new List<ProgressHandler.IntentoPregunta>();
    private float tiempoInicioPregunta;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private Label GetQuestionLabel()
    {
        if (questionLabel == null && questionUIDocument != null)
            questionLabel = questionUIDocument.rootVisualElement?.Q<Label>("question-label");
        return questionLabel;
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

                textoPlano = "{\"items\":" + textoPlano + "}";
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
        if (preguntasUnity.Count == 0) return;

        if (shufflePosition >= shuffledIndices.Count)
            ShuffleIndices();

        PreguntaActual = preguntasUnity[shuffledIndices[shufflePosition]];
        shufflePosition++;
        
        tiempoInicioPregunta = Time.time; // ⏱ inicio del cronómetro

        Label lbl = GetQuestionLabel();
        if (lbl != null)
            lbl.text = PreguntaActual.respuesta_correcta.ToString();

        if (textoPregunta != null)
            textoPregunta.text = PreguntaActual.respuesta_correcta.ToString();
    }

    private void ShuffleIndices()
    {
        shuffledIndices.Clear();
        for (int i = 0; i < preguntasUnity.Count; i++)
            shuffledIndices.Add(i);

        // Fisher-Yates shuffle
        for (int i = shuffledIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[j];
            shuffledIndices[j] = tmp;
        }

        shufflePosition = 0;
    }

    public bool VerificarRespuesta(int resultado)
    {
        Debug.Log("VerificarRespuesta: resultado=" + resultado
            + " esperado=" + PreguntaActual.respuesta_correcta
            + " problema='" + PreguntaActual.problema + "'");
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

        Label lbl = GetQuestionLabel();
        if (lbl != null)
            lbl.text = PreguntaActual.respuesta_correcta.ToString();

        if (textoPregunta != null)
            textoPregunta.text = PreguntaActual.respuesta_correcta.ToString();
    }
    
    public void RegistrarIntento(int idPregunta, string respuestaUsuario, bool esCorrecta)
    {
        float tiempoRespuesta = Time.time - tiempoInicioPregunta;

        var intento = new ProgressHandler.IntentoPregunta
        {
            id_pregunta = idPregunta,
            respuesta_usuario = respuestaUsuario,
            es_correcto = esCorrecta,
            tiempo_respuesta_seg = tiempoRespuesta
        };

        intentos.Add(intento);
    }

    
}