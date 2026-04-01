using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class DraggableNumber : MonoBehaviour
{
    public int numero;
    public bool esOperador;
    public string simboloOperador;

    [SerializeField] private float doubleClickTime = 0.3f;

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera canvasCamera;
    private bool dragging;
    private Vector2 dragOffset;
    private float lastClickTime;

    private Vector2 posicionOriginal;
    private Transform parentOriginal;
    private DropSlot slotActual;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            canvasCamera = parentCanvas.worldCamera;
    }

    private void Start()
    {
        posicionOriginal = rectTransform.anchoredPosition;
        parentOriginal = transform.parent;
    }

    private void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        Vector2 mousePos = mouse.position.ReadValue();

        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                    rectTransform, mousePos, canvasCamera))
            {
                float timeSinceLastClick = Time.time - lastClickTime;

                if (timeSinceLastClick <= doubleClickTime)
                {
                    OnDoubleClick();
                    lastClickTime = 0f;
                    return;
                }

                lastClickTime = Time.time;

                if (slotActual != null)
                {
                    slotActual.Liberar();
                    slotActual = null;
                }

                dragging = true;
                transform.SetParent(parentOriginal, true);
                transform.SetAsLastSibling();

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform.parent as RectTransform,
                    mousePos,
                    canvasCamera,
                    out Vector2 localMouse);

                dragOffset = rectTransform.anchoredPosition - localMouse;
            }
        }

        if (dragging && mouse.leftButton.isPressed)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                mousePos,
                canvasCamera,
                out Vector2 localMouse);

            rectTransform.anchoredPosition = localMouse + dragOffset;
        }

        if (mouse.leftButton.wasReleasedThisFrame && dragging)
        {
            dragging = false;
            IntentarColocarEnSlot(mousePos);
        }
    }

    private void IntentarColocarEnSlot(Vector2 screenPos)
    {
        DropSlot[] slots = FindObjectsByType<DropSlot>(FindObjectsSortMode.None);

        foreach (var slot in slots)
        {
            RectTransform slotRT = slot.GetComponent<RectTransform>();
            if (slotRT == null) continue;

            if (!RectTransformUtility.RectangleContainsScreenPoint(slotRT, screenPos, canvasCamera))
                continue;

            if (!slot.PuedeAceptar(this))
                continue;

            slot.Colocar(this);
            slotActual = slot;

            transform.SetParent(slot.transform, false);
            rectTransform.anchoredPosition = Vector2.zero;

            return;
        }

        rectTransform.anchoredPosition = posicionOriginal;
    }

    public void AsignarSlot(DropSlot slot)
    {
        slotActual = slot;
    }

    private void OnDoubleClick()
    {
        if (slotActual != null)
            slotActual.Liberar();

        if (DragSelectionManager.Instance != null)
        {
            if (esOperador)
                DragSelectionManager.Instance.QuitarOperador(simboloOperador);
            else
                DragSelectionManager.Instance.QuitarNumero(numero);
        }

        Destroy(gameObject);
    }
}
