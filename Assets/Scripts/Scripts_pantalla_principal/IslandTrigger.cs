using System;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    public GameObject playButtonUI;
    [SerializeField] private string islandName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            // Get and set the island name from the gameobjet name when trigger
            GameSession.Instance.SetIsla(islandName);
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
