using UnityEngine;
using UnityEngine.UIElements;

public class UIDocumentPassthrough : MonoBehaviour
{
    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        var root = uiDocument.rootVisualElement;
        root.pickingMode = PickingMode.Ignore;

        // Also ignore picking on ALL children recursively
        root.Query<VisualElement>().ForEach(el =>
        {
            el.pickingMode = PickingMode.Ignore;
        });
    }
}