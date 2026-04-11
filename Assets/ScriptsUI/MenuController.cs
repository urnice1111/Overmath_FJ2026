using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject BotonMenu;
    public Button Reanudar; // Asigna el botón desde el Inspector
    
    private void Start()
    {
        BotonMenu.SetActive(false);
        
        // Vincula el evento click del botón
        if (Reanudar != null)
        {
            Reanudar.onClick.AddListener(ToggleMenu);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
    
    private void ToggleMenu()
    {
        BotonMenu.SetActive(!BotonMenu.activeSelf);
        pauseController.SetPause(BotonMenu.activeSelf);
        Debug.Log("Menu toggled. Game paused: " + pauseController.isGamePuased);
    }
}
