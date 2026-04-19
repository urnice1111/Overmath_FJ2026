using Unity.VisualScripting;
using UnityEngine;

// Esta clase cambia las animaciones del barco según su movimiento.

public class CambiaAnimacion : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 velocidad = rb.linearVelocity;
        bool estaQuieto = velocidad.sqrMagnitude < 0.01f;
        bool mueveHorizontal = Mathf.Abs(velocidad.x) > 0.1f;
        bool mueveArriba = velocidad.y > 0.1f;
        bool mueveAbajo = velocidad.y < -0.1f;
        
        
        animator.SetBool("idle", estaQuieto);
        animator.SetBool("move_horizontal", mueveHorizontal);
        animator.SetBool("move_up", mueveArriba);
        animator.SetBool("move_down", mueveAbajo);

        //animator.SetTrigger("")

        sr.flipX = velocidad.x < -0.1f;
    }
}

