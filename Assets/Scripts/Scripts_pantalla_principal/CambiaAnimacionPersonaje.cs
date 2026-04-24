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
    public AudioClip sonidoNadar;

    private float tiempoEntreBrazadas = 0.5f;
    private float temporizadorNado = 0f;
    private bool estabaEnAgua = false;
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
    
    // Detener audio de pasos SOLO cuando entra al agua (transición)
    if (!estabaEnAgua)
    {
        audioSource.Stop();
        temporizadorPasos = 0f;
        estabaEnAgua = true;
    }

    bool estaNadando = Mathf.Abs(velocidad.x) > 0.1f || Mathf.Abs(velocidad.y) > 0.1f;

    if (estaNadando)
    {
        temporizadorNado -= Time.deltaTime;

        if (temporizadorNado <= 0f && !audioSource.isPlaying)
        {
            Debug.Log("Nadar");
            audioSource.PlayOneShot(sonidoNadar);
            temporizadorNado = tiempoEntreBrazadas;
        }
    }
    else
    {
        temporizadorNado = 0f;
        audioSource.Stop();
    }

    return;
}
        else
        {
            animator.SetBool("isSwimming", false);
            // Detener audio de nado cuando sale del agua
            if (estabaEnAgua)
            {
                audioSource.Stop();
                temporizadorNado = 0f;
                estabaEnAgua = false;
            }
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
