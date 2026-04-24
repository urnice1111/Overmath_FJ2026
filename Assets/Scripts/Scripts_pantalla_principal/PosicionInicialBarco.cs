using UnityEngine;

public class PosicionInicialBarco : MonoBehaviour
{
    void Start()
    {
        if (GameData.hayPosicionGuardada)
        {
            transform.position = GameData.posicionBarco;
        }
    }
}
