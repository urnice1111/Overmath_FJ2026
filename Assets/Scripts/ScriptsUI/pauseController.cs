using UnityEngine;

public class pauseController : MonoBehaviour
{
    public static bool isGamePuased { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        isGamePuased = pause;
        Time.timeScale = pause ? 0 : 1;
    }
}
