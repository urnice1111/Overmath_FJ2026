using UnityEngine;
using UnityEngine.UIElements;

public class PanelNumerosUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void OnEnable()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        var root = uiDocument.rootVisualElement;

        ConfigurarBoton(root, "Boton1", 1);
        ConfigurarBoton(root, "Boton2", 2);
        ConfigurarBoton(root, "Boton3", 3);
        ConfigurarBoton(root, "Boton4", 4);
        ConfigurarBoton(root, "Boton5", 5);
        ConfigurarBoton(root, "Boton6", 6);
        ConfigurarBoton(root, "Boton7", 7);
        ConfigurarBoton(root, "Boton8", 8);
        ConfigurarBoton(root, "Boton9", 9);
        ConfigurarBoton(root, "Boton0", 0);

        ConfigurarBotonOperador(root, "BotonSuma", "+");
    }

    private void ConfigurarBoton(VisualElement root, string nombreBoton, int numero)
    {
        Button boton = root.Q<Button>(nombreBoton);
        if (boton == null)
        {
            Debug.LogWarning("No se encontro el boton: " + nombreBoton);
            return;
        }

        boton.clicked += () =>
        {
            if (DragSelectionManager.Instance == null) return;
            if (!DragSelectionManager.Instance.PuedeAgregar()) return;

            DragSelectionManager.Instance.AgregarNumero(numero);
            if (NumberSpawner.Instance != null)
                NumberSpawner.Instance.SpawnNumero(numero);
        };
    }

    private void ConfigurarBotonOperador(VisualElement root, string nombreBoton, string operador)
    {
        Button boton = root.Q<Button>(nombreBoton);
        if (boton == null)
        {
            Debug.LogWarning("No se encontro el boton: " + nombreBoton);
            return;
        }

        boton.clicked += () =>
        {
            if (DragSelectionManager.Instance == null) return;
            if (!DragSelectionManager.Instance.PuedeAgregar()) return;

            DragSelectionManager.Instance.AgregarOperador(operador);
            if (NumberSpawner.Instance != null)
                NumberSpawner.Instance.SpawnOperador(operador);
        };
    }
}