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
        imagenSkin.sprite = imagen;
        nombreText.text = nombre;
        descripcionText.text = descripcion;
        precioText.text = "Precio: " + precio.ToString();
    }

    public void Cerrar()
    {
        gameObject.SetActive(false);
    }
}
