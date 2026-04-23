using UnityEngine;
using System.Collections;


// The purpose of this script is to handle Villian (as a GameObject on SampleScene) from outside classes
// just like in DisplayResult.cs where it acces its public methods
public class VillianState : MonoBehaviour
{

    private Animator animator;
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
        StartCoroutine(EsperarYRegresarEstadoNormal());
    }

    private IEnumerator EsperarYRegresarEstadoNormal()
    {
        yield return new WaitForSeconds(1.11f);
        animator.SetTrigger("NoAnsw");
        
    }
}




