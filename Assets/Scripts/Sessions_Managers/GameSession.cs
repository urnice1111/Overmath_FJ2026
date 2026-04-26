using System.Collections.Generic;
using UnityEngine;

/*
    This is the gamesession manager, a singleton attached to its own gameobject that handles the difficulty selected.
    On DifficultySelector.cs this instance is called to acces this methods (SetDificultad()).
    On PreguntaManager.cs this instance is called by the GetQuestion method (API call to DB) by the method GetEndpoint. 
    Change this if the endpoint is modified.

    - Emiliano
*/

public enum Dificultad{ Facil, Normal, Dificil}

[System.Serializable]
public class IslaProgreso
{
    public string isla_id;
    public bool desbloqueada;
}

[System.Serializable]
public class ProgresoResponse
{
    public bool tutorial_completado;
    public IslaProgreso[] islas;
}

public class GameSession : MonoBehaviour
{
    public static GameSession Instance {get; private set;}

    public Dificultad DificultadActual {get; private set;} = Dificultad.Normal;

    public string IslaActual {get; private set;}

    public bool IsTutorial { get; set; }

    public int sessionId {get; set;}

    public int userId {get; set;}

    public string skinSelected {get; set;}

    public int globalScore {get; set;}

    public int monedas {get; set;}

    public List<LogInHandler.SkinInfo> availableSkins {get; set;} = new List<LogInHandler.SkinInfo>();

    public bool tutorialCompletado { get; set; }
    public List<IslaProgreso> islasProgreso { get; set; } = new List<IslaProgreso>();

    public bool IsIslaDesbloqueada(string islaId)
    {
        var isla = islasProgreso.Find(i => i.isla_id == islaId);
        return isla != null && isla.desbloqueada;
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameSession Awake: IslaActual = " + IslaActual);
    }

    public void SetDificultad(Dificultad d) => DificultadActual = d;
    public void SetIsla(string isla) => IslaActual = isla;


    public string GetEndpoint()
    {
        string diff = DificultadActual switch
        {
          Dificultad.Facil => "Easy",
          Dificultad.Normal => "Normal",
          Dificultad.Dificil => "Dificil",
          _ => "Dificil"  
        };

        return $"https://udqzin2siulhcshfje2amhkiey0pkadb.lambda-url.us-east-1.on.aws/get_questions/{IslaActual}/{diff}";
    }
    public int GetNivel()
    {
        if (IslaActual == "isla_infinito")
            return 15; // único nivel para infinito

        int islaIndex = IslaActual switch
        {
            "isla_suma" => 0,
            "isla_resta" => 1,
            "isla_multi" => 2,
            "isla_div" => 3,
            "isla_comb" => 4,
            _ => 0
        };

        return islaIndex * 3 + (int)DificultadActual;
    }

    // Later hp bar logic here:
}
