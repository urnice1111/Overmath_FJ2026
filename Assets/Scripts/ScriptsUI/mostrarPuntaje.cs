using UnityEngine;
using TMPro;

public class mostrarPuntaje : MonoBehaviour
{
        [SerializeField] private TextMeshProUGUI _puntaje;

        public void MostrarPuntajee(int puntaje)
        {
            _puntaje.text = "Puntaje: " + puntaje;        
        }
}
    