using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadPantallaPrincipal()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}