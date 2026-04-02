using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    public GameObject playButtonUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playButtonUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playButtonUI.SetActive(false);
        }
    }
}
