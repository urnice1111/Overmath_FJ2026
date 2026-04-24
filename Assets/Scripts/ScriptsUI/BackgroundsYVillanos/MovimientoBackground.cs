using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MovimientoBackground : MonoBehaviour
{
    [SerializeField] private float duracionRecorrido = 10f;

    private Vector3 posicionInicial;
    private float desplazamientoMaximo;
    private SpriteRenderer sr;
    private Camera cam;

    private void Start()
    {
        posicionInicial = transform.position;
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("MovimientoBackground: no se encontró la Main Camera.");
            enabled = false;
            return;
        }

        float anchoSprite = sr.bounds.size.x;
        float altoCamara = cam.orthographicSize * 2f;
        float anchoCamara = altoCamara * cam.aspect;

        desplazamientoMaximo = (anchoSprite - anchoCamara) / 2f;

        if (desplazamientoMaximo <= 0f)
        {
            Debug.LogWarning("MovimientoBackground: el fondo no es más ancho que la cámara.");
            desplazamientoMaximo = 0f;
        }
    }

    private void Update()
    {
        if (desplazamientoMaximo <= 0f)
            return;

        float t = Mathf.PingPong(Time.time / duracionRecorrido, 1f);
        t = Mathf.SmoothStep(0f, 1f, t);

        float nuevaX = Mathf.Lerp(
            posicionInicial.x + desplazamientoMaximo,
            posicionInicial.x - desplazamientoMaximo,
            t
        );

        transform.position = new Vector3(nuevaX, posicionInicial.y, posicionInicial.z);
    }
}