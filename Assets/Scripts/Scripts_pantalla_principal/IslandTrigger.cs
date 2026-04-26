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
            if (GameSession.Instance == null)
            {
                Debug.LogWarning("IslandTrigger: GameSession es null");
                return;
            }

            // Usa islandName configurado y, si falta, intenta con el nombre del objeto.
            string selectedIsland = ResolveIslandName();
            GameSession.Instance.SetIsla(selectedIsland);
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

    private string ResolveIslandName()
    {
        string configuredName = NormalizeIslandName(islandName);
        if (!string.IsNullOrEmpty(configuredName))
            return configuredName;

        string fallbackName = NormalizeIslandName(gameObject.name);
        if (!string.IsNullOrEmpty(fallbackName))
        {
            Debug.LogWarning("IslandTrigger: islandName está vacío, usando gameObject.name = " + fallbackName);
            return fallbackName;
        }

        Debug.LogWarning("IslandTrigger: No se pudo resolver el nombre de isla");
        return string.Empty;
    }

    private static string NormalizeIslandName(string island)
    {
        return string.IsNullOrWhiteSpace(island) ? string.Empty : island.Trim().ToLowerInvariant();
    }
}
