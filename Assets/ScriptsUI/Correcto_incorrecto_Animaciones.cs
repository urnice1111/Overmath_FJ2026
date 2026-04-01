using UnityEngine;

public class Correcto_incorrecto_Animaciones : MonoBehaviour
{
    //Referencia a las animaciones de correcto e incorrecto para el enemigo
    [SerializeField] private CorrectoIncorrectoAnimaciones animacionesEnemigo;
    

    //Reproduce la animación correspondiente según si la respuesta es correcta o incorrecta
    public void ReproducirResultado(bool correcto)
    {
        if (animacionesEnemigo != null)
        {
            animacionesEnemigo.ReproducirResultado(correcto);
        }
    }
}
