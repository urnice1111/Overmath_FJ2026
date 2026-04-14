using System.Collections.Generic;
using UnityEngine;

public class DragSelectionManager : MonoBehaviour
{
    public static DragSelectionManager Instance { get; private set; }

    public const int MaxItems = 5;

    public List<int> numerosSeleccionados = new List<int>();
    public List<string> operadoresSeleccionados = new List<string>();

    // Mapea índice de slot → info del item que contiene
    public static Dictionary<int, SlotAssignment> asignacionesSlots = new Dictionary<int, SlotAssignment>();

    [System.Serializable]
    public struct SlotAssignment
    {
        public bool esOperador;
        public int numero;
        public string simbolo;
        public int cardId;
    }
    public int TotalSeleccionados => numerosSeleccionados.Count + operadoresSeleccionados.Count;

    public static int Resultado { get; set; }
    public static string ExpresionTexto { get; set; }

    public static bool FueCorrecta {get; set;}

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

    public bool PuedeAgregar()
    {
        return TotalSeleccionados < MaxItems;
    }

    public void AgregarNumero(int numero)
    {
        if (!PuedeAgregar())
        {
            Debug.Log("No se pueden agregar mas items. Total: " + TotalSeleccionados);
            return;
        }
        numerosSeleccionados.Add(numero);
        Debug.Log("Numero agregado: " + numero + " | Total: " + TotalSeleccionados);
    }

    public void QuitarNumero(int numero)
    {
        if (numerosSeleccionados.Remove(numero))
        {
            Debug.Log("Numero quitado: " + numero + " | Total: " + TotalSeleccionados);
        }
    }

    public void AgregarOperador(string op)
    {
        if (!PuedeAgregar())
        {
            Debug.Log("No se pueden agregar mas items. Total: " + TotalSeleccionados);
            return;
        }
        operadoresSeleccionados.Add(op);
        Debug.Log("Operador agregado: " + op + " | Total: " + TotalSeleccionados);
    }

    public void QuitarOperador(string op)
    {
        if (operadoresSeleccionados.Remove(op))
        {
            Debug.Log("Operador quitado: " + op + " | Total: " + TotalSeleccionados);
        }
    }

    public void LimpiarTodo()
    {
        numerosSeleccionados.Clear();
        operadoresSeleccionados.Clear();
        asignacionesSlots.Clear();
    }
}