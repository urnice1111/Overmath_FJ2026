using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class HUDController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private List<Sprite> coinFrames;
    [SerializeField] private List<Sprite> trophyFrames;
    [SerializeField] private float frameRate = 6f;

    private VisualElement coinIcon;
    private VisualElement trophyIcon;
    private int coinCurrentFrame;
    private int trophyCurrentFrame;
    private float coinTimer;
    private float trophyTimer;

    void Start()
    {
        var root = uiDocument.rootVisualElement;
        coinIcon = root.Q<VisualElement>("coin-icon");
        trophyIcon = root.Q<VisualElement>("trophy-icon");

        if (coinFrames.Count > 0)
            coinIcon.style.backgroundImage = new StyleBackground(coinFrames[0]);

        if (trophyFrames.Count > 0)
            trophyIcon.style.backgroundImage = new StyleBackground(trophyFrames[0]);
    }

    void Update()
    {
        AnimateIcon(coinFrames, ref coinCurrentFrame, ref coinTimer, coinIcon);
        AnimateIcon(trophyFrames, ref trophyCurrentFrame, ref trophyTimer, trophyIcon);
    }

    private void AnimateIcon(List<Sprite> frames, ref int currentFrame, ref float timer, VisualElement icon)
    {
        if (frames.Count <= 1) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Count;
            icon.style.backgroundImage = new StyleBackground(frames[currentFrame]);
        }
    }
}