using UnityEngine;
using UnityEngine.UIElements;

public class UIPassthrough : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.pickingMode = PickingMode.Ignore;
    }
}