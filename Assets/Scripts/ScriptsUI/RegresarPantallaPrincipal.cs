using UnityEngine;
using UnityEngine.SceneManagement;

public class RegresarPantallaPrincipal : MonoBehaviour
{
    public void IrAPantallaPrincipal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("PantallaPrincipal"); 
        // Asegúrate que el nombre coincida con el de tu escena
    }
    
    public void ReintentarNivel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Esto recarga la escena que está activa, reiniciando el nivel
    }
}
