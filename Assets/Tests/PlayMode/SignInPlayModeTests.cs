using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.TestTools;
using UnityEditor;

public class SignInPlayModeTests
{
    private GameObject signInObj;
    private GameObject logInObj;
    private GameObject mainMenuObj;
    private UIDocument signInDoc;
    private SignInHandler handler;
    private PanelSettings panelSettings;

    private const BindingFlags NonPublic = BindingFlags.NonPublic | BindingFlags.Instance;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/SignIn.uxml");
        Assert.IsNotNull(visualTree, "No se encontró Assets/UI/SignIn.uxml");

        panelSettings = ScriptableObject.CreateInstance<PanelSettings>();

        signInObj = new GameObject("SignInTest");
        signInDoc = signInObj.AddComponent<UIDocument>();
        signInDoc.panelSettings = panelSettings;
        signInDoc.visualTreeAsset = visualTree;

        logInObj = new GameObject("LogInDoc");
        var logInDoc = logInObj.AddComponent<UIDocument>();
        logInDoc.panelSettings = panelSettings;

        mainMenuObj = new GameObject("MainMenuDoc");
        var mainMenuDoc = mainMenuObj.AddComponent<UIDocument>();
        mainMenuDoc.panelSettings = panelSettings;

        yield return null;

        handler = signInObj.AddComponent<SignInHandler>();
        typeof(SignInHandler).GetField("logInDocument", NonPublic).SetValue(handler, logInDoc);
        typeof(SignInHandler).GetField("mainMenuDocument", NonPublic).SetValue(handler, mainMenuDoc);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (signInObj != null) Object.DestroyImmediate(signInObj);
        if (logInObj != null) Object.DestroyImmediate(logInObj);
        if (mainMenuObj != null) Object.DestroyImmediate(mainMenuObj);
        if (panelSettings != null) Object.DestroyImmediate(panelSettings);
        yield return null;
    }

    private void FillForm(VisualElement root, string email, string username,
        string password, string confirmPassword, string firstName, string lastName,
        int day, int month, int year)
    {
        root.Q<TextField>("EmailField").value = email;
        root.Q<TextField>("UsernameField").value = username;
        root.Q<TextField>("PasswordField").value = password;
        root.Q<TextField>("ConfirmPasswordField").value = confirmPassword;
        root.Q<TextField>("FirstNameField").value = firstName;
        root.Q<TextField>("LastNameField").value = lastName;
        root.Q<IntegerField>("DayField").value = day;
        root.Q<IntegerField>("MonthField").value = month;
        root.Q<IntegerField>("YearField").value = year;
    }

    private void ClickRegistrar()
    {
        typeof(SignInHandler)
            .GetMethod("ConfirmCredentials", NonPublic)
            .Invoke(handler, null);
    }

    private Label GetResponseLabel()
    {
        return signInDoc.rootVisualElement.Q<Label>("Response");
    }

    // Test sin red, de validacion nomas

    [UnityTest]
    public IEnumerator SignIn_CamposVacios_MuestraError()
    {
        var root = signInDoc.rootVisualElement;
        FillForm(root, "", "", "", "", "", "", 1, 1, 1990);

        ClickRegistrar();
        yield return null;

        Assert.AreEqual("Please enter both email and password.", GetResponseLabel().text);
    }

    [UnityTest]
    public IEnumerator SignIn_SoloEmailVacio_MuestraError()
    {
        var root = signInDoc.rootVisualElement;
        FillForm(root, "", "user1", "Pass123", "Pass123", "Juan", "Perez", 15, 6, 2000);

        ClickRegistrar();
        yield return null;

        Assert.AreEqual("Please enter both email and password.", GetResponseLabel().text);
    }

    [UnityTest]
    public IEnumerator SignIn_SoloPasswordVacia_MuestraError()
    {
        var root = signInDoc.rootVisualElement;
        FillForm(root, "test@mail.com", "user1", "", "", "Juan", "Perez", 15, 6, 2000);

        ClickRegistrar();
        yield return null;

        Assert.AreEqual("Please enter both email and password.", GetResponseLabel().text);
    }

    [UnityTest]
    public IEnumerator SignIn_PasswordNoCoincide_MuestraError()
    {
        var root = signInDoc.rootVisualElement;
        FillForm(root, "test@mail.com", "user1", "Pass123", "OtraPass456",
                 "Juan", "Perez", 15, 6, 2000);

        ClickRegistrar();
        yield return null;

        Assert.AreEqual("La contraseña no coincide", GetResponseLabel().text);
    }

    // Llama a API real
    [UnityTest]
    public IEnumerator SignIn_ConCredencialesReales_RetornaExito()
    {
        var root = signInDoc.rootVisualElement;

        FillForm(root,
            "test_unity5@correo.com",
            "test_user_unity5",
            "TestPass123",
            "TestPass123",
            "NombrePrueba",
            "ApellidoPrueba",
            15, 6, 2000);

        ClickRegistrar();

        yield return new WaitForSeconds(6f);

        Assert.IsFalse(signInObj.activeSelf, "El GameObject debería desactivarse tras un registro exitoso");
        Assert.IsTrue(logInObj.activeSelf, "El documento de LogIn debería activarse tras un registro exitoso");
    }
}
