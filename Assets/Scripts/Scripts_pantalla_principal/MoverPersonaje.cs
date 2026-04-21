using UnityEngine;
using UnityEngine.InputSystem;
using DialogueSystem;

// Esta clase maneja el movimiento del personaje en 2D con teclado y joystick táctil

public class MoverPersonaje : MonoBehaviour
{
    [SerializeField]
    private InputAction accionMover;

    private Rigidbody2D rb;

    [SerializeField]    
    private float velocidadTierra = 5f;
    [SerializeField]
    private float velocidadAgua = 3f;
    private float velocidadMovimiento;

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
        if (DialogueHolder.IsDialogueActive)
        {
            input = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            return;
        }

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
        
        velocidadMovimiento = CambiaAnimacionPersonaje.isSwimming ? velocidadAgua : velocidadTierra;
    }
    void FixedUpdate()
    {
        if (DialogueHolder.IsDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = input * velocidadMovimiento;
    }
}
