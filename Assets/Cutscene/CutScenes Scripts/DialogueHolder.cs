using UnityEngine;
using System.Collections;
using UnityEngine.Playables;

namespace  DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        public static bool IsDialogueActive { get; private set; }

        [Header("Timeline control (optional)")]
        [SerializeField] private PlayableDirector cutsceneDirector;
        [SerializeField, Min(0f)] private float timelineAdvanceWindow = 0.35f;
        [SerializeField, Min(0f)] private float nextDialogueDelay = 0.1f;
        [SerializeField] private bool advanceAfterLastLine;

        private Coroutine sequenceRoutine;

        private void OnEnable()
        {
            IsDialogueActive = true;
            PauseTimeline();

            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
            }

            sequenceRoutine = StartCoroutine(dialogueSequence());
        }

        private void OnDisable()
        {
            IsDialogueActive = false;
            PauseTimeline();

            if (sequenceRoutine != null)
            {
                StopCoroutine(sequenceRoutine);
                sequenceRoutine = null;
            }

            Deactivate();
        }

        private void OnDestroy()
        {
            IsDialogueActive = false;
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

                    bool hasNextLine = i < transform.childCount - 1;
                    if (hasNextLine || advanceAfterLastLine)
                    {
                        yield return StartCoroutine(AdvanceTimelineWindow());
                    }

                    if (hasNextLine && nextDialogueDelay > 0f)
                    {
                        yield return new WaitForSeconds(nextDialogueDelay);
                    }
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

        private IEnumerator AdvanceTimelineWindow()
        {
            if (cutsceneDirector == null || timelineAdvanceWindow <= 0f)
            {
                yield break;
            }

            cutsceneDirector.Play();
            yield return new WaitForSeconds(timelineAdvanceWindow);
            PauseTimeline();
        }

        private void PauseTimeline()
        {
            if (cutsceneDirector != null)
            {
                cutsceneDirector.Pause();
            }
        }
    }
}