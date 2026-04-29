using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class skinsAvailableInfo
{
    public string nombre_asset;
    public string descripcion;
    public int desbloqueado;
    public int precio;
}

public class comprarSkinData
{
    public int cuentaId;
    public string assetName;
}

[System.Serializable]
public class skinsResult
{
    public skinsAvailableInfo[] response;
}

public class StoreManager : MonoBehaviour
{
    public GameObject panelSkin;
    public skinsAvailableInfo[] skins {get; private set;}
    public bool skinsLoaded {get; private set;} = false;

    public static StoreManager Instance {get; private set;}
    public int currentChestIndex {get; set;}
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

    public void ClickComprar()
    {
        StartCoroutine(ComprarSkin());
    }


    public IEnumerator ComprarSkin()
    {
        skinsAvailableInfo skin = skins[currentChestIndex];

        comprarSkinData buySkinData = new comprarSkinData
        {
            cuentaId = GameSession.Instance.userId,
            assetName = skin.nombre_asset
        };

        string json = JsonUtility.ToJson(buySkinData);
        string url = "http://localhost:8080/buy_skin";

        using UnityWebRequest www = UnityWebRequest.Post(url, json, "application/json");

        yield return www.SendWebRequest();

        bool success = www.result == UnityWebRequest.Result.Success;
        if (!success)
            {Debug.LogError("Error comprando skin: " + www.error);}
        else
            {StartCoroutine(getSkinsInfo());}
            
        
    }
}
