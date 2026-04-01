using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;
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

    }

    public void MakeAnimationCorrectAnsw()
    {
        animator.SetTrigger("CorrectAnsw");
        StartCoroutine(EsperarYRegresarEstadoNormal());
    }

    public void MakeAnimationWrongAnsw()
    {
        animator.SetTrigger("WrongAnsw");
        StartCoroutine(EsperarYRegresarEstadoNormal());
    }

    private IEnumerator EsperarYRegresarEstadoNormal()
    {
        yield return new WaitForSeconds(1.11f);
        animator.SetTrigger("NoAnsw");
        
    }
}
