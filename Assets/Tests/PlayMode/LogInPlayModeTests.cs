using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.TestTools;
using UnityEditor;

public class LogInPlayModeTests
{
    private GameObject logInObj;
    private GameObject mainMenuObj;
    private GameObject gameSessionObj;
    private UIDocument logInDoc;
    private LogInHandler handler;
    private PanelSettings panelSettings;

    private const BindingFlags NonPublic = BindingFlags.NonPublic | BindingFlags.Instance;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/LogIn.uxml");
        Assert.IsNotNull(visualTree, "No se encontró Assets/UI/LogIn.uxml");

        panelSettings = ScriptableObject.CreateInstance<PanelSettings>();

        logInObj = new GameObject("LogInTest");
        logInDoc = logInObj.AddComponent<UIDocument>();
        logInDoc.panelSettings = panelSettings;
        logInDoc.visualTreeAsset = visualTree;

        mainMenuObj = new GameObject("MainMenuDoc");
        var mainMenuDoc = mainMenuObj.AddComponent<UIDocument>();
        mainMenuDoc.panelSettings = panelSettings;

        gameSessionObj = new GameObject("GameSession");
        gameSessionObj.AddComponent<GameSession>();

        yield return null;

        handler = logInObj.AddComponent<LogInHandler>();
        typeof(LogInHandler).GetField("mainMenu", NonPublic).SetValue(handler, mainMenuDoc);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (logInObj != null) Object.DestroyImmediate(logInObj);
        if (mainMenuObj != null) Object.DestroyImmediate(mainMenuObj);
        if (gameSessionObj != null) Object.DestroyImmediate(gameSessionObj);
        if (panelSettings != null) Object.DestroyImmediate(panelSettings);
        yield return null;
    }

    private void FillLoginForm(VisualElement root, string email, string password)
    {
        root.Q<TextField>("EmailField").value = email;
        root.Q<TextField>("PasswordField").value = password;
    }

    private void ClickIniciar()
    {
        typeof(LogInHandler)
            .GetMethod("ConfirmCredentials", NonPublic)
            .Invoke(handler, null);
    }


    [UnityTest]
    public IEnumerator LogIn_CamposVacios_NoHaceLogin()
    {
        FillLoginForm(logInDoc.rootVisualElement, "", "");

        ClickIniciar();
        yield return null;

        Assert.AreEqual(0, GameSession.Instance.userId,
            "userId no debería cambiar si los campos están vacíos");
    }

    [UnityTest]
    public IEnumerator LogIn_SoloEmailVacio_NoHaceLogin()
    {
        FillLoginForm(logInDoc.rootVisualElement, "", "Pass123");

        ClickIniciar();
        yield return null;

        Assert.AreEqual(0, GameSession.Instance.userId,
            "userId no debería cambiar si el email está vacío");
    }

    [UnityTest]
    public IEnumerator LogIn_SoloPasswordVacia_NoHaceLogin()
    {
        FillLoginForm(logInDoc.rootVisualElement, "test@mail.com", "");

        ClickIniciar();
        yield return null;

        Assert.AreEqual(0, GameSession.Instance.userId,
            "userId no debería cambiar si el password está vacío");
    }

    // API
    [UnityTest]
    public IEnumerator LogIn_CredencialesInvalidas_NoHaceLogin()
    {
        FillLoginForm(logInDoc.rootVisualElement, "noexiste@fake.com", "wrongpassword123");

        ClickIniciar();
        yield return new WaitForSeconds(6f);

        Assert.AreEqual(0, GameSession.Instance.userId,
            "userId no debería cambiar con credenciales inválidas");
    }

    [UnityTest]
    public IEnumerator LogIn_ConCredencialesReales_AsignaUserId()
    {
        // Suprimir errores de SceneManager.LoadScene en contexto de test
        LogAssert.ignoreFailingMessages = true;

        // IMPORTANTE: reemplaza con credenciales de un usuario que YA exista en la BD
        FillLoginForm(logInDoc.rootVisualElement, "test_unity2@correo.com", "TestPass123");

        ClickIniciar();
        yield return new WaitForSeconds(6f);

        Assert.Greater(GameSession.Instance.userId, 0,
            "userId debería ser mayor a 0 tras un login exitoso");
    }
}
