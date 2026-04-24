using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaFrontera : MonoBehaviour
{
    public string nombreEscena;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBarco"))
        {
            SceneManager.LoadScene(nombreEscena);
        }
    }
}