using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public static bool TutorialActivo { get; private set; }

    [Header("Pasos del tutorial (en orden)")]
    [SerializeField] private List<TutorialStep> pasos;

    [Header("UI del tutorial")]
    [SerializeField] private GameObject panelTutorial;
    [SerializeField] private TextMeshProUGUI textoTitulo;
    [SerializeField] private TextMeshProUGUI textoBody;
    [SerializeField] private GameObject botonSiguiente;

    [Header("Bloqueador de input (pantalla completa, sortOrder alto)")]
    [SerializeField] private CanvasGroup bloqueador;

    private int pasoActual = -1;
    private TutorialStep stepActivo;
    private TutorialHighlight highlightActivo;
    private bool esperandoTap;
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
        PlayerPrefs.DeleteKey("TutorialCompletado");
        
        bool yaCompletado = PlayerPrefs.GetInt("TutorialCompletado", 0) == 1;
        if (yaCompletado)
        {
            panelTutorial.SetActive(false);
            bloqueador.gameObject.SetActive(false);
            return;
        }
        IniciarTutorial();
    }

    public void IniciarTutorial()
    {
        TutorialActivo = true;
        pasoActual = -1;
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
        MostrarTexto(stepActivo.titulo, stepActivo.texto);
        ResaltarObjetivo(stepActivo.highlightTargetName);
        ConfigurarAvance(stepActivo);
    }

    private void MostrarTexto(string titulo, string cuerpo)
    {
        panelTutorial.SetActive(true);
        textoTitulo.text = titulo;
        textoBody.text = cuerpo;
    }

    private void ResaltarObjetivo(string targetName)
    {
        if (string.IsNullOrEmpty(targetName)) return;

        var obj = GameObject.Find(targetName);
        if (obj == null)
        {
            Debug.LogWarning("TutorialManager: no se encontro el objeto '" + targetName + "' para resaltar.");
            return;
        }

        highlightActivo = obj.GetComponent<TutorialHighlight>();
        if (highlightActivo == null)
            highlightActivo = obj.AddComponent<TutorialHighlight>();

        highlightActivo.Activar();
    }

    private void BloquearTodo()
    {
        bloqueador.gameObject.SetActive(true);
        bloqueador.blocksRaycasts = true;
        bloqueador.alpha = 0.6f;
    }

    private void DesbloquearTodo()
    {
        bloqueador.blocksRaycasts = false;
        bloqueador.alpha = 0f;
    }

    private void ConfigurarAvance(TutorialStep step)
    {
        switch (step.advanceMode)
        {
            case TutorialAdvanceMode.TapToContinue:
                BloquearTodo();
                botonSiguiente.SetActive(true);
                esperandoTap = true;
                break;

            case TutorialAdvanceMode.WaitForSeconds:
                BloquearTodo();
                botonSiguiente.SetActive(false);
                coroutineAvance = StartCoroutine(EsperarYAvanzar(step.waitSeconds));
                break;

            case TutorialAdvanceMode.WaitForSelection:
                DesbloquearTodo();
                botonSiguiente.SetActive(false);
                coroutineAvance = StartCoroutine(EsperarSeleccion(step.seleccionesRequeridas));
                break;

            case TutorialAdvanceMode.WaitForMesaOpen:
                DesbloquearTodo();
                botonSiguiente.SetActive(false);
                coroutineAvance = StartCoroutine(EsperarMesaAbierta());
                break;

            case TutorialAdvanceMode.WaitForSlotsReady:
                DesbloquearTodo();
                botonSiguiente.SetActive(false);
                coroutineAvance = StartCoroutine(EsperarSlotsLlenos());
                break;

            case TutorialAdvanceMode.WaitForEvaluate:
                DesbloquearTodo();
                botonSiguiente.SetActive(false);
                break;

            case TutorialAdvanceMode.WaitForResult:
                BloquearTodo();
                botonSiguiente.SetActive(false);
                coroutineAvance = StartCoroutine(EsperarYAvanzar(3f));
                break;

            case TutorialAdvanceMode.AutoRelease:
                DesbloquearTodo();
                botonSiguiente.SetActive(false);
                coroutineAvance = StartCoroutine(EsperarYAvanzar(2f));
                break;
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
        // ActiveMesaCreacion activa su hijo[0] al hacer click
        ActiveMesaCreacion mesa = null;
        while (true)
        {
            if (mesa == null)
                mesa = FindAnyObjectByType<ActiveMesaCreacion>();

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
                        bool todosLlenos = true;
                        foreach (var s in slots)
                        {
                            if (s.EstaVacio) { todosLlenos = false; break; }
                        }
                        if (todosLlenos) break;
                    }
                }
            }
            yield return null;
        }
        SiguientePaso();
    }

    public void NotificarEvaluacion()
    {
        if (!TutorialActivo) return;
        if (stepActivo != null && stepActivo.advanceMode == TutorialAdvanceMode.WaitForEvaluate)
            SiguientePaso();
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

    private void LimpiarPasoActual()
    {
        if (coroutineAvance != null)
        {
            StopCoroutine(coroutineAvance);
            coroutineAvance = null;
        }
        esperandoTap = false;
        botonSiguiente.SetActive(false);

        if (highlightActivo != null)
        {
            highlightActivo.Desactivar();
            highlightActivo = null;
        }
    }

    private void TerminarTutorial()
    {
        TutorialActivo = false;
        panelTutorial.SetActive(false);
        DesbloquearTodo();
        bloqueador.gameObject.SetActive(false);
        PlayerPrefs.SetInt("TutorialCompletado", 1);
        PlayerPrefs.Save();
        Debug.Log("TutorialManager: Tutorial completado.");
    }
}
