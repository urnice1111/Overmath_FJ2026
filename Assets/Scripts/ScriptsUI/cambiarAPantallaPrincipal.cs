using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiarAPantallaPrincipal : MonoBehaviour
{
    void Update()
        {
            // Detecta clic con mouse
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("PantallaPrincipal"); 
            }
    
            // Detecta toque en pantalla (para móvil)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                SceneManager.LoadScene("PantallaPrincipal");
            }
        }
}
