// Generates an appends child to the spawn parent for every draggable number and/or operator 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSpawner : MonoBehaviour
{
    public static NumberSpawner Instance { get; private set; }

    [Header("Contenedor de tarjetas (arriba)")]
    [SerializeField] private RectTransform spawnParent;

    [Header("Contenedor de slots (abajo)")]
    [SerializeField] private RectTransform slotParent;

    [Header("Tarjeta de numero")]
    [SerializeField] private Vector2 tamanoTarjeta = new Vector2(80, 100);
    [SerializeField] private Color cardColor = new Color(0.18f, 0.22f, 0.36f, 1f);
    [SerializeField] private Sprite cardSprite;

    [Header("Tarjeta de operador")]
    [SerializeField] private Color operadorCardColor = new Color(0.8f, 0.47f, 0.2f, 1f);

    [Header("Texto")]
    [SerializeField] private int fontSize = 44;
    [SerializeField] private Color textColor = Color.white;

    [Header("Sombra")]
    [SerializeField] private Color shadowColor = new Color(0f, 0f, 0f, 0.4f);
    [SerializeField] private Vector2 shadowDistance = new Vector2(2f, -2f);

    [Header("Slots")]
    [SerializeField] private Vector2 slotSize = new Vector2(100, 120);
    [SerializeField] private Color slotVacioColor = new Color(1f, 1f, 1f, 0.12f);
    [SerializeField] private Color slotOcupadoColor = new Color(1f, 1f, 1f, 0.25f);

    [Header("Boton Resolver")]
    [SerializeField] private Vector2 botonResolverSize = new Vector2(200, 60);
    [SerializeField] private Color botonResolverColor = new Color(0.2f, 0.7f, 0.3f, 1f);
    [SerializeField] private int botonFontSize = 30;

    private int spawnCount;
    private List<DropSlot> slotsActuales = new List<DropSlot>();

    public void LimpiarVisual()
    {
        slotsActuales.Clear();
        spawnCount = 0;

        if (spawnParent != null)
            foreach (Transform child in spawnParent)
                Destroy(child.gameObject);

        if (slotParent != null)
            foreach (Transform child in slotParent)
                Destroy(child.gameObject);
    }

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
        RestaurarItems();
    }

    public void RestaurarItems()
    {
        if (DragSelectionManager.Instance == null) return;

        spawnCount = 0;

        foreach (int num in DragSelectionManager.Instance.numerosSeleccionados)
            CrearNumero(num);

        foreach (string op in DragSelectionManager.Instance.operadoresSeleccionados)
            CrearOperador(op);

        CrearSlots();
        RestaurarAsignaciones();
    }

    public void SpawnNumero(int numero)
    {
        CrearNumero(numero);
        CrearSlots();
        RestaurarAsignaciones();
    }

    public void SpawnOperador(string operador)
    {
        CrearOperador(operador);
        CrearSlots();
        RestaurarAsignaciones();
    }

    private void CrearNumero(int numero)
    {
        if (spawnParent == null) return;

        var card = CrearTarjetaBase("Num_" + numero + "_" + spawnCount, numero.ToString(), cardColor);

        var draggable = card.GetComponent<DraggableNumber>();
        draggable.uniqueId = spawnCount;
        draggable.numero = numero;
        draggable.esOperador = false;

        spawnCount++;
    }

    private void CrearOperador(string operador)
    {
        if (spawnParent == null) return;

        var card = CrearTarjetaBase("Op_" + operador + "_" + spawnCount, operador, operadorCardColor);

        var draggable = card.GetComponent<DraggableNumber>();
        draggable.uniqueId = spawnCount;
        draggable.esOperador = true;
        draggable.simboloOperador = operador;

        spawnCount++;
    }

    private void RestaurarAsignaciones()
    {
        if (DragSelectionManager.asignacionesSlots.Count == 0) return;

        DraggableNumber[] cards = spawnParent.GetComponentsInChildren<DraggableNumber>();

        foreach (var kvp in new Dictionary<int, DragSelectionManager.SlotAssignment>(DragSelectionManager.asignacionesSlots))
        {
            int slotIdx = kvp.Key;
            var info = kvp.Value;

            if (slotIdx >= slotsActuales.Count) continue;

            DraggableNumber match = null;
            foreach (var card in cards)
            {
                if (card.transform.parent != spawnParent) continue;
                if (card.uniqueId == info.cardId)
                    { match = card; break; }
            }

            if (match != null)
            {
                slotsActuales[slotIdx].Colocar(match);
                match.transform.SetParent(slotsActuales[slotIdx].transform, false);
                match.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                match.AsignarSlot(slotsActuales[slotIdx]);
            }
        }
    }

    private GameObject CrearTarjetaBase(string nombre, string textoMostrar, Color bgColor)
    {
        var card = new GameObject(
            nombre,
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
        float offsetY = (spawnCount / 5) * -(tamanoTarjeta.y + 10f) + 200f;
        cardRT.anchoredPosition = new Vector2(offsetX, offsetY);

        var cardImg = card.GetComponent<Image>();
        cardImg.color = bgColor;
        cardImg.raycastTarget = true;
        if (cardSprite != null)
        {
            cardImg.sprite = cardSprite;
            cardImg.type = Image.Type.Sliced;
        }

        var shadow = card.GetComponent<Shadow>();
        shadow.effectColor = shadowColor;
        shadow.effectDistance = shadowDistance;

        var textObj = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObj.transform.SetParent(card.transform, false);

        var textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        var text = textObj.GetComponent<Text>();
        text.text = textoMostrar;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.color = textColor;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;

        return card;
    }

    public void CrearSlots()
    {
        if (slotParent == null) return;

        foreach (var slot in slotsActuales)
        {
            if (slot != null && slot.itemActual != null)
                slot.itemActual.transform.SetParent(spawnParent, false);
        }
        slotsActuales.Clear();

        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        if (DragSelectionManager.Instance == null) return;

        int totalSlots = DragSelectionManager.Instance.numerosSeleccionados.Count
                       + DragSelectionManager.Instance.operadoresSeleccionados.Count;

        if (totalSlots == 0) return;

        float spacing = 15f;
        float totalWidth = totalSlots * slotSize.x + (totalSlots - 1) * spacing;
        float currentX = -totalWidth / 2f;

        for (int i = 0; i < totalSlots; i++)
        {
            var slotObj = new GameObject(
                "Slot_" + i,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(Outline),
                typeof(DropSlot)
            );
            slotObj.transform.SetParent(slotParent, false);

            var slotRT = slotObj.GetComponent<RectTransform>();
            slotRT.sizeDelta = slotSize;
            slotRT.anchoredPosition = new Vector2(currentX + slotSize.x / 2f, 0f);

            var outline = slotObj.GetComponent<Outline>();
            outline.effectColor = new Color(1f, 1f, 1f, 0.3f);
            outline.effectDistance = new Vector2(2f, -2f);

            var slot = slotObj.GetComponent<DropSlot>();
            slot.Inicializar(slotVacioColor, slotOcupadoColor);
            slotsActuales.Add(slot);

            currentX += slotSize.x + spacing;
        }

        CrearBotonResolver();
    }

    private void CrearBotonResolver()
    {
        if (slotParent == null) return;

        var btnObj = new GameObject(
            "BotonResolver",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(Shadow),
            typeof(ResolverButton)
        );
        btnObj.transform.SetParent(slotParent, false);

        var btnRT = btnObj.GetComponent<RectTransform>();
        btnRT.sizeDelta = botonResolverSize;
        float slotHeight = slotSize.y;
        btnRT.anchoredPosition = new Vector2(0f, -(slotHeight / 2f + botonResolverSize.y / 2f + 20f));

        var btnImg = btnObj.GetComponent<Image>();
        btnImg.color = botonResolverColor;
        btnImg.raycastTarget = true;

        var shadow = btnObj.GetComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.4f);
        shadow.effectDistance = new Vector2(2f, -2f);

        var textObj = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObj.transform.SetParent(btnObj.transform, false);

        var textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        var text = textObj.GetComponent<Text>();
        text.text = "Resolver";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (text.font == null)
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = botonFontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        text.raycastTarget = false;
    }
}
