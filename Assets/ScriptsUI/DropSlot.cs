using UnityEngine;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour
{
    public DraggableNumber itemActual;

    private Image slotImage;
    private Color colorVacio;
    private Color colorOcupado;

    public void Inicializar(Color vacio, Color ocupado)
    {
        colorVacio = vacio;
        colorOcupado = ocupado;
        slotImage = GetComponent<Image>();
        ActualizarVisual();
    }

    public bool EstaVacio => itemActual == null;

    public bool PuedeAceptar(DraggableNumber item)
    {
        return EstaVacio;
    }

    public void Colocar(DraggableNumber item)
    {
        itemActual = item;
        ActualizarVisual();

        int slotIndex = transform.GetSiblingIndex();
        DragSelectionManager.asignacionesSlots[slotIndex] = new DragSelectionManager.SlotAssignment
        {
            esOperador = item.esOperador,
            numero = item.numero,
            simbolo = item.simboloOperador,
            cardId = item.uniqueId
        };
    }

    public void Liberar()
    {
        itemActual = null;
        ActualizarVisual();

        int slotIndex = transform.GetSiblingIndex();
        DragSelectionManager.asignacionesSlots.Remove(slotIndex);
    }

    public string ObtenerValor()
    {
        if (itemActual == null) return null;
        return itemActual.esOperador ? itemActual.simboloOperador : itemActual.numero.ToString();
    }

    private void ActualizarVisual()
    {
        if (slotImage == null) return;
        slotImage.color = EstaVacio ? colorVacio : colorOcupado;
    }
}
