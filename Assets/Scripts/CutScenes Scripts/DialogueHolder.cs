using UnityEngine;
using System.Collections;

namespace  DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private Coroutine sequenceRoutine;

        private void OnEnable()
        {
            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
            }

            sequenceRoutine = StartCoroutine(dialogueSequence());
        }

        private void OnDisable()
        {
            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
                sequenceRoutine = null;
            }

            Deactivate();
        }

        private IEnumerator dialogueSequence()
        {
            Deactivate();

            for(int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                DialogueLine dialogue = transform.GetChild(i).GetComponent<DialogueLine>();
                if (dialogue != null)
                {
                    yield return new WaitUntil(() => dialogue.Finished);

                    // Espera una entrada de avance (mouse/touch/teclado) para pasar a la siguiente línea.
                    yield return new WaitUntil(AdvancePressed);
                    yield return new WaitUntil(() => !AdvanceHeld());
                }
                else
                {
                    Debug.LogWarning($"No se encontró DialogueLine en hijo {i}", this);
                }
            }

            Deactivate();
            gameObject.SetActive(false);
            sequenceRoutine = null;
        }

        private void Deactivate()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private static bool AdvancePressed()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                return true;
            }

            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }

        private static bool AdvanceHeld()
        {
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            {
                return true;
            }

            return Input.touchCount > 0;
        }
    }
}