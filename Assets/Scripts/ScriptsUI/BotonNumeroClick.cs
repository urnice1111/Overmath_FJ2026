// This script is for the operators and numbers buttons.
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BotonNumeroClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int numero;
    [SerializeField] private bool esOperador;
    [SerializeField] private string simboloOperador;

    //Animation stuff:
    private Animator animator;
    private Vector3 originalScale;

    private float shrinkDuration = 0.08f;

    private float growDuration = 0.12f;

    private void Start()
    {
        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }

        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

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

        StartCoroutine(PulseScale());

    }

    /*
    Due to many and dumb users, the buttons must have any response when clicked.
    So changin the scale attribute of the gameobject when clicked to shrink a little bit.
    */
    
    IEnumerator PulseScale()
    {
        Vector3 from = originalScale;
        Vector3 to = originalScale * 0.70f; // slightly smaller when pressed

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(from, to, Mathf.Clamp01(t));
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / growDuration;
            transform.localScale = Vector3.Lerp(to, from, Mathf.Clamp01(t));
            yield return null;
        }

        transform.localScale = from; // exact reset
    }
}
