using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class ResolverButton : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera canvasCamera;

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

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = mouse.position.ReadValue();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, canvasCamera))
            {
                Debug.Log("ResolverButton: Click detectado");
                if (ExpressionEvaluator.Instance != null)
                    ExpressionEvaluator.Instance.IntentarEvaluar();
                else
                    Debug.LogError("ResolverButton: ExpressionEvaluator.Instance es null");
            }
        }
    }
}
