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

    public void RegistrarRespuesta(bool correcta)
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

    public void EvaluarRank()
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
