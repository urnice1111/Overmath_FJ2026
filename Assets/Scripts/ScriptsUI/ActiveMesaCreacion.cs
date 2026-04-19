// This script activates the crafting table 
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveMesaCreacion : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("Clicked");
    }

    public void Cerrar()
    {
        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(false);
    }
}
