//If fro controlling canvas menu -> esc (keyboard)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject BotonMenu;
    public Button Reanudar; // Asigna el botón desde el Inspector
    [SerializeField] private string botonMenuObjectName = "BotonMenu";
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryResolveMenuReferences();
        if (BotonMenu != null)
        {
            BotonMenu.SetActive(false);
        }
        
        // Vincula el evento click del botón
        if (Reanudar != null)
        {
            Reanudar.onClick.AddListener(ToggleMenu);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryResolveMenuReferences();
        if (BotonMenu != null)
        {
            BotonMenu.SetActive(false);
        }
        pauseController.SetPause(false);
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
        if (BotonMenu == null)
        {
            TryResolveMenuReferences();
            if (BotonMenu == null)
            {
                Debug.LogWarning("MenuController: No se encontro BotonMenu en esta escena.");
                pauseController.SetPause(false);
                return;
            }
        }

        BotonMenu.SetActive(!BotonMenu.activeSelf);
        pauseController.SetPause(BotonMenu.activeSelf);
        Debug.Log("Menu toggled. Game paused: " + pauseController.isGamePuased);
    }

    private void TryResolveMenuReferences()
    {
        if (BotonMenu == null)
        {
            GameObject candidate = GameObject.Find(botonMenuObjectName);
            if (candidate != null)
            {
                BotonMenu = candidate;
            }
        }
    }
}
