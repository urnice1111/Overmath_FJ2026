using UnityEngine;
using UnityEngine.EventSystems;

public class PowerButtonClick : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SkinPowerManager.Instance != null)
            SkinPowerManager.Instance.UsarPoder();
    }
}
