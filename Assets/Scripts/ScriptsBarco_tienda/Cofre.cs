// using UnityEngine;

// public class CofreInteractuable : MonoBehaviour
// {
//     // 🔗 Referencia global al cofre actual
//     public static CofreInteractuable cofreActual;

//     [Header("UI")]
//     public GameObject botonAbrir;
//     public GameObject panelSkin;
//     public SkinUIManager uiManager;

//     [Header("Datos de la Skin")]
//     public Sprite imagenSkin;
//     public string nombreSkin;
//     [TextArea]
//     public string descripcion;
//     public int precio;

//     void Start()
//     {
//         botonAbrir.SetActive(false);
//         panelSkin.SetActive(false);
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             botonAbrir.SetActive(true);
//             cofreActual = this; // 👈 ESTE COFRE ES EL ACTIVO
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             botonAbrir.SetActive(false);

//             if (cofreActual == this)
//                 cofreActual = null;
//         }
//     }

//     public void AbrirCofre()
//     {
//         panelSkin.SetActive(true);
//         botonAbrir.SetActive(false);

//         uiManager.MostrarSkin(imagenSkin, nombreSkin, descripcion, precio);
//     }
// }

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
}