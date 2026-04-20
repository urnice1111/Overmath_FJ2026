using UnityEngine;
using System.Collections;

namespace  DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(dialogueSequence());
        }
        private IEnumerator dialogueSequence()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                DialogueLine dialogue = transform.GetChild(i).GetComponent<DialogueLine>();
                if (dialogue != null)
                {
                    Debug.Log($"Esperando a que termine diálogo {i}...", dialogue);
                    yield return new WaitUntil(() => dialogue.Finished);
                    Debug.Log($"Diálogo {i} completado.", dialogue);

                    // Espera un clic fresco para avanzar al siguiente diálogo
                    yield return new WaitUntil(() => !Input.GetMouseButton(0));
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                }
                else
                {
                    Debug.LogWarning($"No se encontró DialogueLine en hijo {i}", this);
                }
            }

            Deactivate();
            gameObject.SetActive(false);
            Debug.Log("Secuencia de diálogos completada.", this);
        }

        private void Deactivate()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}