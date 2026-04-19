using UnityEngine;
using UnityEngine.InputSystem;

// Esta clase maneja el movimiento del personaje en 2D con teclado y joystick táctil

public class MoverPersonaje : MonoBehaviour
{
    [SerializeField]
    private InputAction accionMover;

    private Rigidbody2D rb;

    [SerializeField]    
    private float velocidadMovimiento = CambiaAnimacionPersonaje.isSwimming ? 3f : 5f; 

    // Referencia al joystick 
    [SerializeField]
    private Joystick joystick;

    private Vector2 input;

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
    void Update()
    {
        // Input de teclado
        Vector2 inputTeclado = accionMover.ReadValue<Vector2>();
        // Input táctil (joystick)
        Vector2 inputTouch = Vector2.zero;

        if (joystick != null)
        {
            inputTouch = joystick.GetInput();
        }

        // Combinar ambos inputs
        input = inputTeclado + inputTouch;
        // Evitar que sea mayor a 1 (normalizar)
        input = Vector2.ClampMagnitude(input, 1f);
    }
    void FixedUpdate()
    {
        rb.linearVelocity = input * velocidadMovimiento;
    }
}
