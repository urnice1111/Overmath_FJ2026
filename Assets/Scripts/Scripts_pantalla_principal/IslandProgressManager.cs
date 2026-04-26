using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class IslandProgressManager : MonoBehaviour
{
    [SerializeField] private TutorialIslandTrigger tutorialIsland;

    private const string BaseUrl =
        "http://localhost:8080";

    private void Start()
    {
        BloquearTodasAlInicio();
        StartCoroutine(CargarProgreso());
    }

    private void BloquearTodasAlInicio()
    {
        foreach (var island in FindObjectsByType<IslandTrigger>(FindObjectsSortMode.None))
            island.SetBloqueada(true);
    }

    private IEnumerator CargarProgreso()
    {
        int userId = GameSession.Instance.userId;
        string url = $"{BaseUrl}/get_player_progress/{userId}";

        using UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("IslandProgressManager: Error al obtener progreso: " + www.error);
            yield break;
        }

        ProgresoResponse response = JsonUtility.FromJson<ProgresoResponse>(www.downloadHandler.text);

        GameSession.Instance.tutorialCompletado = response.tutorial_completado;
        GameSession.Instance.islasProgreso.Clear();
        if (response.islas != null)
            GameSession.Instance.islasProgreso.AddRange(response.islas);

        AplicarProgreso(response);
    }

    private void AplicarProgreso(ProgresoResponse response)
    {
        if (!response.tutorial_completado)
        {
            foreach (var island in FindObjectsByType<IslandTrigger>(FindObjectsSortMode.None))
                island.SetBloqueada(true);

            if (tutorialIsland != null)
                tutorialIsland.gameObject.SetActive(true);

            return;
        }

        if (tutorialIsland != null && tutorialIsland.completedBadgePublic != null)
            tutorialIsland.completedBadgePublic.SetActive(true);

        var allIslands = FindObjectsByType<IslandTrigger>(FindObjectsSortMode.None);

        foreach (var island in allIslands)
        {
            string name = island.GetIslandName();
            if (string.IsNullOrEmpty(name)) continue;

            bool desbloqueada = GameSession.Instance.IsIslaDesbloqueada(name);
            island.SetBloqueada(!desbloqueada);
        }
    }
}
