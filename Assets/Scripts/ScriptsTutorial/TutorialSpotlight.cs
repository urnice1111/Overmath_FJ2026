using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TutorialSpotlight : MonoBehaviour, ICanvasRaycastFilter
{
    [SerializeField] private Material dimmerMaterial;
    [SerializeField] private float animDuration = 0.25f;
    [SerializeField] private float dimAlpha = 0.6f;
    [SerializeField] private float padding = 16f;
    [SerializeField] private float worldPadding = 0.5f;
    [SerializeField] private RectTransform borderFrame;

    private Image dimmerImage;
    private Material matInstance;
    private Transform currentTargetTransform;
    private RectTransform currentTargetRT;
    private bool targetIsUI;
    private Coroutine animCoroutine;
    private Coroutine pulseCoroutine;
    private Coroutine followCoroutine;
    private Vector4 activeHoleViewport = new Vector4(-1, -1, -1, -1);
    private bool initialized;

    private static readonly int HoleRectID = Shader.PropertyToID("_HoleRect");
    private static readonly int DimAlphaID = Shader.PropertyToID("_DimAlpha");

    private void Awake()
    {
        EnsureInitialized();
        if (borderFrame != null)
            borderFrame.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void EnsureInitialized()
    {
        if (initialized) return;
        dimmerImage = GetComponent<Image>();
        matInstance = Instantiate(dimmerMaterial);
        matInstance.SetFloat(DimAlphaID, 0f);
        matInstance.SetVector(HoleRectID, new Vector4(-1, -1, -1, -1));
        initialized = true;
    }

    private void OnEnable()
    {
        EnsureInitialized();
        dimmerImage.material = matInstance;
    }

    public void Show(string targetName)
    {
        EnsureInitialized();
        gameObject.SetActive(true);
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        dimmerImage.material = matInstance;

        if (animCoroutine != null) StopCoroutine(animCoroutine);
        if (followCoroutine != null) StopCoroutine(followCoroutine);

        if (string.IsNullOrEmpty(targetName))
        {
            ClearTarget();
            activeHoleViewport = new Vector4(-1, -1, -1, -1);
            matInstance.SetVector(HoleRectID, activeHoleViewport);
            animCoroutine = StartCoroutine(AnimateDimAlpha(dimAlpha));
            HideBorder();
            return;
        }

        var obj = GameObject.Find(targetName);
        if (obj == null)
        {
            Debug.LogWarning("TutorialSpotlight: target '" + targetName + "' not found.");
            ClearTarget();
            activeHoleViewport = new Vector4(-1, -1, -1, -1);
            matInstance.SetVector(HoleRectID, activeHoleViewport);
            animCoroutine = StartCoroutine(AnimateDimAlpha(dimAlpha));
            HideBorder();
            return;
        }

        currentTargetRT = obj.GetComponent<RectTransform>();
        currentTargetTransform = obj.transform;
        targetIsUI = currentTargetRT != null;

        Vector4 target = ComputeViewportRect();
        animCoroutine = StartCoroutine(AnimateHole(target));
        followCoroutine = StartCoroutine(FollowTarget());
    }

    public void Hide()
    {
        if (!gameObject.activeSelf) return;

        if (animCoroutine != null) StopCoroutine(animCoroutine);
        if (followCoroutine != null) StopCoroutine(followCoroutine);
        if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);

        ClearTarget();
        activeHoleViewport = new Vector4(-1, -1, -1, -1);
        matInstance.SetVector(HoleRectID, activeHoleViewport);
        animCoroutine = StartCoroutine(AnimateDimAlphaAndDisable(0f));
        HideBorder();
    }

    private void ClearTarget()
    {
        currentTargetRT = null;
        currentTargetTransform = null;
    }

    private Vector4 ComputeViewportRect()
    {
        if (targetIsUI && currentTargetRT != null)
            return ComputeViewportRectUI(currentTargetRT);
        if (currentTargetTransform != null)
            return ComputeViewportRectWorld(currentTargetTransform);
        return new Vector4(-1, -1, -1, -1);
    }

    private Vector4 ComputeViewportRectUI(RectTransform target)
    {
        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        Canvas targetCanvas = target.GetComponentInParent<Canvas>();
        if (targetCanvas != null) targetCanvas = targetCanvas.rootCanvas;
        Camera cam = null;
        if (targetCanvas != null && targetCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            cam = targetCanvas.worldCamera;

        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < 4; i++)
        {
            Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, corners[i]);
            min = Vector2.Min(min, screen);
            max = Vector2.Max(max, screen);
        }

        min -= Vector2.one * padding;
        max += Vector2.one * padding;

        return new Vector4(
            min.x / Screen.width,
            min.y / Screen.height,
            max.x / Screen.width,
            max.y / Screen.height
        );
    }

    private Vector4 ComputeViewportRectWorld(Transform target)
    {
        Camera cam = Camera.main;
        if (cam == null) return new Vector4(-1, -1, -1, -1);

        Bounds bounds = new Bounds(target.position, Vector3.zero);
        bool hasBounds = false;

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            if (!hasBounds) { bounds = r.bounds; hasBounds = true; }
            else bounds.Encapsulate(r.bounds);
        }

        if (!hasBounds)
        {
            Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>();
            foreach (var c in colliders)
            {
                if (!hasBounds) { bounds = c.bounds; hasBounds = true; }
                else bounds.Encapsulate(c.bounds);
            }
        }

        if (!hasBounds)
            bounds = new Bounds(target.position, Vector3.one * worldPadding * 2f);

        Vector3[] worldCorners = new Vector3[4];
        worldCorners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
        worldCorners[1] = new Vector3(bounds.min.x, bounds.max.y, bounds.center.z);
        worldCorners[2] = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);
        worldCorners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);

        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < 4; i++)
        {
            Vector2 screen = cam.WorldToScreenPoint(worldCorners[i]);
            min = Vector2.Min(min, screen);
            max = Vector2.Max(max, screen);
        }

        min -= Vector2.one * padding;
        max += Vector2.one * padding;

        return new Vector4(
            min.x / Screen.width,
            min.y / Screen.height,
            max.x / Screen.width,
            max.y / Screen.height
        );
    }

    private IEnumerator AnimateHole(Vector4 targetRect)
    {
        float startAlpha = matInstance.GetFloat(DimAlphaID);
        Vector4 startRect = activeHoleViewport;

        Vector2 center = new Vector2(
            (targetRect.x + targetRect.z) * 0.5f,
            (targetRect.y + targetRect.w) * 0.5f
        );
        if (startRect.x < 0)
            startRect = new Vector4(center.x, center.y, center.x, center.y);

        float t = 0f;
        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / animDuration);

            float alpha = Mathf.Lerp(startAlpha, dimAlpha, p);
            matInstance.SetFloat(DimAlphaID, alpha);

            activeHoleViewport = Vector4.Lerp(startRect, targetRect, p);
            matInstance.SetVector(HoleRectID, activeHoleViewport);

            yield return null;
        }

        matInstance.SetFloat(DimAlphaID, dimAlpha);
        activeHoleViewport = targetRect;
        matInstance.SetVector(HoleRectID, activeHoleViewport);
        ShowBorder();
    }

    private IEnumerator FollowTarget()
    {
        while (currentTargetTransform != null)
        {
            Vector4 rect = ComputeViewportRect();
            activeHoleViewport = rect;
            matInstance.SetVector(HoleRectID, rect);
            UpdateBorderPosition();
            yield return null;
        }
    }

    private IEnumerator AnimateDimAlpha(float target)
    {
        float start = matInstance.GetFloat(DimAlphaID);
        float t = 0f;
        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.SmoothStep(0f, 1f, t / animDuration);
            matInstance.SetFloat(DimAlphaID, Mathf.Lerp(start, target, p));
            yield return null;
        }
        matInstance.SetFloat(DimAlphaID, target);
    }

    private IEnumerator AnimateDimAlphaAndDisable(float target)
    {
        yield return AnimateDimAlpha(target);
        gameObject.SetActive(false);
    }

    private void ShowBorder()
    {
        if (borderFrame == null) return;
        borderFrame.gameObject.SetActive(true);
        UpdateBorderPosition();
        if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
        pulseCoroutine = StartCoroutine(PulseBorder());
    }

    private void HideBorder()
    {
        if (borderFrame == null) return;
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }
        borderFrame.gameObject.SetActive(false);
    }

    private void UpdateBorderPosition()
    {
        if (borderFrame == null || activeHoleViewport.x < 0) return;

        float screenW = Screen.width;
        float screenH = Screen.height;

        Vector2 minScreen = new Vector2(activeHoleViewport.x * screenW, activeHoleViewport.y * screenH);
        Vector2 maxScreen = new Vector2(activeHoleViewport.z * screenW, activeHoleViewport.w * screenH);

        RectTransform parentRT = borderFrame.parent as RectTransform;
        Canvas parentCanvas = borderFrame.GetComponentInParent<Canvas>().rootCanvas;
        Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

        Vector2 localMin, localMax;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, minScreen, cam, out localMin);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, maxScreen, cam, out localMax);

        Vector2 ctr = (localMin + localMax) * 0.5f;
        Vector2 size = localMax - localMin;

        borderFrame.anchoredPosition = ctr;
        borderFrame.sizeDelta = size + Vector2.one * 8f;
    }

    private IEnumerator PulseBorder()
    {
        var img = borderFrame.GetComponent<Image>();
        if (img == null) yield break;

        Color baseColor = new Color(1f, 0.85f, 0f, 0.7f);
        float t = 0f;
        while (true)
        {
            t += Time.unscaledDeltaTime * 2.5f;
            float a = Mathf.Lerp(0.4f, 0.9f, (Mathf.Sin(t) + 1f) * 0.5f);
            img.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            yield return null;
        }
    }

    public void SetBlocksRaycasts(bool blocks)
    {
        EnsureInitialized();
        dimmerImage.raycastTarget = blocks;
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (activeHoleViewport.x < 0) return true;

        float vpX = screenPoint.x / Screen.width;
        float vpY = screenPoint.y / Screen.height;

        bool insideHole = vpX >= activeHoleViewport.x && vpX <= activeHoleViewport.z
                       && vpY >= activeHoleViewport.y && vpY <= activeHoleViewport.w;

        return !insideHole;
    }
}
