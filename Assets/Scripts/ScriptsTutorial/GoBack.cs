// This script activates the crafting table 
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoBack : MonoBehaviour, IPointerDownHandler
{
    private Vector3 originalScale;

    private float shrinkDuration = 0.08f;

    private float growDuration = 0.12f;

    private void Start()
    {
        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }

        originalScale = transform.localScale;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TutorialManager.Instance.isGoBackButtonPressed = true;
        Debug.Log("This button was pressed");

        StartCoroutine(PulseScale());
    }

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
