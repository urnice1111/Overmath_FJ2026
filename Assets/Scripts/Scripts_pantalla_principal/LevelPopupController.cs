using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPopupController : MonoBehaviour
{
    public GameObject popup;

    public void OpenPopup()
    {
        popup.SetActive(true);
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}