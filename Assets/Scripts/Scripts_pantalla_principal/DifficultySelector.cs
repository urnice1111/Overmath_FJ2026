// Manges the difficulty for each game.
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "OperandosEnterosScene";

    /*public void OnFacilSelected()
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
    }*/
    // Isla Suma
    public void OnSumaFacil()
    {
        GameSession.Instance.SetIsla("isla_suma");
        GameSession.Instance.SetDificultad(Dificultad.Facil);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnSumaNormal()
    {
        GameSession.Instance.SetIsla("isla_suma");
        GameSession.Instance.SetDificultad(Dificultad.Normal);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnSumaDificil()
    {
        GameSession.Instance.SetIsla("isla_suma");
        GameSession.Instance.SetDificultad(Dificultad.Dificil);
        SceneManager.LoadScene(gameSceneName);
    }

    // Isla Resta
    public void OnRestaFacil()
    {
        GameSession.Instance.SetIsla("isla_resta");
        GameSession.Instance.SetDificultad(Dificultad.Facil);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnRestaNormal()
    {
        GameSession.Instance.SetIsla("isla_resta");
        GameSession.Instance.SetDificultad(Dificultad.Normal);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnRestaDificil()
    {
        GameSession.Instance.SetIsla("isla_resta");
        GameSession.Instance.SetDificultad(Dificultad.Dificil);
        SceneManager.LoadScene(gameSceneName);
    }

    // Isla Multiplicación
    public void OnMultiFacil()
    {
        GameSession.Instance.SetIsla("isla_multi");
        GameSession.Instance.SetDificultad(Dificultad.Facil);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnMultiNormal()
    {
        GameSession.Instance.SetIsla("isla_multi");
        GameSession.Instance.SetDificultad(Dificultad.Normal);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnMultiDificil()
    {
        GameSession.Instance.SetIsla("isla_multi");
        GameSession.Instance.SetDificultad(Dificultad.Dificil);
        SceneManager.LoadScene(gameSceneName);
    }

    // Isla División
    public void OnDivFacil()
    {
        GameSession.Instance.SetIsla("isla_div");
        GameSession.Instance.SetDificultad(Dificultad.Facil);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnDivNormal()
    {
        GameSession.Instance.SetIsla("isla_div");
        GameSession.Instance.SetDificultad(Dificultad.Normal);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnDivDificil()
    {
        GameSession.Instance.SetIsla("isla_div");
        GameSession.Instance.SetDificultad(Dificultad.Dificil);
        SceneManager.LoadScene(gameSceneName);
    }

    // Isla Combinación
    public void OnCombFacil()
    {
        GameSession.Instance.SetIsla("isla_comb");
        GameSession.Instance.SetDificultad(Dificultad.Facil);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnCombNormal()
    {
        GameSession.Instance.SetIsla("isla_comb");
        GameSession.Instance.SetDificultad(Dificultad.Normal);
        SceneManager.LoadScene(gameSceneName);
    }
    public void OnCombDificil()
    {
        GameSession.Instance.SetIsla("isla_comb");
        GameSession.Instance.SetDificultad(Dificultad.Dificil);
        SceneManager.LoadScene(gameSceneName);
    }

    // Isla Infinito (solo un nivel)
    public void OnInfinito()
    {
        GameSession.Instance.SetIsla("isla_infinito");
        SceneManager.LoadScene(gameSceneName);
    }
}
