using UnityEngine;

// Esta clase cambia las animaciones del personaje según su movimiento y estado (agua/tierra)

public class CambiaAnimacionPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    public static bool isSwimming = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 velocidad = rb.linearVelocity;

        
        if (isSwimming)
        {
            animator.SetBool("isSwimming", true);

            
            sr.flipX = velocidad.x < -0.1f;

            return; 
        }
        else
        {
            animator.SetBool("isSwimming", false);
        }

        // Animaciones normales (tierra)
        bool estaQuieto = velocidad.sqrMagnitude < 0.01f;
        bool mueveHorizontal = Mathf.Abs(velocidad.x) > 0.1f;
        bool mueveArriba = velocidad.y > 0.1f;
        bool mueveAbajo = velocidad.y < -0.1f;

        animator.SetBool("idle", estaQuieto);
        animator.SetBool("move_horizontal", mueveHorizontal);
        animator.SetBool("move_up", mueveArriba);
        animator.SetBool("move_down", mueveAbajo);

        sr.flipX = velocidad.x < -0.1f;
    }

    // Detectar entrada al agua
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = true;
        }
    }

    // Detectar salida del agua
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
        }
    }
}