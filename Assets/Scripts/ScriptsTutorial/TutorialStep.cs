using UnityEngine;

public enum TutorialAdvanceMode
{
    TapToContinue,
    GoBack,
    
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

    [Tooltip("Enable for steps where the player needs to click a world-space GameObject (not a UI button)")]
    public bool allowWorldClicks;

    [Tooltip("Target camera position (used by GoBack mode)")]
    public Vector2 cameraTargetPos;

    public bool hideDimmer;
}
