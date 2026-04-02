using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ExpressionEvaluator : MonoBehaviour
{
    public static ExpressionEvaluator Instance { get; private set; }

    [SerializeField] private string escenaResultado = "SampleScene";
    [SerializeField] private RectTransform slotParent;

    [SerializeField] private VillianState villianState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void IntentarEvaluar()
    {
        if (slotParent == null)
        {
            Debug.LogError("ExpressionEvaluator: slotParent es null. Asignalo en el Inspector.");
            return;
        }

        DropSlot[] slots = slotParent.GetComponentsInChildren<DropSlot>();
        Debug.Log("ExpressionEvaluator: slots encontrados = " + slots.Length);


        // If the user has no selected items and try to evaluate. Maybe change that later to only appear the button when 1 or more slots appeared
        if (slots.Length == 0)
        {
            Debug.LogWarning("ExpressionEvaluator: No hay slots.");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            string val = slots[i].ObtenerValor();
            Debug.Log("  Slot[" + i + "] vacio=" + slots[i].EstaVacio + " valor=" + (val ?? "null"));
        }

        foreach (var slot in slots)
        {
            if (slot.EstaVacio)
            {
                Debug.LogWarning("ExpressionEvaluator: Hay slots vacios, no se puede evaluar.");
                return;
            }
        }

        var ordenados = slots.OrderBy(s => s.GetComponent<RectTransform>().anchoredPosition.x).ToArray();

        string expresion = "";
        foreach (var slot in ordenados)
        {
            string val = slot.ObtenerValor();
            if (val == null) return;
            expresion += val;
        }

        int resultado = EvaluarExpresion(ordenados);
        if (resultado == int.MinValue)
        {
            Debug.LogError("ExpressionEvaluator: expresion invalida -> " + expresion);
            return;
        }

        Debug.Log("Expresion: " + expresion + " = " + resultado);


        bool correcto = PreguntaManager.Instance != null
            && PreguntaManager.Instance.VerificarRespuesta(resultado);
        if (correcto)
        {
            Debug.Log("¡Respuesta correcta!");
            
        }
            
        else
            Debug.Log("Respuesta incorrecta.");
            

        if(PuntajedePregunta.Instance != null)
        {
            PuntajedePregunta.Instance.RegistrarResultado(correcto);
        }
        else
        {
            Debug.LogWarning("ExpressionEvaluator: No se encontro PuntajedePregunta en la escena.");
        }

        DragSelectionManager.Resultado = resultado;
        DragSelectionManager.ExpresionTexto = expresion + " = " + resultado;
        DragSelectionManager.FueCorrecta = correcto;

        DragSelectionManager.Instance.LimpiarTodo();

        SceneManager.LoadScene(escenaResultado);

        PreguntaManager.Instance.CargarPreguntaAleatoria();
    }

    private int EvaluarExpresion(DropSlot[] slotsOrdenados)
    {
        List<int> numeros = new List<int>();
        List<string> operadores = new List<string>();

        // Tokenize: consecutive numbers are concatenated into multi-digit numbers.
        // e.g. [5][5][+][3] -> numeros=[55,3], operadores=[+]
        string numAcumulado = "";

        foreach (var slot in slotsOrdenados)
        {
            DraggableNumber item = slot.itemActual;
            if (item == null) return int.MinValue;

            if (!item.esOperador)
            {
                numAcumulado += item.numero.ToString();
            }
            else
            {
                if (numAcumulado.Length == 0) return int.MinValue;

                if (!int.TryParse(numAcumulado, out int parsed)) return int.MinValue;
                numeros.Add(parsed);
                numAcumulado = "";

                operadores.Add(item.simboloOperador);
            }
        }

        if (numAcumulado.Length == 0) return int.MinValue;
        if (!int.TryParse(numAcumulado, out int ultimo)) return int.MinValue;
        numeros.Add(ultimo);

        if (numeros.Count != operadores.Count + 1) return int.MinValue;

        // Pasada 1: resolver * y /
        for (int i = 0; i < operadores.Count; i++)
        {
            if (operadores[i] == "*" || operadores[i] == "/")
            {
                int res;
                if (operadores[i] == "*")
                    res = numeros[i] * numeros[i + 1];
                else
                {
                    if (numeros[i + 1] == 0) return int.MinValue;
                    res = numeros[i] / numeros[i + 1];
                }

                numeros[i] = res;
                numeros.RemoveAt(i + 1);
                operadores.RemoveAt(i);
                i--;
            }
        }

        // Pasada 2: resolver + y -
        int acumulado = numeros[0];
        for (int i = 0; i < operadores.Count; i++)
        {
            if (operadores[i] == "+")
                acumulado += numeros[i + 1];
            else if (operadores[i] == "-")
                acumulado -= numeros[i + 1];
        }

        return acumulado;
    }
}
