using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public string OperandosEnterosScene;

    public void LoadLevel()
    {
        SceneManager.LoadScene(OperandosEnterosScene);
    }
}
