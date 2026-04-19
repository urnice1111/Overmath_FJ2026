using UnityEngine;
using UnityEngine.UIElements;

public class LogInMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument logInDocument;
    [SerializeField] private UIDocument signInDocument;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        root.Q<Button>("BtnIngresar").clicked += OnIngresarClicked;
        root.Q<Button>("BtnRegistrarse").clicked += OnRegistrarseClicked;
     
    }

    private void OnDisable()
    {
        var root = logInDocument.rootVisualElement;
        var btnIngresar = root.Q<Button>("BtnIngresar");
        var btnRegistrarse = root.Q<Button>("BtnRegistrarse");

        if (btnIngresar != null) btnIngresar.clicked -= OnIngresarClicked;
        if (btnRegistrarse != null) btnRegistrarse.clicked -= OnRegistrarseClicked;
    }

    private void OnIngresarClicked()
    {
        gameObject.SetActive(false);
        logInDocument.gameObject.SetActive(true);
        signInDocument.gameObject.SetActive(false);
    }

    private void OnRegistrarseClicked()
    {
        gameObject.SetActive(false);
        logInDocument.gameObject.SetActive(false);
        signInDocument.gameObject.SetActive(true);
    }
}