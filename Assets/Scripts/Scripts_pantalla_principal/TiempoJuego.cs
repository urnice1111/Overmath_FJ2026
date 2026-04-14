// The script controls the time bar. 
using UnityEngine;

public class TiempoJuego : MonoBehaviour
{
    [SerializeField]
    private Transform barTransform;
    public float maxTime = 100;
    public float decreaseRate = 5;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private Vector3 initialScale;

    void Start()
    {
        // Obtiene el Transform del mismo objeto
        barTransform = GetComponent<Transform>();

        currentTime = maxTime;
        initialScale = barTransform.localScale;
    }

    void Update()
    {
        currentTime -= decreaseRate * Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);

        float percent = currentTime / maxTime;
        barTransform.localScale = new Vector3(initialScale.x * percent, initialScale.y, initialScale.z);
    }
    
    public void AjustarTiempo(float cantidad)
    {
        currentTime -= cantidad; // restar o sumar tiempo
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);
    }
}