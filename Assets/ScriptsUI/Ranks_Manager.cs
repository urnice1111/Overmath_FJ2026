using UnityEngine;

public class Ranks_Manager : MonoBehaviour
{
    public static Ranks_Manager Instance { get; private set; }

    public int streak = 0;
    public string rank = "D";

    [SerializeField] private string escenaResultado = "SampleScene";

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

    public void RegistrarRespuesta(bool correcta) //Este método se llama cada vez que el jugador responde correctamente o incorrectamente a una pregunta, y actualiza la racha y el rango
    {
        if (correcta)
        {
            streak++;
        }
        else
        {
            streak = 0;
        }

        EvaluarRank();
        Debug.Log("Ranks_Manager: streak=" + streak + " rank=" + rank);
    }

    public void EvaluarRank()  //Evalúa el rango del jugador en función de su racha actual (streak) y actualiza la variable rank:
    {
        if (streak >= 10)
        {
            rank = "OverMath";
        }
        else if (streak >= 7)
        {
            rank = "A";
        }
        else if (streak >= 5)
        {
            rank = "B";
        }
        else if (streak >= 3)
        {
            rank = "C";
        }
        else
        {
            rank = "D";
        }

        Debug.Log("Ranks_Manager: Rank evaluado: " + rank);
    }

    
}
