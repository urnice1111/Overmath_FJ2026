using System.Collections;
using UnityEngine;

public class CofreInteractuable : MonoBehaviour
{
    public static CofreInteractuable cofreActual;

    [Header("UI")]
    public GameObject botonAbrir;
    public GameObject panelSkin;
    public SkinUIManager uiManager;

    [Header("Animación")]
    public Animator animator;

    [Header("Datos de la Skin")]
    public Sprite imagenSkin;
    public string nombreSkin;
    [TextArea]
    public int precio;

    public int indexOfResponse;

    public string descripcion;


    void Start()
    {
        botonAbrir.SetActive(false);
        panelSkin.SetActive(false);
        StartCoroutine(WaitForSkins());
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            botonAbrir.SetActive(true);
            cofreActual = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            botonAbrir.SetActive(false);

            if (cofreActual == this)
                cofreActual = null;
        }
    }

    public void AbrirCofre()
    {
        panelSkin.SetActive(true);
        botonAbrir.SetActive(false);

        // 🎬 ANIMACIÓN ABRIR
        if (animator != null)
            animator.SetTrigger("abrir");

        uiManager.MostrarSkin(imagenSkin, nombreSkin, descripcion, precio);
    }

    public void CerrarCofre()
    {
        // 🎬 ANIMACIÓN CERRAR
        if (animator != null)
            animator.SetTrigger("cerrar");
    }

    private IEnumerator WaitForSkins()
    {
        yield return new WaitUntil(() => StoreManager.Instance != null && StoreManager.Instance.skinsLoaded);
        descripcion = StoreManager.Instance.skins[indexOfResponse].descripcion;
    }
}