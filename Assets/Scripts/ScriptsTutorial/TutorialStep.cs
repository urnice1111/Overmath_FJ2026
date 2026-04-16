using UnityEngine;

public enum TutorialAdvanceMode
{
    TapToContinue,
    WaitForSeconds,
    WaitForSelection,
    WaitForMesaOpen,
    WaitForSlotsReady,
    WaitForEvaluate,
    WaitForCorrectAnswer
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Overmath/Tutorial Step")]
public class TutorialStep : ScriptableObject
{
    [TextArea(2, 5)] public string texto;
    public TutorialAdvanceMode advanceMode;
    public float waitSeconds = 2f;
    public int seleccionesRequeridas = 0;

    [Tooltip("Name of the GameObject in the scene to spotlight (leave empty for no spotlight)")]
    public string highlightTargetName;
}
