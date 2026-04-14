using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickDetector : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Debug.Log("Double Clicked: " + gameObject.name);
            // Perform your double-click action here
        }
        else if (eventData.clickCount == 1)
        {
            Debug.Log("Single Clicked: " + gameObject.name);
        }
    }
}
