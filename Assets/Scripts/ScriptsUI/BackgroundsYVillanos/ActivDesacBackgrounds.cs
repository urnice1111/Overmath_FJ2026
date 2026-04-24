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

        string islaActual = GameSession.Instance.IslaActual;
        Debug.Log("Isla actual: " + islaActual);

        bool encontro = false;

        for (int i = 0; i < visualD.Length; i++)
        {
            Debug.Log("Comparando con: " + visualD[i].islandName);

            bool activar = visualD[i].islandName == islaActual;

            if (visualD[i].backgroundObject != null)
                visualD[i].backgroundObject.SetActive(activar);

            if (visualD[i].villianObject != null)
                visualD[i].villianObject.SetActive(activar);

            if (activar)
                encontro = true;
        }

        if (!encontro)
        {
            Debug.LogWarning("No se encontró visual para la isla: " + islaActual);
        }
    }
}