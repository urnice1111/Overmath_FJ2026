//<<<<<<< HEAD
using System;
using Unity.Profiling;
using UnityEngine;

public class ActiveManager : MonoBehaviour
{
    private GameObject operadorActivado;

    [SerializeField] private GameObject operadorSuma;
    [SerializeField] private GameObject operadorResta;
    [SerializeField] private GameObject operadorMultiplicacion;
    [SerializeField] private GameObject operadorDivision;

    private void Start()
    {
        // if (IsVisualDataManagerActive())
        // {
        //     Debug.Log("ActiveManager: ActivDesacBackgrounds está activo; se omite activación por nombres.");
        //     return;
        // }

        // if (GameSession.Instance == null)
        // {
        //     Debug.LogWarning("ActiveManager: GameSession es null");
        //     return;
        // }

        // string isla = NormalizeIslandName(GameSession.Instance.IslaActual);

        // if (string.IsNullOrEmpty(isla))
        // {
        //     Debug.LogWarning("ActiveManager: IslaActual está vacía o null");
        //     return;
        // }


        // Debug.Log("ActiveManager: isla normalizada = '" + isla + "'");


        // This switch is unnecesary lol
        // switch (isla)
        // {
        //     case "isla_suma":
        //         ActivatePair("VillanoTae", "FondoTae");
        //         Debug.Log("AAAAAAAAAAA");
        //         break;
        //     case "isla_resta":
        //         ActivatePair("VillanoHuerto", "FondoHuerto");
        //         break;
        //     case "isla_multi":
        //         ActivatePair("VillanoArte", "FondoArte");
        //         break;
        //     case "isla_div":
        //         ActivatePair("VillanoDeporte", "FondoDeportes");
        //         break;
        //     case "isla_comb":
        //         ActivatePair("VillanoComb", "FondoCine");
        //         break;
        //     default:
        //         Debug.LogWarning("ActiveManager: IslaActual no reconocida: " + isla);
        //         break;
        // }

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
    }
}
//=======
// using UnityEngine;

// public class ActiveManager : MonoBehaviour
// {
//     private void Start()
//     {
//         if (IsVisualDataManagerActive())
//         {
//             Debug.Log("ActiveManager: ActivDesacBackgrounds está activo; se omite activación por nombres.");
//             return;
//         }

//         if (GameSession.Instance == null)
//         {
//             Debug.LogWarning("ActiveManager: GameSession es null");
//             return;
//         }

//         string isla = NormalizeIslandName(GameSession.Instance.IslaActual);

//         if (string.IsNullOrEmpty(isla))
//         {
//             Debug.LogWarning("ActiveManager: IslaActual está vacía o null");
//             return;
//         }

//         switch (isla)
//         {
//             case "isla_suma":
//                 ActivatePair("VillanoTae", "FondoTae");
//                 break;
//             case "isla_resta":
//                 ActivatePair("VillanoHuerto", "FondoHuerto");
//                 break;
//             case "isla_multi":
//                 ActivatePair("VillanoArte", "FondoArte");
//                 break;
//             case "isla_div":
//                 ActivatePair("VillanoDeporte", "FondoDeportes");
//                 break;
//             case "isla_comb":
//                 ActivatePair("VillanoComb", "FondoCine");
//                 break;
//             default:
//                 Debug.LogWarning("ActiveManager: IslaActual no reconocida: " + isla);
//                 break;
//         }
//     }
//>>>>>>> 4b7ff7a (sprites de nosotros)

//     private static string NormalizeIslandName(string island)
//     {
//         return string.IsNullOrWhiteSpace(island) ? string.Empty : island.Trim().ToLowerInvariant();
//     }

//     private static bool IsVisualDataManagerActive()
//     {
//         ActivDesacBackgrounds[] managers = Resources.FindObjectsOfTypeAll<ActivDesacBackgrounds>();
//         for (int i = 0; i < managers.Length; i++)
//         {
//             ActivDesacBackgrounds manager = managers[i];
//             if (manager == null)
//                 continue;

//             if (!manager.gameObject.scene.IsValid())
//                 continue;

//             if (manager.isActiveAndEnabled)
//                 return true;
//         }

//         return false;
//     }

//     private static void ActivatePair(string villanoName, params string[] fondoNames)
//     {
//         GameObject villano = FindInSceneIncludingInactive(villanoName);
//         GameObject fondo = null;

//         if (fondoNames != null)
//         {
//             for (int i = 0; i < fondoNames.Length; i++)
//             {
//                 fondo = FindInSceneIncludingInactive(fondoNames[i]);
//                 if (fondo != null)
//                     break;
//             }
//         }

//         if (villano == null)
//             Debug.LogWarning("ActiveManager: No se encontró objeto " + villanoName);
//         else
//             villano.SetActive(true);

//         if (fondo == null)
//             Debug.LogWarning("ActiveManager: No se encontró fondo para " + villanoName);
//         else
//             fondo.SetActive(true);
//     }

//     private static GameObject FindInSceneIncludingInactive(string objectName)
//     {
//         if (string.IsNullOrEmpty(objectName))
//             return null;

//         Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
//         for (int i = 0; i < allTransforms.Length; i++)
//         {
//             Transform current = allTransforms[i];
//             if (current == null)
//                 continue;

//             if (!current.gameObject.scene.IsValid())
//                 continue;

//             if (current.name == objectName)
//                 return current.gameObject;
//         }

//         return null;
//     }
// }
