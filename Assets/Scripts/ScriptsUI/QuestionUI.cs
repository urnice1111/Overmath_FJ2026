using UnityEngine;
using UnityEngine.UIElements;

public class QuestionUI : MonoBehaviour
{
    private Label questionLabel;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        questionLabel = uiDocument.rootVisualElement.Q<Label>("question-label");
    }

    public void UpdateQuestion(string texto)
    {
        if (questionLabel != null)
            questionLabel.text = texto;
    }
}