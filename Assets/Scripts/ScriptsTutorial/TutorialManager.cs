using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private int pasoActual = -1;
    private TutorialStep stepActivo;
    private bool esperandoTap;
    private bool esperandoEvaluacion;
    private bool ultimaRespuestaCorrecta;
    private Coroutine coroutineAvance;

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
        if (GameSession.Instance == null || !GameSession.Instance.IsTutorial)
        {
            panelDialogue.SetActive(false);
            enabled = false;
            return;
        }

        IniciarTutorial();
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

        if (!string.IsNullOrEmpty(stepActivo.highlightTargetName))
            spotlight.Show(stepActivo.highlightTargetName);
        else
            spotlight.Show(null);

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

    private IEnumerator EsperarSeleccion(int requeridos)
    {
        while (DragSelectionManager.Instance == null
            || DragSelectionManager.Instance.TotalSeleccionados < requeridos)
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
            if (ExpressionEvaluator.Instance != null)
            {
                var slotParent = ExpressionEvaluator.Instance.GetComponentInChildren<RectTransform>();
                if (slotParent != null)
                {
                    var slots = slotParent.GetComponentsInChildren<DropSlot>();
                    if (slots != null && slots.Length > 0)
                    {
                        bool allFilled = true;
                        foreach (var s in slots)
                        {
                            if (s.EstaVacio) { allFilled = false; break; }
                        }
                        if (allFilled) break;
                    }
                }
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
    }

    private void TerminarTutorial()
    {
        TutorialActivo = false;
        panelDialogue.SetActive(false);
        spotlight.Hide();

        PlayerPrefs.SetInt("TutorialCompletado", 1);
        PlayerPrefs.Save();

        if (GameSession.Instance != null)
            GameSession.Instance.IsTutorial = false;

        if (ScreenFadereManager.Instance != null)
            ScreenFadereManager.Instance.ChangeScene(escenaMapa);
        else
            SceneManager.LoadScene(escenaMapa);
    }
}
