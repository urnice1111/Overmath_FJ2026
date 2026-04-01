using UnityEngine;

public class CorrectoIncorrectoAnimaciones : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerCorrecto = "CorrectAnsw";
    [SerializeField] private string triggerIncorrecto = "WrongAnsw";

    //Se hace hash de los nombres de los triggers para que Unity los procese más rápido
    private int _hashCorrecto; 
    private int _hashIncorrecto;

    private void Awake() //Inicializa los hashes de los triggers al iniciar el juego
    {
        _hashCorrecto = Animator.StringToHash(triggerCorrecto);
        _hashIncorrecto = Animator.StringToHash(triggerIncorrecto);
    }

    //Reproduce la animación correspondiente según si la respuesta es correcta o incorrecta
    public void ReproducirResultado(bool esCorrecto)
    {
        if (animator == null)
        {
            Debug.LogWarning("CorrectoIncorrectoAnimaciones: Animator no asignado.");
            return;
        }

        if (esCorrecto)
        {
            animator.ResetTrigger(_hashIncorrecto);//Resetea el trigger de incorrecto para evitar conflictos si se presionó antes
            animator.SetTrigger(_hashCorrecto);//Activa el trigger de correcto para reproducir la animación correspondiente
        }
        else
        {
            animator.ResetTrigger(_hashCorrecto);
            animator.SetTrigger(_hashIncorrecto);
        }
    }
}
