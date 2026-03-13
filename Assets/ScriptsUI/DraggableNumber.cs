using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class DraggableNumber : MonoBehaviour
{
    public int numero;

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera canvasCamera;
    private bool dragging;
    private Vector2 dragOffset;

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
}
