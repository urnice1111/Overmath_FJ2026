using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiaEscena : MonoBehaviour
{
    public string sceneToLoad;

    public Transform barco;

    public bool guardarPosicionAntesDeCambiar = false;

    public void LoadNuevaEscena()
    {
        if (guardarPosicionAntesDeCambiar && barco != null)
        {
            GameData.posicionBarco = barco.position;
            GameData.hayPosicionGuardada = true;
            
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}