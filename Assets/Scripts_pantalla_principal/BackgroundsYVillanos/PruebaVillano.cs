using UnityEngine;

public class PruebaVillano : MonoBehaviour
{
    public ControlVillano controlVillano; //Referencia al script ControlVillano para poder llamar a sus funciones

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            controlVillano.RespuestaCorrecta();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            controlVillano.RespuestaIncorrecta();
        }
    }
}
