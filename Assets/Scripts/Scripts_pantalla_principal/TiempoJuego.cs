using UnityEngine;

public class TiempoJuego : MonoBehaviour
{
    public Transform barTransform;
    public float maxTime = 100f;
    public float decreaseRate = 0.5f;
    private float currentTime;
    private Vector3 initialScale;

    void Start()
    {
        currentTime = maxTime;
        initialScale = barTransform.localScale;
    }

    void Update()
    {
        currentTime -= decreaseRate * Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);

        float percent = currentTime / maxTime;

        // Escala solo hacia la derecha (si el pivote está a la izquierda)
        barTransform.localScale = new Vector3(initialScale.x * percent, initialScale.y, initialScale.z);
    }
}
