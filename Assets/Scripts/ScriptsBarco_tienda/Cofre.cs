using UnityEngine;

public class CofreInteractuable : MonoBehaviour
{
    [Header("UI")]
    public GameObject botonAbrir;
    public GameObject panelSkin;
    public SkinUIManager uiManager;

    [Header("Datos de la Skin")]
    public Sprite imagenSkin;
    public string nombreSkin;
    [TextArea]
    public string descripcion;
    public int precio;

    void Start()
    {
        botonAbrir.SetActive(false);
        panelSkin.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            botonAbrir.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            botonAbrir.SetActive(false);
        }
    }

    public void AbrirCofre()
    {
        panelSkin.SetActive(true);
        botonAbrir.SetActive(false);

        uiManager.MostrarSkin(imagenSkin, nombreSkin, descripcion, precio);
    }
}
