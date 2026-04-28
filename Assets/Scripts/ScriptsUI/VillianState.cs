using UnityEngine;
using System.Collections;


// The purpose of this script is to handle Villian (as a GameObject on SampleScene) from outside classes
// just like in DisplayResult.cs where it acces its public methods
public class VillianState : MonoBehaviour
{

    private Animator animator;
    
    [Header("Audio de respuestas")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctAnswSound;
    [SerializeField] private AudioClip wrongAnswSound;
    [SerializeField, Range(0f, 1f)] private float audioVolume = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"VillianState [{gameObject.name}]: Animator NOT found on {gameObject.name}");
        }
        else
        {
            Debug.Log($"VillianState [{gameObject.name}]: Animator found successfully");
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning($"VillianState [{gameObject.name}]: AudioSource NOT found on {gameObject.name}. Sounds won't play.");
            }
        }
    }

    public void MakeAnimationCorrectAnsw()
    {
        if (animator == null)
        {
            Debug.LogError($"VillianState [{gameObject.name}]: Animator is null! Cannot play CorrectAnsw animation");
            return;
        }
        Debug.Log($"VillianState [{gameObject.name}]: Playing CorrectAnsw animation");
        animator.SetTrigger("CorrectAnsw");
        PlayCorrectAnswSound();
        StartCoroutine(EsperarYRegresarEstadoNormal());
    }

    public void MakeAnimationWrongAnsw()
    {
        if (animator == null)
        {
            Debug.LogError($"VillianState [{gameObject.name}]: Animator is null! Cannot play WrongAnsw animation");
            return;
        }
        Debug.Log($"VillianState [{gameObject.name}]: Playing WrongAnsw animation");
        animator.SetTrigger("WrongAnsw");
        PlayWrongAnswSound();
        StartCoroutine(EsperarYRegresarEstadoNormal());
    }

    private void PlayCorrectAnswSound()
    {
        if (audioSource == null || correctAnswSound == null)
        {
            return;
        }
        audioSource.PlayOneShot(correctAnswSound, audioVolume);
    }

    private void PlayWrongAnswSound()
    {
        if (audioSource == null || wrongAnswSound == null)
        {
            return;
        }
        audioSource.PlayOneShot(wrongAnswSound, audioVolume);
    }

    private IEnumerator EsperarYRegresarEstadoNormal()
    {
        yield return new WaitForSeconds(1.11f);
        animator.SetTrigger("NoAnsw");
        
    }
}




