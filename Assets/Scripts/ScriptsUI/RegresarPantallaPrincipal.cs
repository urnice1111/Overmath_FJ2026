using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;


public class RegresarPantallaPrincipal : MonoBehaviour
{
    public void IrAPantallaPrincipal()
    {
        Time.timeScale = 1;
        /*FindObjectOfType<LosePopupUI>().Hide(() =>
            SceneManager.LoadScene("PantallaPrincipal"));*/
        SceneManager.LoadScene("PantallaPrincipal");
    }

    public void ReintentarNivel()
    {
        Time.timeScale = 1;
        //FindObjectOfType<LosePopupUI>().Hide(() =>
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name));        // Cuando termine la animación, recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
}
