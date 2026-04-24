using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (GameSession.Instance.IslaActual)
        {
            case "isla_suma":
                GameObject VillanoTae = GameObject.Find("VillanoTae");
                VillanoTae.SetActive(true);
                GameObject FondoTae = GameObject.Find("FondoTae");
                FondoTae.SetActive(true);
                break;
            case "isla_resta":
                GameObject VillanoHuerto = GameObject.Find("VillanoHuerto");
                VillanoHuerto.SetActive(true);
                GameObject FondoHuerto = GameObject.Find("FondoHuerto");
                FondoHuerto.SetActive(true);
                break;
            case "isla_multi":
                GameObject VillanoArte = GameObject.Find("VillanoArte");
                VillanoArte.SetActive(true);
                GameObject FondoArte = GameObject.Find("FondoArte");
                FondoArte.SetActive(true);
                break;

            case "isla_div":
                GameObject VillanoDeporte = GameObject.Find("VillanoDeporte");
                VillanoDeporte.SetActive(true);
                GameObject FondoDeporte = GameObject.Find("FondoDeporte");
                FondoDeporte.SetActive(true);
                break;

            case "isla_comb":
                GameObject VillanoComb = GameObject.Find("VillanoComb");
                VillanoComb.SetActive(true);
                GameObject FondoCine = GameObject.Find("FondoCine");
                FondoCine.SetActive(true);
                break;
            
            default:
                break;
        }
    }
}
