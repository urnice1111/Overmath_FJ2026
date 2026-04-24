using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiaEscena : MonoBehaviour
{
    public string sceneToLoad;

    // 👉 Referencia opcional al barco
    public Transform barco;

    // 👉 Activa esto SOLO en el botón de entrar a tienda
    public bool guardarPosicionAntesDeCambiar = false;

    public void LoadNuevaEscena()
    {
        // 🔥 Guardar posición SOLO si está activado
        if (guardarPosicionAntesDeCambiar && barco != null)
        {
            GameData.posicionBarco = barco.position;
            GameData.hayPosicionGuardada = true;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}