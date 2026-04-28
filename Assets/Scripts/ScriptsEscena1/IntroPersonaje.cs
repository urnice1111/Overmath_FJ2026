using UnityEngine;

public class IntroPersonaje : MonoBehaviour
{
    public MonoBehaviour controlMovimiento; 
    public CambiaAnimacionPersonaje animScript; 

    public float duracionAnimacion = 2.5f;

    void Start()
    {
        controlMovimiento.enabled = false;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        Invoke("ActivarControl", duracionAnimacion);
    }

    void ActivarControl()
    {
        controlMovimiento.enabled = true;
    }
}
