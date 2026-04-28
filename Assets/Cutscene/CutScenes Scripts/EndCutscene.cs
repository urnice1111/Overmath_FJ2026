using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Start()
    {
        StartCoroutine(EndCutsceneCoroutine());
    }

    IEnumerator EndCutsceneCoroutine()
    {
        yield return new WaitForSeconds(15f);

        if (!string.IsNullOrWhiteSpace(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
            yield break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
