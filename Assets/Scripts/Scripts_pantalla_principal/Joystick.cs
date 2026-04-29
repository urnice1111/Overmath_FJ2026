// This script manages the joystick and updates barquito position.
using UnityEngine;
using UnityEngine.EventSystems;
using DialogueSystem;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    private Vector2 input = Vector2.zero;
    private CanvasGroup canvasGroup;

    public Vector2 GetInput()
    {
        return input;
    }

    private void Awake()
    {
        if (background != null)
        {
            canvasGroup = background.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = background.gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Update()
    {
        if (canvasGroup == null) return;
        bool dialog = DialogueHolder.IsDialogueActive;
        canvasGroup.alpha = dialog ? 0f : 1f;
        canvasGroup.interactable = !dialog;
        canvasGroup.blocksRaycasts = !dialog;

        if (dialog)
        {
            input = Vector2.zero;
            if (handle != null)
                handle.anchoredPosition = Vector2.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DialogueHolder.IsDialogueActive) return;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );
        pos.x = pos.x / background.sizeDelta.x;
        pos.y = pos.y / background.sizeDelta.y;

        input = new Vector2(pos.x * 2, pos.y * 2);
        input = input.magnitude > 1 ? input.normalized : input;

        handle.anchoredPosition = new Vector2(
            input.x * (background.sizeDelta.x / 2),
            input.y * (background.sizeDelta.y / 2)
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DialogueHolder.IsDialogueActive) return;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
