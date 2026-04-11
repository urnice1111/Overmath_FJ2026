using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MoveToVillianScene : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
            // If the pause menu triggers scene change, resume time first.
            pauseController.SetPause(false);
            SceneManager.LoadScene(sceneName);  
    }


    
}
