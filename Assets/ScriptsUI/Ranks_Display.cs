using UnityEngine;

public class Ranks_Display : MonoBehaviour
{
    [System.Serializable]
    private class RankState
    {
        public string rank;
        public string triggerName;
    }

    [SerializeField] private Animator animator;
    [SerializeField] private int layerIndex = 0;
    [SerializeField] private RankState[] rankStates =
    {
        new RankState { rank = "D", triggerName = "RangoD" },
        new RankState { rank = "C", triggerName = "RangoC" },
        new RankState { rank = "B", triggerName = "RangoB" },
        new RankState { rank = "A", triggerName = "RangoA" },
        new RankState { rank = "OverMath", triggerName = "RangoMax" }
    };
    
    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Ranks_Display: Animator no asignado.");
            return;
        }

        if (Ranks_Manager.Instance == null)
        {
            Debug.LogError("Ranks_Display: No existe Ranks_Manager.Instance en esta escena.");
            return;
        }

        string rank = Ranks_Manager.Instance.rank;
        string targetTrigger = GetTriggerByRank(rank);

        if (string.IsNullOrEmpty(targetTrigger))
        {
            Debug.LogWarning("Ranks_Display: Rank sin estado configurado: " + rank);
            return;
        }

        int triggerHash = Animator.StringToHash(targetTrigger);
        bool triggerExists = false;

        for (int i = 0; i < animator.parameters.Length; i++)
        {
            AnimatorControllerParameter parameter = animator.parameters[i];
            if (parameter.nameHash == triggerHash && parameter.type == AnimatorControllerParameterType.Trigger)
            {
                triggerExists = true;
                break;
            }
        }

        if (!triggerExists)
        {
            Debug.LogError("Ranks_Display: El trigger no existe en el Animator: " + targetTrigger);
            return;
        }

        animator.SetTrigger(triggerHash);
    }

    private string GetTriggerByRank(string rank)
    {
        for (int i = 0; i < rankStates.Length; i++)
        {
            if (rankStates[i].rank == rank)
            {
                return rankStates[i].triggerName;
            }
        }

        return null;
    }
}
