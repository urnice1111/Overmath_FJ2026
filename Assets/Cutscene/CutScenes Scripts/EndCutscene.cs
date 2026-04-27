using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EndCutsceneCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EndCutsceneCoroutine()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
