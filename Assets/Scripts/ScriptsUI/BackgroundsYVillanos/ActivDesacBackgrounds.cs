using System;
using UnityEngine;
[System.Serializable]
public class VisualData
{
    public string islandName; // Name of the island to activate the background
    public GameObject backgroundObjects; // Reference to the background GameObject
    public GameObject villianObject; // Reference to the Villian GameObject

}
public class ActivDesacBackgrounds : MonoBehaviour
{
    [SerializeField] private VisualData[] visualD; // Array to hold data for each island

    private void Start()
    {
        CargarDatosVisuales();
    }

    private void CargarDatosVisuales()
    {
        if (GameSession.Instance == null)
        {
            Debug.LogWarning("ActivDesacBackgrounds: GameSession.Instance es null");
            return;
        }

        string islaActual = GameSession.Instance.IslaActual;
        for (int i = 0; i < visualD.Length; i++)
        {
            if (visualD[i].islandName == islaActual)
            {
                visualD[i].backgroundObjects.SetActive(true);
                visualD[i].villianObject.SetActive(true);
                //Debug.Log("Se activaron los objetos para la isla: " + islaActual);
            }
            else
            {
                visualD[i].backgroundObjects.SetActive(false);
                visualD[i].villianObject.SetActive(false);
            }
        }

        Debug.Log("No se encontró la isla actual = " + islaActual);
    }
}
    
