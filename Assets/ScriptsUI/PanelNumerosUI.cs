using UnityEngine;
using UnityEngine.UIElements;

public class PanelNumerosUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private MesaCreacionScript mesaCreacion;
    
    private Button botonActivatePopUp;

    private void OnEnable()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        var root = uiDocument.rootVisualElement;

        botonActivatePopUp = root.Q<Button>("MesaCreacion");


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
    }

    private void ConfigurarBoton(VisualElement root, string nombreBoton, int numero)
    {
        Button boton = root.Q<Button>(nombreBoton);

        if (boton == null)
        {
            Debug.LogWarning($"No se encontró el botón: {nombreBoton}");
            return;
        }

        boton.clicked += () =>
        {
            Debug.Log($"Click en {nombreBoton}");

            if (DragSelectionManager.Instance == null)
            {
                Debug.LogError("DragSelectionManager.Instance es NULL");
                return;
            }

            if(DragSelectionManager.Instance.numerosSeleccionados.Count < 5)
            {
                DragSelectionManager.Instance.AgregarNumero(numero);
            
                if (NumberSpawner.Instance != null)
                {
                    NumberSpawner.Instance.SpawnNumero(numero);
                }
            }
        };
    }
}