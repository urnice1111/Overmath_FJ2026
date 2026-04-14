using UnityEngine;

/*
    This is the gamesession manager, a singleton attached to its own gameobject that handles the difficulty selected.
    On DifficultySelector.cs this instance is called to acces this methods (SetDificultad()).
    On PreguntaManager.cs this instance is called by the GetQuestion method (API call to DB) by the method GetEndpoint. 
    Change this if the endpoint is modified.

    - Emiliano
*/

public enum Dificultad{ Facil, Normal, Dificil}

public class GameSession : MonoBehaviour
{
    public static GameSession Instance {get; private set;}

    public Dificultad DificultadActual {get; private set;} = Dificultad.Normal;

    public string IslaActual {get; private set;} = "isla_suma";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

        return $"http://localhost:3000/get_questions/{IslaActual}/{diff}";
    }

    // Later hp bar logic here:
}
