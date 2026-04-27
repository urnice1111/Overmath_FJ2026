using UnityEngine;

public class PersonajeMov : MonoBehaviour
{
    public float velocidad = 1.5f;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector2 objetivo;
    private bool pausado = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sprite;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (pausado)
        {
            animator.SetBool("Caminando", false);
            return;
        }

        Vector2 nuevaPosicion = Vector2.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
        nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, minX, maxX);
        nuevaPosicion.y = Mathf.Clamp(nuevaPosicion.y, minY, maxY);
        transform.position = nuevaPosicion;

        animator.SetBool("Caminando", true);

        if (objetivo.x < transform.position.x)
            spriteRenderer.flipX = true;
        else 
            spriteRenderer.flipX = false;

        bool llegueAlObjetivo = Vector2.Distance(transform.position, objetivo) < 0.05f;

        bool enLimite = transform.position.x <= minX - 0.01f || transform.position.x >= maxX - 0.01f || transform.position.y <= minY + 0.01f || transform.position.y >= maxY - 0.01f;

        if (llegueAlObjetivo || enLimite)
            ElegirNuevoObjetivo();

        sprite.sortingOrder = Mathf.RoundToInt(-transform.position.y * 200);
    }

    void ElegirNuevoObjetivo()
    {
        float nuevoX = Random.Range(minX, maxX);
        float nuevoY = Random.Range(minY, maxY);
        objetivo = new Vector2(nuevoX, nuevoY);
    }

    public void PausarMovimiento(bool estado)
    {
        pausado = estado;
    }
}