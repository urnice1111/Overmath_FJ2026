using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHighlight : MonoBehaviour
{
    private GameObject glowOverlay;
    private Coroutine pulseCoroutine;

    public void Activar()
    {
        if (glowOverlay != null) return;

        glowOverlay = new GameObject("TutorialGlow", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        glowOverlay.transform.SetParent(transform, false);

        var rt = glowOverlay.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(-8, -8);
        rt.offsetMax = new Vector2(8, 8);

        var img = glowOverlay.GetComponent<Image>();
        img.color = new Color(1f, 0.85f, 0f, 0.4f);
        img.raycastTarget = false;

        pulseCoroutine = StartCoroutine(PulseGlow(img));
    }

    public void Desactivar()
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);
        pulseCoroutine = null;

        if (glowOverlay != null)
            Destroy(glowOverlay);
        glowOverlay = null;
    }

    private IEnumerator PulseGlow(Image img)
    {
        float t = 0f;
        while (true)
        {
            t += Time.unscaledDeltaTime * 2f;
            float alpha = Mathf.Lerp(0.2f, 0.5f, (Mathf.Sin(t) + 1f) / 2f);
            img.color = new Color(1f, 0.85f, 0f, alpha);
            yield return null;
        }
    }
}
