using UnityEngine;
using UnityEngine.EventSystems;

public class BotonNumeroClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int numero;
    [SerializeField] private bool esOperador;
    [SerializeField] private string simboloOperador;

    private void Start()
    {
        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (esOperador)
        {
            if (DragSelectionManager.Instance == null) return;
            if (!DragSelectionManager.Instance.PuedeAgregar()) return;
            DragSelectionManager.Instance.AgregarOperador(simboloOperador);
            if (NumberSpawner.Instance != null)
                NumberSpawner.Instance.SpawnOperador(simboloOperador);
        }
        else
        {
            if (DragSelectionManager.Instance == null) return;
            if (!DragSelectionManager.Instance.PuedeAgregar()) return;
            DragSelectionManager.Instance.AgregarNumero(numero);
            if (NumberSpawner.Instance != null)
                NumberSpawner.Instance.SpawnNumero(numero);
        }
    }
}