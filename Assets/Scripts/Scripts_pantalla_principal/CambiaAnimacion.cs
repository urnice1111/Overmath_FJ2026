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
        }

        animator.SetBool("move_up", ultimaDireccion == "up");
        animator.SetBool("move_down", ultimaDireccion == "down");
        animator.SetBool("move_horizontal", ultimaDireccion == "horizontal");

        animator.SetBool("idle", false);
        if (Mathf.Abs(velocidad.x) > 0.1f)
        {
            mirandoIzquierda = velocidad.x < 0;
        }
        sr.flipX = mirandoIzquierda;
    }
}

