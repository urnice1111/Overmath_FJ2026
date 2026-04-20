using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace DialogueSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DialogueLine : DialogueBaseClass
    {
        private TextMeshProUGUI textHolder;
        [SerializeField] private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private TMP_FontAsset textFont;
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;


        private void OnEnable()
        {
            StopAllCoroutines(); // Detener cualquier diálogo en curso al activar este objeto   

            textHolder = GetComponent<TextMeshProUGUI>();

            if (textHolder == null)
            {
                Debug.LogError("DialogueLine requires a UnityEngine.UI.Text component on the same GameObject.", this);
                return;
            }

            if (imageHolder != null && characterSprite != null)
            {
                imageHolder.sprite = characterSprite;
                imageHolder.preserveAspect = true;
            }

            // Resetear estado y reiniciar el diálogo al activarse
            Finished = false;
            if (!string.IsNullOrEmpty(input))
            {
                Debug.Log($"Iniciando diálogo: '{input}'", this);
                StartCoroutine(WriteTextAndMarkFinished(input, textHolder, textColor, textFont, delay));
            }
        }

        private IEnumerator WriteTextAndMarkFinished(string input, TextMeshProUGUI textHolder, Color textColor, TMP_FontAsset textFont, float delay)
        {
            yield return StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay, delayBetweenLines));
            Finished = true;
            Debug.Log($"Diálogo terminado: '{input}'", this);
        }
    }
}