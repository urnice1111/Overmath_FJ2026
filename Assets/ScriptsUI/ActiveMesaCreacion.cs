using UnityEngine;
using UnityEngine.UIElements;

public class ActiveMesaCreacion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private UIDocument uiDocument;

    [SerializeField] private MesaCreacionScript mesaCreacion;

    private Button botonActivatePopUp;



    private void OnEnable()
    {
        
        uiDocument = GetComponent<UIDocument>();

        var popUp = uiDocument.rootVisualElement;

        if (mesaCreacion == null)
            mesaCreacion = FindAnyObjectByType<MesaCreacionScript>(FindObjectsInactive.Include);

        botonActivatePopUp = popUp.Q<Button>("MesaCreacion");
        botonActivatePopUp.RegisterCallback<ClickEvent>(activatePopUp);


    }


    private void activatePopUp(ClickEvent evt)
    {
        if (mesaCreacion == null)
        {
            Debug.Log("nulo");
            return;
        }
        mesaCreacion.AbrirMesaCreacion();
    }

}
