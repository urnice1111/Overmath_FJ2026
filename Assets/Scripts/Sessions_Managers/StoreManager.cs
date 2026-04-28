using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class skinsAvailableInfo
{
    public string nombre_asset;
    public string descripcion;
    public int desbloqueado;
}

[System.Serializable]
public class skinsResult
{
    public skinsAvailableInfo[] response;
}
public class StoreManager : MonoBehaviour
{
    public skinsAvailableInfo[] skins {get; private set;}
    public bool skinsLoaded {get; private set;} = false;

    public static StoreManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(getSkinsInfo());
    }

    private IEnumerator getSkinsInfo()
    {
        int userId = GameSession.Instance.userId;
        string url = $"http://localhost:8080/get_skins_for_store/{userId}";

        using UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(www.error);
            yield break;
        }

        string rawJson = "{\"response\":" + www.downloadHandler.text + "}";
        skinsResult result = JsonUtility.FromJson<skinsResult>(rawJson);

        skins = result.response;
        skinsLoaded = true;

        Debug.Log("Skins cargadas: " + skins.Length);

        
    }
}
