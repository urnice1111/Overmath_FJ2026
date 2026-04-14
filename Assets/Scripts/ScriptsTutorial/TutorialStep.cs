using UnityEngine;

public enum TutorialAdvanceMode
{
    TapToContinue,
    WaitForSeconds,
    WaitForSelection,
    WaitForMesaOpen,
    WaitForSlotsReady,
    WaitForEvaluate,
    WaitForResult,
    AutoRelease
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Overmath/Tutorial Step")]
public class TutorialStep : ScriptableObject
{
    public string titulo;
    [TextArea(2, 4)] public string texto;
    public TutorialAdvanceMode advanceMode;
    public float waitSeconds = 2f;
    public int seleccionesRequeridas = 0;

    [Tooltip("Nombre del GameObject en la escena que se debe resaltar (dejar vacio si no aplica)")]
    public string highlightTargetName;
}
