using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinUIManager : MonoBehaviour
{
    public Image imagenSkin;
    public TMP_Text nombreText;
    public TMP_Text descripcionText;
    public TMP_Text precioText;

    public void MostrarSkin(Sprite imagen, string nombre, string descripcion, int precio)
    {
        // Limpiar UI primero
        imagenSkin.sprite = null;
        nombreText.text = "";
        descripcionText.text = "";
        precioText.text = "";

        // Asignar nuevos datos
        if (imagen != null)
            imagenSkin.sprite = imagen;

        if (!string.IsNullOrEmpty(nombre))
            nombreText.text = nombre;

        if (!string.IsNullOrEmpty(descripcion))
            descripcionText.text = descripcion;

        if (precio > 0)
            precioText.text = precio.ToString();
    }

    public void Cerrar()
    {
        gameObject.SetActive(false);
    }
}