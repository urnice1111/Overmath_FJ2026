using UnityEngine;
using UnityEngine.InputSystem;

// Esta clase maneja el movimiento del barco en 2D 

public class MoverBarco : MonoBehaviour
{
    [SerializeField]
    private InputAction accionMover;

    private Rigidbody2D rb;
    [SerializeField]
    private float velocidadMovimiento = 7f;
        
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        accionMover.Enable();
    }

    void OnDisable()
    {
        accionMover.Disable();
    }

    void FixedUpdate()
    {
        Vector2 movimiento = accionMover.ReadValue<Vector2>();
        rb.linearVelocity = movimiento * velocidadMovimiento;
    }
}

//git checkout --theirs Assets/Scenes/VistaSeleccion.unity.meta
//git add Assets/Scenes/VistaSeleccion.unity.meta