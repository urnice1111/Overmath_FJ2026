//If fro controlling canvas menu -> esc (keyboard)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour, IPointerDownHandler
{
    public GameObject BotonMenu;
    public Button Reanudar; // Asigna el botón desde el Inspector
    [SerializeField] private string botonMenuObjectName = "BotonMenu";
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip menuClickSound;
    [SerializeField, Range(0f, 1f)] private float menuClickVolume = 1f;

    private readonly List<Button> menuButtonsWithSound = new List<Button>();
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        TryResolveMenuReferences();
        BindMenuButtonSounds();
        if (BotonMenu != null)
        {
            BotonMenu.SetActive(false);
        }
        
        // Vincula el evento click del botón
        if (Reanudar != null)
        {
            Reanudar.onClick.RemoveListener(ToggleMenu);
            Reanudar.onClick.AddListener(ToggleMenu);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (Reanudar != null)
        {
            Reanudar.onClick.RemoveListener(ToggleMenu);
        }

        UnbindMenuButtonSounds();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryResolveMenuReferences();
        BindMenuButtonSounds();
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

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayMenuClickSound();
        ToggleMenu();
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

    private void BindMenuButtonSounds()
    {
        UnbindMenuButtonSounds();

        if (BotonMenu == null)
        {
            return;
        }

        Button[] buttons = BotonMenu.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            if (button == null)
            {
                continue;
            }

            button.onClick.AddListener(PlayMenuClickSound);
            menuButtonsWithSound.Add(button);
        }
    }

    private void UnbindMenuButtonSounds()
    {
        for (int i = 0; i < menuButtonsWithSound.Count; i++)
        {
            Button button = menuButtonsWithSound[i];
            if (button == null)
            {
                continue;
            }

            button.onClick.RemoveListener(PlayMenuClickSound);
        }

        menuButtonsWithSound.Clear();
    }

    private void PlayMenuClickSound()
    {
        if (uiAudioSource == null || menuClickSound == null)
        {
            return;
        }

        uiAudioSource.PlayOneShot(menuClickSound, menuClickVolume);
    }
}
