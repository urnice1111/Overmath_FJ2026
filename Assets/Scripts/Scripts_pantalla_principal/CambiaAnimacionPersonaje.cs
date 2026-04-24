using UnityEngine;

// Animación + pasos (versión simple y funcional)

public class CambiaAnimacionPersonaje : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    public AudioClip sonidoPasos;

    public static bool isSwimming = false;

    private float tiempoEntrePasos = 0.4f;
    private float temporizadorPasos = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector2 velocidad = rb.linearVelocity;

        if (isSwimming)
        {
            animator.SetBool("isSwimming", true);
            sr.flipX = velocidad.x < -0.1f;
            temporizadorPasos = 0f;
            return;
        }
        else
        {
            animator.SetBool("isSwimming", false);
        }

        bool estaQuieto = velocidad.sqrMagnitude < 0.01f;
        bool mueveHorizontal = Mathf.Abs(velocidad.x) > 0.1f;
        bool mueveArriba = velocidad.y > 0.1f;
        bool mueveAbajo = velocidad.y < -0.1f;

        animator.SetBool("idle", estaQuieto);
        animator.SetBool("move_horizontal", mueveHorizontal);
        animator.SetBool("move_up", mueveArriba);
        animator.SetBool("move_down", mueveAbajo);

        sr.flipX = velocidad.x < -0.1f;

        
        bool estaCaminando = Mathf.Abs(velocidad.x) > 0.1f || Mathf.Abs(velocidad.y) > 0.1f;

        if (estaCaminando)
        {
            temporizadorPasos -= Time.deltaTime;

            if (temporizadorPasos <= 0f && !audioSource.isPlaying)
            {
                Debug.Log("Paso"); 
                audioSource.PlayOneShot(sonidoPasos);
                temporizadorPasos = tiempoEntrePasos;
            }
        }
        else
        {
            temporizadorPasos = 0f;
            audioSource.Stop();
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
        }
    }
}
