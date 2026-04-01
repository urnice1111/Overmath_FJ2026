using UnityEngine;

public class CorrectoIncorrectoAnimaciones : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerCorrecto = "CorrectAnsw";
    [SerializeField] private string triggerIncorrecto = "WrongAnsw";

    private int _hashCorrecto;
    private int _hashIncorrecto;

    private void Awake()
    {
        _hashCorrecto = Animator.StringToHash(triggerCorrecto);
        _hashIncorrecto = Animator.StringToHash(triggerIncorrecto);
    }

    public void ReproducirResultado(bool esCorrecto)
    {
        if (animator == null)
        {
            Debug.LogWarning("CorrectoIncorrectoAnimaciones: Animator no asignado.");
            return;
        }

        if (esCorrecto)
        {
            animator.ResetTrigger(_hashIncorrecto);
            animator.SetTrigger(_hashCorrecto);
        }
        else
        {
            animator.ResetTrigger(_hashCorrecto);
            animator.SetTrigger(_hashIncorrecto);
        }
    }
}
