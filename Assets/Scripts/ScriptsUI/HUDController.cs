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
    private Label coinLabel;
    private VisualElement trophyIcon;
    private Label trophyLabel;
    private int coinCurrentFrame;
    private int trophyCurrentFrame;
    private float coinTimer;
    private float trophyTimer;

    private VisualElement skinCollapsed;
    private VisualElement skinExpanded;

    void Start()
    {
        var root = uiDocument.rootVisualElement;
        coinIcon = root.Q<VisualElement>("coin-icon");
        trophyIcon = root.Q<VisualElement>("trophy-icon");
        coinLabel = root.Q<Label>("coin-label");
        trophyLabel = root.Q<Label>("score-label");

        coinLabel.text = GameSession.Instance.monedas.ToString();
        trophyLabel.text = GameSession.Instance.globalScore.ToString();


        if (coinFrames.Count > 0)
            coinIcon.style.backgroundImage = new StyleBackground(coinFrames[0]);

        if (trophyFrames.Count > 0)
            trophyIcon.style.backgroundImage = new StyleBackground(trophyFrames[0]);

        skinCollapsed = root.Q<VisualElement>("SkinSelectorCollapsed");
        skinExpanded = root.Q<VisualElement>("SkinSelectorExpanded");

        skinCollapsed.pickingMode = PickingMode.Position;
        skinCollapsed.RegisterCallback<ClickEvent>(evt => ToggleSkinPanel());

        Debug.Log("[SkinSelector] GameSession null? " + (GameSession.Instance == null));
        Debug.Log("[SkinSelector] skinSelected: " + GameSession.Instance.skinSelected);
        Debug.Log("[SkinSelector] availableSkins count: " + GameSession.Instance.availableSkins.Count);

        SetCollapsedSkinImage(GameSession.Instance.skinSelected);
        PopulateSkinSlots();
    }

    private Sprite LoadSkinSprite(string skinName)
    {
        string spriteName = skinName + "_0";
        Sprite[] all = Resources.LoadAll<Sprite>("Skins/" + skinName);
        Debug.Log("[SkinSelector] LoadAll 'Skins/" + skinName + "' returned " + all.Length + " sprites");

        foreach (var s in all)
        {
            Debug.Log("[SkinSelector]   found sub-sprite: " + s.name);
            if (s.name == spriteName)
                return s;
        }

        if (all.Length > 0)
            return all[0];

        return null;
    }

    private void SetCollapsedSkinImage(string skinName)
    {
        Sprite sprite = LoadSkinSprite(skinName);
        if (sprite != null)
            skinCollapsed.style.backgroundImage = new StyleBackground(sprite);
    }

    private void PopulateSkinSlots()
    {
        skinExpanded.Clear();

        foreach (var skin in GameSession.Instance.availableSkins)
        {
            Debug.Log("[SkinSelector] Creating slot for: " + skin.nombre_asset);

            var slot = new VisualElement();
            slot.style.width = 100;
            slot.style.height = 100;
            slot.style.marginBottom = 5;
            slot.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0.4f));

            Sprite sprite = LoadSkinSprite(skin.nombre_asset);
            if (sprite != null)
                slot.style.backgroundImage = new StyleBackground(sprite);

            slot.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            slot.pickingMode = PickingMode.Position;

            string assetName = skin.nombre_asset;
            slot.RegisterCallback<ClickEvent>(evt => OnSkinSlotClicked(assetName));

            skinExpanded.Add(slot);
        }

        Debug.Log("[SkinSelector] Total slots created: " + skinExpanded.childCount);
    }

    private void OnSkinSlotClicked(string skinName)
    {
        GameSession.Instance.skinSelected = skinName;
        SetCollapsedSkinImage(skinName);
        Debug.Log("[SkinSelector] Skin selected: " + skinName);
    }

    private void ToggleSkinPanel()
    {
        bool isVisible = skinExpanded.resolvedStyle.display == DisplayStyle.Flex;
        skinExpanded.style.display = isVisible ? DisplayStyle.None : DisplayStyle.Flex;
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