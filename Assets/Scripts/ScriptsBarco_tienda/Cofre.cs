using System.Collections;
using UnityEngine;
using TMPro;

public class CofreInteractuable : MonoBehaviour
{
    [SerializeField] GameObject botonComprar;
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

    public class skinPurchaseInof
    {
        public string assetName;
    }

    [SerializeField] TMP_Text textoPrecio;

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

        StoreManager.Instance.currentChestIndex = indexOfResponse;

        Debug.Log("Current chest: " + StoreManager.Instance.currentChestIndex);
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

    // Leer siempre los datos frescos del StoreManager
    var skin = StoreManager.Instance.skins[indexOfResponse];
    descripcion = skin.descripcion;
    precio = skin.precio;

    if (skin.desbloqueado == 1)
    {
        botonComprar.SetActive(false);
        textoPrecio.color = Color.gray;
    }
    else if (skin.precio > GameSession.Instance.monedas)
    {
        botonComprar.SetActive(false);
        textoPrecio.color = Color.red;
    }
    else
    {
        botonComprar.SetActive(true);
        textoPrecio.color = Color.white;
    }

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
        precio = StoreManager.Instance.skins[indexOfResponse].precio;

        switch (StoreManager.Instance.skins[indexOfResponse].nombre_asset)
        {
            case "azul_skin":
                nombreSkin = "Congelar el tiempo";
                break;

            default: 
                nombreSkin = "d";
                break;
        }

        string spriteName = StoreManager.Instance.skins[indexOfResponse].nombre_asset + "_0";
        Sprite[] all = Resources.LoadAll<Sprite>("Skins/" + StoreManager.Instance.skins[indexOfResponse].nombre_asset);
        imagenSkin = null;
        foreach (var s in all)
        {
            if (s.name == spriteName)
            {
                imagenSkin = s;
                break;
            }
        }
        if (imagenSkin == null && all.Length > 0)
            imagenSkin = all[0];

    }
}