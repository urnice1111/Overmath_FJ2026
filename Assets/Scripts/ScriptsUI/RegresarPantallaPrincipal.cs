using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;


public class RegresarPantallaPrincipal : MonoBehaviour
{
    private void LimpiarEstadoPartida()
    {
        if (DragSelectionManager.Instance != null)
            DragSelectionManager.Instance.LimpiarTodo();

        if (NumberSpawner.Instance != null)
            NumberSpawner.Instance.LimpiarVisual();

        pauseController.SetPause(false);
    }

    public void IrAPantallaPrincipal()
    {
        LimpiarEstadoPartida();
        Time.timeScale = 1;
        /*FindObjectOfType<LosePopupUI>().Hide(() =>
            SceneManager.LoadScene("PantallaPrincipal"));*/
        SceneManager.LoadScene("PantallaPrincipal");
    }

    public void ReintentarNivel()
    {
        LimpiarEstadoPartida();
        Time.timeScale = 1;
        //FindObjectOfType<LosePopupUI>().Hide(() =>
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name));        // Cuando termine la animación, recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
}
