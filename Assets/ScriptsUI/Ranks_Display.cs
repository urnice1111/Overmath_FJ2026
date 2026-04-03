using UnityEngine;

public class Ranks_Display : MonoBehaviour
{
    [System.Serializable]
    private class RankState
    {
        public string rank;
        public string triggerName;
    }


    //Configuración del Animator y mapeo de rangos a triggers
    [SerializeField] private Animator animator;
    [SerializeField] private int layerIndex = 0;
    [SerializeField] private RankState[] rankStates =   //El orden de los estados debe coincidir con el orden de los rangos en Ranks_Manager
    {
        new RankState { rank = "D", triggerName = "RangoD" },   //Rango en texto que esta en ranks_manager y el trigger que se activa en el animator
        new RankState { rank = "C", triggerName = "RangoC" },
        new RankState { rank = "B", triggerName = "RangoB" },
        new RankState { rank = "A", triggerName = "RangoA" },
        new RankState { rank = "OverMath", triggerName = "RangoMax" }
    };

    //Al iniciar, se obtiene el rango actual del Ranks_Manager y se activa el trigger correspondiente en el animator
    void Start()
    {
        if (animator == null) //Verifica que el Animator esté asignado
        {
            Debug.LogError("Ranks_Display: Animator no asignado.");
            return;
        }

        if (Ranks_Manager.Instance == null)  //Verifica que el Ranks_Manager esté asignado en la escena
        {
            Debug.LogError("Ranks_Display: No existe Ranks_Manager.Instance en esta escena.");
            return;
        }

        //Obtener el rango actual del Ranks_Manager
        string rank = Ranks_Manager.Instance.rank;  //Obtiene el rango actual y se actualiza cada vez que el jugador responde correctamente o incorrectamente a una pregunta
        string targetTrigger = GetTriggerByRank(rank);  //Obtiene el nombre del trigger que corresponde al rango actual


        //Esto es para verificar que el trigger existe en el Animator antes de activarlo
        if (string.IsNullOrEmpty(targetTrigger))
        {
            Debug.LogWarning("Ranks_Display: Rank sin estado configurado: " + rank);
            return;
        }


        //Verificar que el trigger existe en el Animator
        int triggerHash = Animator.StringToHash(targetTrigger); //StringToHash es una función que convierte el nombre del trigger en un hash para optimizar la búsqueda en el Animator
        bool triggerExists = false;  //Primero se asume que el trigger no existe


        //Al momento de buscar el trigger, se recorre la lista de parámetros del Animator para verificar que el trigger existe antes de activarlo
        for (int i = 0; i < animator.parameters.Length; i++)
        {
            AnimatorControllerParameter parameter = animator.parameters[i]; //Unity tiene una clase AnimatorControllerParameter que representa cada parámetro del Animator, y se puede acceder a su nombre, tipo, etc.
            if (parameter.nameHash == triggerHash && parameter.type == AnimatorControllerParameterType.Trigger) //Si el hash del parámetro coincide con el hash del trigger que queremos activar, y el tipo del parámetro es Trigger, entonces el trigger existe en el Animator
            {
                triggerExists = true;
                break;
            }
        }

        if (!triggerExists) //Si después de recorrer todos los parámetros del Animator no se encuentra el trigger, se muestra un error en la consola y no se intenta activar el trigger
        {
            Debug.LogError("Ranks_Display: El trigger no existe en el Animator: " + targetTrigger);
            return;
        }

        animator.SetTrigger(triggerHash); //Si el trigger existe, se activa el trigger en el Animator para mostrar la animación correspondiente
    }

    private string GetTriggerByRank(string rank)
    {
        for (int i = 0; i < rankStates.Length; i++) //Recorre el array de RankState, que es una clase que contiene el nombre del rango y el nombre del trigger correspondiente en el Animator, para encontrar el trigger que corresponde al rango actual del Ranks_Manager
        {
            if (rankStates[i].rank == rank)
            {
                return rankStates[i].triggerName;
            }
        }

        return null;
    }
}
