// Manges the difficulty for each game.
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "OperandosEnterosScene";

    public void OnFacilSelected()
    {
        GameSession.Instance.SetDificultad(Dificultad.Facil);
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnNormalSelected()
    {
        GameSession.Instance.SetDificultad(Dificultad.Normal);
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnDificilSelected()
    {
        GameSession.Instance.SetDificultad(Dificultad.Dificil);
        SceneManager.LoadScene(gameSceneName);
    }
}
