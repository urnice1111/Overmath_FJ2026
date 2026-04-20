using UnityEngine;
using DG.Tweening;
using System;

public class LosePopupUI : MonoBehaviour
{
    [SerializeField] private RectTransform popupContent; // asigna aquí el panel interno

    public void Show()
    {
        gameObject.SetActive(true);

        // Estado inicial
        popupContent.localScale = Vector3.zero;
        popupContent.localRotation = Quaternion.Euler(0, 0, 90);

        // Animación visible
        popupContent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        popupContent.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void Hide(Action onComplete = null)
    {
        popupContent.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).SetUpdate(true);
        popupContent.DORotate(new Vector3(0, 0, 90), 0.3f).SetEase(Ease.InBack).SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }
}