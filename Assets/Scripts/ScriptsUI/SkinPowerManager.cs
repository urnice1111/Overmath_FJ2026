using UnityEngine;

public enum SkinPower { None, CongelarTiempo, SkipPregunta }

public class SkinPowerManager : MonoBehaviour
{
    public static SkinPowerManager Instance { get; private set; }

    [SerializeField] private GameObject freezeButton;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private float freezeDuration = 5f;

    private SkinPower activePower = SkinPower.None;
    private bool powerUsed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        string skinName = GameSession.Instance != null
            ? GameSession.Instance.skinSelected
            : "";

        switch (skinName)
        {
            case "azul_skin":
                activePower = SkinPower.CongelarTiempo;
                break;
            case "amarillo_skin":
                activePower = SkinPower.SkipPregunta;
                break;
            default:
                activePower = SkinPower.None;
                break;
        }

        if (freezeButton != null)
            freezeButton.SetActive(activePower == SkinPower.CongelarTiempo);
        if (skipButton != null)
            skipButton.SetActive(activePower == SkinPower.SkipPregunta);
    }

    public void UsarPoder()
    {
        if (powerUsed || activePower == SkinPower.None) return;

        powerUsed = true;

        switch (activePower)
        {
            case SkinPower.CongelarTiempo:
                ActivarCongelarTiempo();
                break;
            case SkinPower.SkipPregunta:
                ActivarSkipPregunta();
                break;
        }

        if (freezeButton != null)
            freezeButton.SetActive(false);
        if (skipButton != null)
            skipButton.SetActive(false);
    }

    private void ActivarCongelarTiempo()
    {
        if (TiempoJuego.Instance != null)
            TiempoJuego.Instance.Pausar(freezeDuration);
    }

    private void ActivarSkipPregunta()
    {
        if (PuntajedePregunta.Instance != null)
            PuntajedePregunta.Instance.RegistrarResultado(true, 1, 1,
                PreguntaManager.Instance != null ? PreguntaManager.Instance.PreguntaActual.id_pregunta : 0,
                "SKIP");

        if (PreguntaManager.Instance != null)
        {
            PreguntaManager.Instance.RegistrarIntento(
                PreguntaManager.Instance.PreguntaActual.id_pregunta,
                "SKIP",
                true
            );
            PreguntaManager.Instance.CargarPreguntaAleatoria();
        }

        if (Ranks_Manager.Instance != null)
            Ranks_Manager.Instance.RegistrarRespuesta(true);

        Transform cam = Camera.main != null ? Camera.main.transform : null;
        if (cam != null)
            cam.position = new Vector3(0.0f, -19.9f, -100f);

        DragSelectionManager.Instance?.LimpiarTodo();

        if (ExpressionEvaluator.Instance != null)
            ExpressionEvaluator.Instance.CleanChildren();

        GameObject mesaCanva = GameObject.Find("MesaCreacionCanva");
        if (mesaCanva != null)
            mesaCanva.SetActive(false);
    }
}
