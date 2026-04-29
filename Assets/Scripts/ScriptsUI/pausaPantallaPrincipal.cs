using UnityEngine;
using UnityEngine.UI;

public class pausaPantallaPrincipal : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;   // Asigna tu Canvas de menú en el Inspector
    [SerializeField] private Button pauseButton;      // Asigna el botón de pausa en el Inspector

    private void Start()
    {
        if (menuCanvas != null)
        {
            menuCanvas.gameObject.SetActive(false); // Asegura que el menú esté oculto al inicio
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OpenMenu);
        }
    }

    private void OpenMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.gameObject.SetActive(true); // Activas el GameObject del Canvas
            Time.timeScale = 0f;        // Pausa el juego
        }
    }

    public void CloseMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.gameObject.SetActive(false); // Oculta el menú
            Time.timeScale = 1f;         // Reanuda el juego
        }
    }
}
