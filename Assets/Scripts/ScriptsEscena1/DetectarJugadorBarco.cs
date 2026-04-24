using UnityEngine;

public class DetectarJugadorBarco : MonoBehaviour
{
   public GameObject botonSubir;

   void Start()
    {
        botonSubir.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            botonSubir.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            botonSubir.SetActive(false);
        }
    }
}
