// This script controls the transition to the villain scene (effect)
using UnityEngine;
using UnityEngine.EventSystems;

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

            if (ScreenFadereManager.Instance != null)
            {
                ScreenFadereManager.Instance.ChangeScene(sceneName);
            }
            else
            {
                Debug.LogWarning("ScreenFadereManager no encontrado. Cargando escena sin transición.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
    }


    
}
