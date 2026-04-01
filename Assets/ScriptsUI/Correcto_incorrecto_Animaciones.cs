using UnityEngine;

public class Correcto_incorrecto_Animaciones : MonoBehaviour
{
    [SerializeField] private CorrectoIncorrectoAnimaciones animacionesEnemigo;
    
    public void ReproducirResultado(bool correcto)
    {
        if (animacionesEnemigo != null)
        {
            animacionesEnemigo.ReproducirResultado(correcto);
        }
    }
}
