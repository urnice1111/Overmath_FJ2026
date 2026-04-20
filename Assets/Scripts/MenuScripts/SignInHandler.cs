using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SignInHandler : MonoBehaviour
{
    [SerializeField] private UIDocument logInDocument;

    [System.Serializable]
    public class SignInData
    {
        public string email;
        public string username;
        public string password;
        public string name;
        public string last_name;
        public string date;
    }

    private TextField emailEntry;
    private TextField usernameEntry;
    private TextField passwordEntry;
    private TextField confirmPasswordEntry;
    private TextField nameEntry;
    private TextField last_nameEntry;

    private IntegerField dayEntry;
    private IntegerField monthEntry;
    private IntegerField yearEntry;

    private Label resultMessage;


    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        emailEntry = root.Q<TextField>("EmailField");
        usernameEntry = root.Q<TextField>("UsernameField");
        passwordEntry = root.Q<TextField>("PasswordField");
        confirmPasswordEntry = root.Q<TextField>("ConfirmPasswordField");
        nameEntry = root.Q<TextField>("FirstNameField");
        last_nameEntry = root.Q<TextField>("LastNameField");
        resultMessage = root.Q<Label>("Response");

        // DAte

        dayEntry = root.Q<IntegerField>("DayField");
        monthEntry = root.Q<IntegerField>("MonthField");
        yearEntry = root.Q<IntegerField>("YearField");



        Button signInButton = root.Q<Button>("BtnRegistrar");
        signInButton.clicked += ConfirmCredentials;


    }

    private void ConfirmCredentials()
    {
        string email = emailEntry.value;
        string username = usernameEntry.value;
        string password = passwordEntry.value;
        string confirmPassword = confirmPasswordEntry.value;
        string name = nameEntry.value;
        string last_name = last_nameEntry.value;

        int day = dayEntry.value;
        int month = monthEntry.value;
        int year = yearEntry.value;

        string fechaNacimiento = $"{year}-{month:D2}-{day:D2}";



        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Please enter both email and password.", Color.red);
            return;
        }

        if (confirmPassword != password)
        {
            ShowMessage("La contraseña no coincide", Color.red);
            return;
        }

        StartCoroutine(PostRequestSignIn(email, username, password, name, last_name, fechaNacimiento));


    }


    private IEnumerator PostRequestSignIn(string email, string username, string password, string name, string last_name, string date)
    {
        SignInData signInData = new SignInData
        {
          email = email,
          username = username,
          password = password,
          name = name,
          last_name = last_name,
          date = date
        };

        string jsonBody = JsonUtility.ToJson(signInData);

        using UnityWebRequest www = UnityWebRequest.Post("https://q623ldzsbzpk3j6nktpzcvqi7y0qrpsr.lambda-url.us-east-1.on.aws/register_jugador", jsonBody, "application/json");

        yield return www.SendWebRequest();

        if (www.responseCode == 201)
        {
            Debug.Log(www.responseCode);
            ShowMessage("SignIn successful! Loading game...", Color.green);
            OnSuccesfullSignIn();
            
            // StartCoroutine(RegisterSessionInDB(response.user.id));
        } else
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


    private void OnSuccesfullSignIn()
    {
        gameObject.SetActive(false);
        logInDocument.gameObject.SetActive(true);
    }
}
