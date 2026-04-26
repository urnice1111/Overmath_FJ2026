using UnityEngine;

[System.Serializable]
public class VisualData
{
    public string islandName;
    public GameObject backgroundObject;
    public GameObject villianObject;
}

public class ActivDesacBackgrounds : MonoBehaviour
{
    [SerializeField] private VisualData[] visualD;

    private void Start()
    {
        CargarDatosVisuales();
    }

    private void CargarDatosVisuales()
    {
        if (GameSession.Instance == null)
        {
            Debug.LogWarning("GameSession es null");
            return;
        }

        if (visualD == null || visualD.Length == 0)
        {
            Debug.LogWarning("ActivDesacBackgrounds: visualD está vacío");
            return;
        }

        string islaActual = NormalizeIslandName(GameSession.Instance.IslaActual);
        Debug.Log("Isla actual: " + islaActual);

        bool encontro = false;

        for (int i = 0; i < visualD.Length; i++)
        {
            string islandName = NormalizeIslandName(visualD[i].islandName);
            Debug.Log("Comparando con: " + islandName);

            bool activar = islandName == islaActual;

            if (visualD[i].backgroundObject != null)
                visualD[i].backgroundObject.SetActive(activar);
            else
                Debug.LogWarning("ActivDesacBackgrounds: Falta backgroundObject en índice " + i);

            if (visualD[i].villianObject != null)
                visualD[i].villianObject.SetActive(activar);
            else
                Debug.LogWarning("ActivDesacBackgrounds: Falta villianObject en índice " + i);

            if (activar)
                encontro = true;
        }

        if (!encontro)
        {
            Debug.LogWarning("No se encontró visual para la isla: " + islaActual);
        }
    }

    private static string NormalizeIslandName(string island)
    {
        return string.IsNullOrWhiteSpace(island) ? string.Empty : island.Trim().ToLowerInvariant();
    }
}