using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroPersonaje : MonoBehaviour
{
    public MonoBehaviour controlMovimiento;
    public float duracionAnimacion = 2.5f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "Escena1")
        {
            controlMovimiento.enabled = false;
            animator.SetTrigger("despertar");
            Invoke("ActivarControl", duracionAnimacion);
        }
        else
        {
            controlMovimiento.enabled = true;
        }
    }
    void ActivarControl()
    {
        controlMovimiento.enabled = true;
    }
}