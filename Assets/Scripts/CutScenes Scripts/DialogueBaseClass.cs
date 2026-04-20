using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{ 
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool Finished { get; protected set; }
        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, Color textColor, TMP_FontAsset textFont, float delay, float delayBetweenLines)
        {
            textHolder.color = textColor;
            if (textFont != null)
                textHolder.font = textFont;

            textHolder.text = ""; // Limpiar el texto antes de escribir
            
            // Asegurar que el componente está habilitado
            textHolder.enabled = true;
            textHolder.gameObject.SetActive(true);
            
            Debug.Log($"Iniciando escritura de texto: '{input}' con delay: {delay}s", textHolder);
            
            for(int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                yield return new WaitForSeconds(delay);
            }
            Debug.Log($"Texto completado: '{textHolder.text}'", textHolder);
            Finished = true;
        }
    }
}
