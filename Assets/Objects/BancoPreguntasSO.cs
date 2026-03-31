using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BancoPreguntasSO", menuName = "Scriptable Objects/BancoPreguntasSO")]
public class BancoPreguntasSO : ScriptableObject
{
    public List<PreguntaSO> preguntas;
    
}
