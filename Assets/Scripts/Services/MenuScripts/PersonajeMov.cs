using UnityEngine;

public class PersonajeMov : MonoBehaviour
{
    public float velocidad = 1.5f;

    [Header("Limites de movimiento")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Orden visual")]
    public int prioridadVisual = 0;

    private Vector2 objetivo;
    private bool pausado = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ElegirNuevoObjetivo();
    }

    void Update()
    {
        if (pausado)
        {
            animator.SetBool("Caminando", false);
            return;
        }

        Vector2 nuevaPosicion = Vector2.MoveTowards(
            transform.position,
            objetivo,
            velocidad * Time.deltaTime
        );

        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, minX, maxX);
        nuevaPosicion.y = Mathf.Clamp(nuevaPosicion.y, minY, maxY);

        transform.position = nuevaPosicion;

        animator.SetBool("Caminando", true);

        if (objetivo.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        bool llegueAlObjetivo =
            Vector2.Distance(transform.position, objetivo) < 0.05f;

        bool enLimite =
            transform.position.x <= minX + 0.01f ||
            transform.position.x >= maxX - 0.01f ||
            transform.position.y <= minY + 0.01f ||
            transform.position.y >= maxY - 0.01f;

        if (llegueAlObjetivo || enLimite)
        {
            ElegirNuevoObjetivo();
        }

        // Orden visual: personajes siempre arriba de la isla/fondo
        spriteRenderer.sortingOrder =
            1000 + Mathf.RoundToInt(-transform.position.y * 100) + prioridadVisual;
    }

    void ElegirNuevoObjetivo()
    {
        float nuevoX = Random.Range(minX, maxX);
        float nuevoY = Random.Range(minY, maxY);

        objetivo = new Vector2(nuevoX, nuevoY);
    }

    void ElegirObjetivoLejosDe(Vector2 posicionOtro)
    {
        Vector2 direccion = ((Vector2)transform.position - posicionOtro).normalized;

        if (direccion == Vector2.zero)
            direccion = Random.insideUnitCircle.normalized;

        Vector2 nuevoObjetivo = (Vector2)transform.position + direccion * 2f;

        nuevoObjetivo.x = Mathf.Clamp(nuevoObjetivo.x, minX, maxX);
        nuevoObjetivo.y = Mathf.Clamp(nuevoObjetivo.y, minY, maxY);

        objetivo = nuevoObjetivo;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Personaje"))
        {
            ElegirObjetivoLejosDe(other.transform.position);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Personaje"))
        {
            ElegirObjetivoLejosDe(other.transform.position);
        }
    }

    public void PausarMovimiento(bool estado)
    {
        pausado = estado;
    }
}