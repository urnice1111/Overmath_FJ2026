using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ActiveMesaCreacion : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("Clicked");
    }

}
