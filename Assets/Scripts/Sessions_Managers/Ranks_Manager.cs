using UnityEngine;

public class Ranks_Manager : MonoBehaviour
{
    public static Ranks_Manager Instance { get; private set; }

    public int streak = 0;
    public string rank = "D";

    [SerializeField] private string escenaResultado = "SampleScene";
    [Header("Audio de rangos")]
    [SerializeField] private AudioSource rankUpAudioSource;
    [SerializeField] private AudioClip rankUpSound;
    [SerializeField, Range(0f, 1f)] private float rankUpVolume = 1f;
    [SerializeField] private AudioSource overMathLoopAudioSource;
    [SerializeField] private AudioClip overMathLoopSound;
    [SerializeField, Range(0f, 1f)] private float overMathLoopVolume = 0.65f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RefreshOverMathLoop();
    }

    public void RegistrarRespuesta(bool correcta) //Este método se llama cada vez que el jugador responde correctamente o incorrectamente a una pregunta, y actualiza la racha y el rango
    {
        string rankAnterior = rank;

        if (correcta)
        {
            streak++;
        }
        else
        {
            streak = 0;
        }

        EvaluarRank();
        PlayRankAudios(rankAnterior, rank);
        Debug.Log("Ranks_Manager: streak=" + streak + " rank=" + rank);

        if (Ranks_Display.Instance != null)
        {
            Ranks_Display.Instance.ActualizarRango(rank);
        }
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

    private void PlayRankAudios(string rankAnterior, string rankActual)
    {
        if (GetRankValue(rankActual) > GetRankValue(rankAnterior))
        {
            PlayRankUpSound();
        }

        RefreshOverMathLoop();
    }

    private int GetRankValue(string rankName)
    {
        switch (rankName)
        {
            case "D":
                return 0;
            case "C":
                return 1;
            case "B":
                return 2;
            case "A":
                return 3;
            case "OverMath":
                return 4;
            default:
                return -1;
        }
    }

    private void PlayRankUpSound()
    {
        if (rankUpAudioSource == null || rankUpSound == null)
        {
            return;
        }

        rankUpAudioSource.PlayOneShot(rankUpSound, rankUpVolume);
    }

    private void RefreshOverMathLoop()
    {
        if (overMathLoopAudioSource == null || overMathLoopSound == null)
        {
            return;
        }

        bool isOverMath = rank == "OverMath";

        if (isOverMath)
        {
            overMathLoopAudioSource.clip = overMathLoopSound;
            overMathLoopAudioSource.volume = overMathLoopVolume;
            overMathLoopAudioSource.loop = true;

            if (!overMathLoopAudioSource.isPlaying)
            {
                overMathLoopAudioSource.Play();
            }

            return;
        }

        if (overMathLoopAudioSource.isPlaying)
        {
            overMathLoopAudioSource.Stop();
        }
    }

    
}
