using UnityEngine;
using TMPro;

public class FrasesCanvaPerdiste : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI campoFrase;
    [SerializeField] private TextMeshProUGUI _puntaje;
    
    public string[] listaDeFrases = {
        "Incluso los mejores héroes tropiezan a veces.",
        "La práctica hace al maestro. ¡Dale otra vez!",
        "¡Estuviste a nada! La próxima será la vencida.",
        "Un pequeño error no define tu gran partida.",
        "Aprender de una caída es empezar a levantarse.",
        "El éxito es la suma de muchos intentos fallidos.",
        "¡Tus reflejos están mejorando, sigue así!",
        "No es un final, es solo una pausa para mejorar.",
        "¡Esa estuvo cerca! Vuelve con más fuerza.",
        "Tómate un respiro y regresa a la carga."
    };

    // Este método se ejecuta cada vez que el LoseCanvas se activa (SetActivate(true))
    void OnEnable()
    {
        AsignarFraseNueva();
    }

    private void AsignarFraseNueva()
    {
        if (campoFrase != null && listaDeFrases.Length > 0)
        {
            // Elegimos un índice al azar
            int indiceAleatorio = Random.Range(0, listaDeFrases.Length);
            
            // Asignamos el texto
            campoFrase.text = listaDeFrases[indiceAleatorio];
        }
    }

    public void MostrarPuntaje(int puntaje)
    {
        _puntaje.text = "Puntaje: " + puntaje;        
    }
}
