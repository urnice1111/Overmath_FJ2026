using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager Instance { get; private set; }
    public static bool TutorialActivo { get; private set; }

    [Header("Steps (in order)")]
    [SerializeField] private List<TutorialStep> pasos;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TextMeshProUGUI textoDialogue;

    [Header("Spotlight overlay")]
    [SerializeField] private TutorialSpotlight spotlight;

    [Header("Return scene after tutorial ends")]
    [SerializeField] private string escenaMapa = "PantallaPrincipal";

    [Header("Intro dialogue (optional, needs DialogueSystem.DialogueHolder component)")]
    [SerializeField] private GameObject DialogueHolder;

    private Canvas tutorialCanvas;
    private int pasoActual = -1;
    private TutorialStep stepActivo;
    private bool esperandoTap;
    private bool esperandoEvaluacion;
    private bool ultimaRespuestaCorrecta;
    private Coroutine coroutineAvance;
    public bool isGoBackButtonPressed = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        tutorialCanvas = panelDialogue.GetComponentInParent<Canvas>(true);
    }

    private void Start()
    {
        if (GameSession.Instance == null || !GameSession.Instance.IsTutorial)
        {
            SetTutorialCanvasActive(false);
            panelDialogue.SetActive(false);
            enabled = false;
            return;
        }

        SetTutorialCanvasActive(true);

        bool hasIntro = DialogueHolder != null
            && DialogueHolder.GetComponent<DialogueSystem.DialogueHolder>() != null;

        if (hasIntro)
        {
            StartCoroutine(EjecutarIntroYEmpezarTutorial());
        }
        else
        {
            IniciarTutorial();
        }
    }

    private void SetTutorialCanvasActive(bool active)
    {
        if (tutorialCanvas != null)
            tutorialCanvas.gameObject.SetActive(active);
    }

    public void IniciarTutorial()
    {
        TutorialActivo = true;
        pasoActual = -1;
        panelDialogue.SetActive(false);
        SiguientePaso();
    }

    public void SiguientePaso()
    {
        LimpiarPasoActual();

        pasoActual++;
        if (pasoActual >= pasos.Count)
        {
            TerminarTutorial();
            return;
        }

        stepActivo = pasos[pasoActual];

        MostrarTexto(stepActivo.texto);

        if (stepActivo.hideDimmer)
            spotlight.Hide();
        else if (!string.IsNullOrEmpty(stepActivo.highlightTargetName))
            spotlight.Show(stepActivo.highlightTargetName);
        else
            spotlight.Show(null);

        spotlight.SetBlocksRaycasts(!stepActivo.allowWorldClicks);

        ConfigurarAvance(stepActivo);
    }

    private void MostrarTexto(string texto)
    {
        panelDialogue.SetActive(true);
        textoDialogue.text = texto;
    }

    private void ConfigurarAvance(TutorialStep step)
    {
        switch (step.advanceMode)
        {
            case TutorialAdvanceMode.TapToContinue:
                esperandoTap = true;
                break;

            case TutorialAdvanceMode.GoBack:
                coroutineAvance = StartCoroutine(RegresarYMoverCamara());
                break;

            case TutorialAdvanceMode.WaitForSeconds:
                coroutineAvance = StartCoroutine(EsperarYAvanzar(step.waitSeconds));
                break;

            case TutorialAdvanceMode.WaitForSelection:
                coroutineAvance = StartCoroutine(EsperarSeleccion(step.seleccionesRequeridas));
                break;

            case TutorialAdvanceMode.WaitForMesaOpen:
                coroutineAvance = StartCoroutine(EsperarMesaAbierta());
                break;

            case TutorialAdvanceMode.WaitForSlotsReady:
                coroutineAvance = StartCoroutine(EsperarSlotsLlenos());
                break;

            case TutorialAdvanceMode.WaitForEvaluate:
                esperandoEvaluacion = true;
                break;

            case TutorialAdvanceMode.WaitForCorrectAnswer:
                esperandoEvaluacion = true;
                if (PreguntaManager.Instance != null)
                    PreguntaManager.Instance.CargarPreguntaTutorial(1);
                break;
        }
    }

    private void Update()
    {
        if (!esperandoTap) return;
        if (Input.GetMouseButtonDown(0)
            || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            esperandoTap = false;
            SiguientePaso();
        }
    }

    public void NotificarEvaluacion(bool correcta)
    {
        if (!TutorialActivo || !esperandoEvaluacion) return;

        if (stepActivo.advanceMode == TutorialAdvanceMode.WaitForEvaluate)
        {
            esperandoEvaluacion = false;
            SiguientePaso();
            return;
        }

        if (stepActivo.advanceMode == TutorialAdvanceMode.WaitForCorrectAnswer)
        {
            if (correcta)
            {
                esperandoEvaluacion = false;
                SiguientePaso();
            }
            else
            {
                textoDialogue.text = "Almost! Try again, you can do it!";
            }
        }
    }

    private IEnumerator EsperarYAvanzar(float segundos)
    {
        yield return new WaitForSecondsRealtime(segundos);
        SiguientePaso();
    }

    private IEnumerator EjecutarIntroYEmpezarTutorial()
    {
        DialogueHolder.SetActive(true);
        while (DialogueHolder.activeSelf)
            yield return null;

        IniciarTutorial();
    }

    private IEnumerator RegresarYMoverCamara()
    {
        isGoBackButtonPressed = false;
        while (!isGoBackButtonPressed)
            yield return null;

        isGoBackButtonPressed = false;

        // Transform cam = Camera.main.transform;
        // Vector3 end = new Vector3(0.0f,0.0f,-0.0f);

        // cam.position = end;

        SiguientePaso();
    }


    private IEnumerator EsperarSeleccion(int requeridos)
    {
        while (DragSelectionManager.Instance == null)
            yield return null;

        int baseline = DragSelectionManager.Instance.TotalSeleccionados;

        while (DragSelectionManager.Instance.TotalSeleccionados < baseline + requeridos)
        {
            yield return null;
        }
        SiguientePaso();
    }

    private IEnumerator EsperarMesaAbierta()
    {
        ActiveMesaCreacion mesa = null;
        while (true)
        {
            if (mesa == null)
                mesa = Object.FindAnyObjectByType<ActiveMesaCreacion>();

            if (mesa != null && mesa.transform.childCount > 0
                && mesa.transform.GetChild(0).gameObject.activeSelf)
                break;
    

            yield return null;
        }
        SiguientePaso();
    }

    private IEnumerator EsperarSlotsLlenos()
    {
        while (true)
        {
            DropSlot[] slots = FindObjectsOfType<DropSlot>();
            if (slots.Length > 0)
            {
                bool allFilled = true;
                foreach (var s in slots)
                {
                    if (s.EstaVacio) { allFilled = false; break; }
                }
                if (allFilled) break;
            }
            yield return null;
        }
        SiguientePaso();
    }

    private void LimpiarPasoActual()
    {
        if (coroutineAvance != null)
        {
            StopCoroutine(coroutineAvance);
            coroutineAvance = null;
        }
        esperandoTap = false;
        esperandoEvaluacion = false;
        spotlight.SetBlocksRaycasts(true);
    }

    private void TerminarTutorial()
    {
        TutorialActivo = false;
        panelDialogue.SetActive(false);
        spotlight.Hide();
        SetTutorialCanvasActive(false);

        if (GameSession.Instance != null)
            GameSession.Instance.IsTutorial = false;

        StartCoroutine(SetTutorialCompletedAndReturn());
    }

    [System.Serializable]
    private class TutorialCompletedRequest
    {
        public int id_user;
    }

    private IEnumerator SetTutorialCompletedAndReturn()
    {
        var body = new TutorialCompletedRequest
        {
            id_user = GameSession.Instance.userId
        };

        string jsonBody = JsonUtility.ToJson(body);

        using UnityWebRequest www = UnityWebRequest.Post(
            "http://localhost:8080/set_tutorial_completed",
            jsonBody,
            "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
            Debug.Log("Tutorial marcado como completado en BD.");
        else
            Debug.LogError("Error al marcar tutorial completado: " + www.error);

        if (ScreenFadereManager.Instance != null)
            ScreenFadereManager.Instance.ChangeScene(escenaMapa);
        else
            SceneManager.LoadScene(escenaMapa);
    }
}
