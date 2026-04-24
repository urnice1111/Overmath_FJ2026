// Displays the result 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultadoDisplay : MonoBehaviour
{
    [SerializeField] private float duracionDisplay = 4f;
    [SerializeField] private string escenaRetorno = "OperandosEnterosScene";
    [SerializeField] private bool volverAutomaticamente = true;
    [SerializeField] private bool permitirVolverConClick = true;
    [SerializeField] private int fontSizeResultado = 90;
    [SerializeField] private Color colorTexto = Color.white;

    [SerializeField] private VillianState villianState;

    private Coroutine coroutineRetorno;
    private bool retornoEjecutado;

    private void Start()
    {
        CrearUI();
        // MakeAnimation(); 
        // if (volverAutomaticamente)
        //     coroutineRetorno = StartCoroutine(EsperarYVolver());

        
    }

    private void Update()
    {
        if (!permitirVolverConClick || retornoEjecutado)
            return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            CargarEscenaRetorno();
        }
    }

    private void CrearUI()
    {
        var canvasObj = new GameObject("ResultadoCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var textObj = new GameObject("TextoResultado", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObj.transform.SetParent(canvasObj.transform, false);

        var rt = textObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(50, 50);
        rt.offsetMax = new Vector2(-50, -50);

        var text = textObj.GetComponent<Text>();
        text.text = DragSelectionManager.ExpresionTexto ?? "Sin resultado";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSizeResultado;
        text.color = colorTexto;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
    }

    // private IEnumerator EsperarYVolver()
    // {
    //     yield return new WaitForSeconds(duracionDisplay);
    //     CargarEscenaRetorno();
    // }

    public void CargarEscenaRetorno()
    {
        if (retornoEjecutado)
            return;

        if (string.IsNullOrWhiteSpace(escenaRetorno))
        {
            Debug.LogError("[ResultadoDisplay] escenaRetorno esta vacia.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(escenaRetorno))
        {
            Debug.LogError($"[ResultadoDisplay] No se puede cargar la escena '{escenaRetorno}'. Verifica que este en Build Settings.");
            return;
        }

        retornoEjecutado = true;

        if (coroutineRetorno != null)
        {
            StopCoroutine(coroutineRetorno);
            coroutineRetorno = null;
        }

        SceneManager.LoadScene(escenaRetorno);
    }

    private void MakeAnimation()
    {
        // Si no está asignado, buscarlo en la escena
        if (villianState == null)
        {
            VillianState[] villains = FindObjectsOfType<VillianState>();
            
            if (villains.Length == 0)
            {
                Debug.LogWarning("ResultadoDisplay: No se encontro ningun VillianState en la escena.");
                return;
            }
            
            if (villains.Length > 1)
            {
                Debug.LogWarning($"ResultadoDisplay: Se encontraron {villains.Length} villanos. Usando el primero activo.");
                // Priorizar el activo
                foreach (var villain in villains)
                {
                    if (villain.gameObject.activeSelf)
                    {
                        villianState = villain;
                        break;
                    }
                }
                // Si no hay ninguno activo, usar el primero
                if (villianState == null)
                    villianState = villains[0];
            }
            else
            {
                villianState = villains[0];
            }
        }

        if (villianState == null)
        {
            Debug.LogError("ResultadoDisplay: No se pudo obtener una referencia valida a VillianState.");
            return;
        }

        Debug.Log($"ResultadoDisplay: Reproduciendo animacion en villano: {villianState.gameObject.name}");
        if (DragSelectionManager.FueCorrecta)
        {
            villianState.MakeAnimationCorrectAnsw();
        } else
        {
            villianState.MakeAnimationWrongAnsw();
        }
    }
}
