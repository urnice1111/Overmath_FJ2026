using System;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    public GameObject playButtonUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            // Get and set the island name from the gameobjet name when trigger
            GameSession.Instance.SetIsla(gameObject.name);
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
