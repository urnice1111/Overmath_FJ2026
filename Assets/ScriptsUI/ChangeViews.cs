using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ChangeViews : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    [SerializeField] private string leftSceneName = "PrimerEscena";
    [SerializeField] private string rightSceneName = "OperandosEnterosEscena";

    private Button LeftView;
    private Button RightView;

    private void OnEnable()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("ChangeViews: No se encontro UIDocument en este objeto.");
            return;
        }

        var root = uiDocument.rootVisualElement;
        LeftView = root.Q<Button>("VistaIzq");
        RightView = root.Q<Button>("VistaDer");

        if (LeftView != null)
            LeftView.clicked += LoadLeftScene;
        else
            Debug.LogWarning("ChangeViews: No se encontro el boton LeftView.");

        if (RightView != null)
            RightView.clicked += LoadRightScene;
        else
            Debug.LogWarning("ChangeViews: No se encontro el boton RightView.");
    }

    private void OnDisable()
    {
        if (LeftView != null)
            LeftView.clicked -= LoadLeftScene;

        if (RightView != null)
            RightView.clicked -= LoadRightScene;
    }

    private void LoadLeftScene()
    {
        LoadSceneByName(leftSceneName);
    }

    private void LoadRightScene()
    {
        LoadSceneByName(rightSceneName);
    }

    private void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("ChangeViews: El nombre de escena esta vacio.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
