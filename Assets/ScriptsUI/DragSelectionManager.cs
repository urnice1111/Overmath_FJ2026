using System.Collections.Generic;
using UnityEngine;

public class DragSelectionManager : MonoBehaviour
{
    public static DragSelectionManager Instance { get; private set; }

    public List<int> numerosSeleccionados = new List<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AgregarNumero(int numero)
    {
        if (numerosSeleccionados.Count < 5)
        {
           numerosSeleccionados.Add(numero);
           Debug.Log("Número agregado: " + numero);
        }
        else
        {
            Debug.Log("No se pueden agregar más números." + numerosSeleccionados.Count);
        }
    }
}