using UnityEngine;
using UnityEngine.UI;

public class NumberSpawner : MonoBehaviour
{
    public static NumberSpawner Instance { get; private set; }

    [Tooltip("RectTransform dentro del Canvas MesaCreacion donde aparecen los numeros")]
    [SerializeField] private RectTransform spawnParent;

    [SerializeField] private int fontSize = 60;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private Vector2 tamanoNumero = new Vector2(80, 80);

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

    public void SpawnNumero(int numero)
    {
        if (spawnParent == null)
        {
            Debug.LogError("NumberSpawner: spawnParent no asignado.");
            return;
        }

        var obj = new GameObject(
            "Num_" + numero + "_" + spawnCount,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Text),
            typeof(DraggableNumber)
        );

        obj.transform.SetParent(spawnParent, false);

        var rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = tamanoNumero;

        float offsetX = (spawnCount % 5) * 90f - 180f;
        float offsetY = (spawnCount / 5) * -90f + 100f;
        rt.anchoredPosition = new Vector2(offsetX, offsetY);

        var text = obj.GetComponent<Text>();
        text.text = numero.ToString();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = textColor;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = true;

        var draggable = obj.GetComponent<DraggableNumber>();
        draggable.numero = numero;

        spawnCount++;
    }
}
