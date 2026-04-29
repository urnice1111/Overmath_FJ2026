
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    private GameObject operadorActivado;

    [SerializeField] private GameObject operadorSuma;
    [SerializeField] private GameObject operadorResta;
    [SerializeField] private GameObject operadorMultiplicacion;
    [SerializeField] private GameObject operadorDivision;

    [SerializeField] private GameObject botonFreeze;
    [SerializeField] private GameObject botonResolver;

    private void Start()
    {


        switch (GameSession.Instance.IslaActual)
        {
            case "isla_suma":
                operadorSuma.SetActive(true);
                break;
            case "isla_resta":
                operadorResta.SetActive(true);
                break;
            case "isla_multi":
                operadorMultiplicacion.SetActive(true);
                break;
            case "isla_div":
                operadorDivision.SetActive(true);
                break;
            default:
                operadorDivision.SetActive(true);
                operadorMultiplicacion.SetActive(true);
                operadorResta.SetActive(true);
                operadorSuma.SetActive(true);
                break;
        }

        switch (GameSession.Instance.skinSelected)
        {
            case "azul_skin":
                botonFreeze.SetActive(true);
                break;
            case "amarillo_skin":
                botonResolver.SetActive(false);
                break;
            default:
                break;
        }
    }
}