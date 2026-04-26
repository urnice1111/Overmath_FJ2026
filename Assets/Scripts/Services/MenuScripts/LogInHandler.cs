using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LogInHandler : MonoBehaviour
{

    [SerializeField] private UIDocument mainMenu;


    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;

        public string deviceType;
    }
    
    [System.Serializable]
    public class LoginResponse
    {
        public SignInResult result;
    }

    [System.Serializable]
    public class SkinInfo
    {
        public string nombre_asset;
        public string descripcion;
    }

    [System.Serializable]
    public class SignInResult
    {
        public bool ok;
        public UserInfo user;
        public SkinInfo[] skins;
    }

    [System.Serializable]
    public class UserInfo{
        public int id_cuenta;
        public string correo;
        public string skin_actual;
        public int monedas;
        public int score_global;
        
    }



    private TextField emailEntry;
    private TextField passwordEntry;
    private Label resultMessage;
    
    private Button logInButton;
    private Button goBackButton;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        emailEntry = root.Q<TextField>("EmailField");
        passwordEntry = root.Q<TextField>("PasswordField");
        resultMessage = root.Q<Label>("Response");

        logInButton = root.Q<Button>("BtnIniciar");
        logInButton.clicked += ConfirmCredentials;

        goBackButton = root.Q<Button>("BtnRegresar");
        goBackButton.clicked += GoBack;
    }

    void OnDisable()
    {
        logInButton.clicked -= ConfirmCredentials;
        goBackButton.clicked -= GoBack;
    }
    private void ConfirmCredentials()
    {
        string email = emailEntry.value;
        string password = passwordEntry.value;
        string type = SystemInfo.deviceType.ToString();


        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Please enter both email and password.", Color.red);
            return;
        }

        StartCoroutine(PostRequestLogin(email, password, type));
    }

    IEnumerator PostRequestLogin(string email, string password, string deviceType)
    {
        LoginData loginData = new LoginData
        {
            email = email,
            password = password,
            deviceType = deviceType
        };

        string jsonBody = JsonUtility.ToJson(loginData);

        //https://udqzin2siulhcshfje2amhkiey0pkadb.lambda-url.us-east-1.on.aws

        using UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/login", jsonBody, "application/json");


        yield return www.SendWebRequest();

        // if (www.result != UnityWebRequest.Result.Success)
        // {
        //     Debug.LogError("Login request failed: " + www.error);
        //     ShowMessage("Unable to contact server. Try again later.", Color.red);
        //     yield break;
        // }

        if (www.responseCode == 201)
        {

            Debug.Log("Response text: " + www.downloadHandler.text);
            Debug.Log("GameSession.Instance is null? " + (GameSession.Instance == null));

            LoginResponse response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
            GameSession.Instance.userId = response.result.user.id_cuenta;
            GameSession.Instance.skinSelected = response.result.user.skin_actual;
            GameSession.Instance.monedas = response.result.user.monedas;
            GameSession.Instance.globalScore = response.result.user.score_global;

            GameSession.Instance.availableSkins.Clear();
            if (response.result.skins != null)
                GameSession.Instance.availableSkins.AddRange(response.result.skins);

            Debug.Log(GameSession.Instance.userId);
            Debug.Log(GameSession.Instance.skinSelected + " Esta skin esta seleccionada");
            Debug.Log("Skins disponibles: " + GameSession.Instance.availableSkins.Count);

            Debug.Log(www.responseCode);
            ShowMessage("Login successful! Loading game...", Color.green);
            SceneManager.LoadScene("PantallaPrincipal");
            // StartCoroutine(RegisterSessionInDB(response.user.id));
        }
        else if (www.responseCode == 401 || www.responseCode == 403)
        {
            ShowMessage("Email or password is incorrect.", Color.red);
        }
        else
        {
            Debug.LogWarning("Unexpected login response: " + www.responseCode + " - " + www.downloadHandler.text);
            ShowMessage("Login failed. Please try again.", Color.red);
        }
    }
    
    
    private void ShowMessage(string text, Color color)
    {
        if (resultMessage != null)
        {
            resultMessage.text = text;
            resultMessage.style.color = color;
            resultMessage.style.opacity = 1;
        }
    }


    private void GoBack()
    {
        gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}