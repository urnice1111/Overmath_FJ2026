using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpressionEvaluator : MonoBehaviour
{
    public static ExpressionEvaluator Instance { get; private set; }

    [SerializeField] private string escenaResultado = "SampleScene";
    [SerializeField] private RectTransform slotParent;

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

        if (slots.Length == 0)
        {
            Debug.LogWarning("ExpressionEvaluator: No hay slots.");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            string val = slots[i].ObtenerValor();
            Debug.Log("  Slot[" + i + "] tipo=" + slots[i].tipoSlot + " vacio=" + slots[i].EstaVacio + " valor=" + (val ?? "null"));
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

        int resultado = EvaluarSuma(ordenados);
        if (resultado == int.MinValue)
        {
            Debug.LogError("ExpressionEvaluator: expresion invalida -> " + expresion);
            return;
        }

        Debug.Log("Expresion: " + expresion + " = " + resultado);


        bool correcto = PreguntaManager.Instance != null
            && PreguntaManager.Instance.VerificarRespuesta(resultado);
        if (correcto)
            Debug.Log("¡Respuesta correcta!");
        else
            Debug.Log("Respuesta incorrecta.");

        DragSelectionManager.Resultado = resultado;
        DragSelectionManager.ExpresionTexto = expresion + " = " + resultado;

        DragSelectionManager.Instance.LimpiarTodo();

        SceneManager.LoadScene(escenaResultado);

        PreguntaManager.Instance.CargarPreguntaAleatoria();
    }

    private int EvaluarSuma(DropSlot[] slotsOrdenados)
    {
        int acumulado = 0;
        bool esperaNumero = true;

        foreach (var slot in slotsOrdenados)
        {
            DraggableNumber item = slot.itemActual;
            if (item == null) return int.MinValue;

            if (esperaNumero)
            {
                if (item.esOperador) return int.MinValue;
                acumulado += item.numero;
            }
            else
            {
                if (!item.esOperador) return int.MinValue;
                // Solo suma por ahora; el operador ya esta validado como "+"
            }

            esperaNumero = !esperaNumero;
        }

        if (!esperaNumero) return acumulado;

        return int.MinValue;
    }
}
