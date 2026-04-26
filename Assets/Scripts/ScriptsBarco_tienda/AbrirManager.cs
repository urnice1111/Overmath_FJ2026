using UnityEngine;

public class BotonAbrirManager : MonoBehaviour
{
    public void AbrirCofreActual()
    {
        if (CofreInteractuable.cofreActual != null)
        {
            CofreInteractuable.cofreActual.AbrirCofre();
        }
    }
}