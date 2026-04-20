using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        [SerializeField] private TextMeshProUGUI textHolder;
        [SerializeField] private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private TMP_FontAsset textFont;
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;
        private Coroutine writingRoutine;

        private void Awake()
        {
            if (textHolder == null)
            {
                textHolder = GetComponent<TextMeshProUGUI>();
                textHolder.text = ""; // Limpiar el texto antes de escribir
            }

            if (textHolder == null)
            {
                textHolder = GetComponentInChildren<TextMeshProUGUI>(true);
            }
        }

        private void OnEnable()
        {
            Finished = false;

            if (textHolder == null)
            {
                textHolder = GetComponent<TextMeshProUGUI>();
                textHolder.text = ""; // Limpiar el texto antes de escribir
                Debug.LogError("DialogueLine requires a TextMeshProUGUI component on this object or a child.", this);
            }

            if (imageHolder != null && characterSprite != null)
            {
                imageHolder.sprite = characterSprite;
                imageHolder.preserveAspect = true;
            }

            textHolder.text = string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                if (writingRoutine != null)
                {
                    StopCoroutine(writingRoutine);
                }

                writingRoutine = StartCoroutine(WriteTextAndMarkFinished(input, textHolder, textColor, textFont, delay));
            }
        }

        private void OnDisable()
        {
            if (writingRoutine != null)
            {
                StopCoroutine(writingRoutine);
                writingRoutine = null;
            }
        }

        private IEnumerator WriteTextAndMarkFinished(string input, TextMeshProUGUI textHolder, Color textColor, TMP_FontAsset textFont, float delay)
        {
            yield return StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay, delayBetweenLines));
            Finished = true;
            writingRoutine = null;
        }
    }
}