using TMPro;
using UnityEngine;

public class resultadosUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntaje;
    [SerializeField] private TextMeshProUGUI tiempo;
    [SerializeField] private TextMeshProUGUI contestadas;
    [SerializeField] private TextMeshProUGUI correctas;

    public void MostrarResultados(int puntajee, float tiempoo, int contestadass, int correctass)
    {
        Debug.Log("Resultados actualizados");
        
        puntaje.text = "" + puntajee;
        tiempo.text = "" + FormatearTiempo(tiempoo);
        contestadas.text = "" + contestadass;
        correctas.text = "" + correctass;
    }
    
    private string FormatearTiempo(float tiempooo)
    {
        int minutos = Mathf.FloorToInt(tiempooo / 60f);
        int segundos = Mathf.FloorToInt(tiempooo % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}


