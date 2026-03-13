using UnityEngine;
using UnityEngine.UI;

public class NumberSpawner : MonoBehaviour
{
    public static NumberSpawner Instance { get; private set; }

    [Header("Contenedor")]
    [Tooltip("RectTransform dentro del Canvas MesaCreacion donde aparecen los numeros")]
    [SerializeField] private RectTransform spawnParent;

    [Header("Tarjeta")]
    [SerializeField] private Vector2 tamanoTarjeta = new Vector2(90, 110);
    [SerializeField] private Color cardColor = new Color(0.18f, 0.22f, 0.36f, 1f);
    [Tooltip("Sprite opcional para la tarjeta (dejar vacio = rectangulo solido)")]
    [SerializeField] private Sprite cardSprite;

    [Header("Texto")]
    [SerializeField] private int fontSize = 52;
    [SerializeField] private Color textColor = Color.white;

    [Header("Sombra")]
    [SerializeField] private Color shadowColor = new Color(0f, 0f, 0f, 0.4f);
    [SerializeField] private Vector2 shadowDistance = new Vector2(3f, -3f);

    private int spawnCount;

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
        RestaurarNumeros();
    }

    public void RestaurarNumeros()
    {
        if (DragSelectionManager.Instance == null) return;
        if (DragSelectionManager.Instance.numerosSeleccionados.Count == 0) return;

        spawnCount = 0;
        foreach (int num in DragSelectionManager.Instance.numerosSeleccionados)
        {
            SpawnNumero(num);
        }
    }

    public void SpawnNumero(int numero)
    {
        if (spawnParent == null)
        {
            Debug.LogError("NumberSpawner: spawnParent no asignado.");
            return;
        }

        // --- Tarjeta (padre) ---
        var card = new GameObject(
            "Num_" + numero + "_" + spawnCount,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(Shadow),
            typeof(DraggableNumber)
        );
        card.transform.SetParent(spawnParent, false);

        var cardRT = card.GetComponent<RectTransform>();
        cardRT.sizeDelta = tamanoTarjeta;

        float offsetX = (spawnCount % 5) * (tamanoTarjeta.x + 10f) - 180f;
        float offsetY = (spawnCount / 5) * -(tamanoTarjeta.y + 10f) + 100f;
        cardRT.anchoredPosition = new Vector2(offsetX, offsetY);

        var cardImg = card.GetComponent<Image>();
        cardImg.color = cardColor;
        cardImg.raycastTarget = true;
        if (cardSprite != null)
        {
            cardImg.sprite = cardSprite;
            cardImg.type = Image.Type.Sliced;
        }

        var shadow = card.GetComponent<Shadow>();
        shadow.effectColor = shadowColor;
        shadow.effectDistance = shadowDistance;

        // --- Texto (hijo) ---
        var textObj = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObj.transform.SetParent(card.transform, false);

        var textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        var text = textObj.GetComponent<Text>();
        text.text = numero.ToString();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = textColor;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;

        var draggable = card.GetComponent<DraggableNumber>();
        draggable.numero = numero;

        spawnCount++;
    }
}
