using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class DraggableNumber : MonoBehaviour
{
    public int numero;

    [SerializeField] private float doubleClickTime = 0.3f;

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera canvasCamera;
    private bool dragging;
    private Vector2 dragOffset;
    private float lastClickTime;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            canvasCamera = parentCanvas.worldCamera;
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

                OnSingleClick();
                lastClickTime = Time.time;

                dragging = true;
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

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            dragging = false;
        }
    }

    private void OnSingleClick()
    {
        Debug.Log("Click en: " + gameObject.name);
    }

    private void OnDoubleClick()
    {
        if (DragSelectionManager.Instance != null)
            DragSelectionManager.Instance.QuitarNumero(numero);

        Destroy(gameObject);
    }
}
