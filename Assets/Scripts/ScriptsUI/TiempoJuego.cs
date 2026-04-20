// The script controls the time bar. 
using UnityEngine;

public class TiempoJuego : MonoBehaviour
{
    public static TiempoJuego Instance { get; private set; }
    

    [SerializeField]
    private Transform barTransform;
    [SerializeField]
    private float maxTime = 100;
    [SerializeField]
    private float decreaseRate = 5;
    private float currentTime;
    [SerializeField]
    private Vector3 initialScale;
    public float TiempoJugado { get; private set; }
    
    public float TiempoRestante => currentTime;

    void Start()
    {
        // Obtiene el Transform del mismo objeto
        barTransform = GetComponent<Transform>();

        currentTime = maxTime;
        initialScale = barTransform.localScale;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        currentTime -= decreaseRate * Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);

        float percent = currentTime / maxTime;
        barTransform.localScale = new Vector3(initialScale.x * percent, initialScale.y, initialScale.z);
        
        if (currentTime > 0)
        {
            TiempoJugado += Time.deltaTime;
        }
    }
    
    public void AjustarTiempo(float cantidad)
    {
        currentTime -= cantidad; // restar o sumar tiempo
        currentTime = Mathf.Clamp(currentTime, 0, maxTime);
    }
}