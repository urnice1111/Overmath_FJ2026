using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiaEscena : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadNuevaEscena()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}