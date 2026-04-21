using UnityEngine;

public class CambiaAnimacion : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private string ultimaDireccion = "down";

    private bool mirandoIzquierda = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 velocidad = rb.linearVelocity;

        bool mueveHorizontal = Mathf.Abs(velocidad.x) > 0.1f;
        bool mueveArriba = velocidad.y > 0.1f;
        bool mueveAbajo = velocidad.y < -0.1f;
        bool estaQuieto = velocidad.sqrMagnitude < 0.01f;

        if (mueveArriba)
        {
            ultimaDireccion = "up";
        }
        else if (mueveAbajo)
        {
            ultimaDireccion = "down";
        }
        else if (mueveHorizontal)
        {
            ultimaDireccion = "horizontal";
            mirandoIzquierda = velocidad.x < 0;
        }

        animator.SetBool("move_up", false);
        animator.SetBool("move_down", false);
        animator.SetBool("move_horizontal", false);
        animator.SetBool("idle", false);

        if (estaQuieto)
        {
            if (ultimaDireccion == "horizontal")
            {
                animator.SetBool("idle", true);
            }
            else if (ultimaDireccion == "up")
            {
                animator.SetBool("move_up", true);
            }
            else if (ultimaDireccion == "down")
            {
                animator.SetBool("move_down", true);
            }
        }
        
        else
        {
            if (ultimaDireccion == "up")
            {
                animator.SetBool("move_up", true);
            }
            else if (ultimaDireccion == "down")
            {
                animator.SetBool("move_down", true);
            }
            else if (ultimaDireccion == "horizontal")
            {
                animator.SetBool("move_horizontal", true);
            }
        }

        sr.flipX = mirandoIzquierda;
    }
}

