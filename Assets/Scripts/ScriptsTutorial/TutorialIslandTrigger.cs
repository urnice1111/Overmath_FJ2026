using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialIslandTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialButtonUI;
    [SerializeField] private GameObject completedBadge;
    [SerializeField] private string gameSceneName = "OperandosEnterosScene";

    private bool playerInRange;

    private void Start()
    {
        tutorialButtonUI.SetActive(false);
        if (completedBadge != null)
            completedBadge.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        bool alreadyDone = PlayerPrefs.GetInt("TutorialCompletado", 0) == 1;

        tutorialButtonUI.SetActive(true);
        if (completedBadge != null)
            completedBadge.SetActive(alreadyDone);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        tutorialButtonUI.SetActive(false);
        if (completedBadge != null)
            completedBadge.SetActive(false);
    }

    public void StartTutorial()
    {
        if (!playerInRange) return;

        GameSession.Instance.IsTutorial = true;
        GameSession.Instance.SetIsla("tutorial");
        GameSession.Instance.SetDificultad(Dificultad.Facil);

        if (ScreenFadereManager.Instance != null)
            ScreenFadereManager.Instance.ChangeScene(gameSceneName);
        else
            SceneManager.LoadScene(gameSceneName);
    }
}
