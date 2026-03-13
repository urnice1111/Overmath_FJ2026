using UnityEngine;

public class MesaCreacionScript : MonoBehaviour
{
    [SerializeField] private GameObject mesaCreacion;
    [SerializeField] private KeyCode teclaToggle = KeyCode.Escape;

    void Start()
    {
        if (mesaCreacion != null)
        {
            mesaCreacion.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaToggle))
        {
            ToggleMesaCreacion();
        }
    }

    // Conecta este metodo al OnClick() de un boton para abrir el Canvas.
    public void AbrirMesaCreacion()
    {
        if (mesaCreacion != null)
        {
            mesaCreacion.SetActive(true);
        }
    }

    // Conecta este metodo al OnClick() de un boton para cerrar el Canvas.
    public void CerrarMesaCreacion()
    {
        if (mesaCreacion != null)
        {
            mesaCreacion.SetActive(false);
        }
    }

    // Conecta este metodo al OnClick() para alternar entre abierto/cerrado.
    public void ToggleMesaCreacion()
    {
        if (mesaCreacion != null)
        {
            mesaCreacion.SetActive(!mesaCreacion.activeSelf);
        }
    }
}
