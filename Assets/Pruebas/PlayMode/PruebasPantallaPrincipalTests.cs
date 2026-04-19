using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class PruebasPantallaPrincipalTests
{
    private GameObject gameSessionObject;
    private GameObject playerObject;
    private GameObject playButtonObject;
    private GameObject islandObject;
    private GameObject difficultySelectorObject;

    private GameSession gameSession;
    private IslandTrigger islandTrigger;
    private DifficultySelector difficultySelector;
    private Collider2D playerCollider;

    [SetUp]
    public void SetUp()
    {
        ResetGameSessionSingleton();

        gameSessionObject = new GameObject("GameSession");
        gameSession = gameSessionObject.AddComponent<GameSession>();

        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerCollider = playerObject.AddComponent<BoxCollider2D>();

        playButtonObject = new GameObject("PlayButton");
        playButtonObject.SetActive(false);

        islandObject = new GameObject("isla_suma");
        islandObject.AddComponent<BoxCollider2D>().isTrigger = true;
        islandTrigger = islandObject.AddComponent<IslandTrigger>();
        islandTrigger.playButtonUI = playButtonObject;

        difficultySelectorObject = new GameObject("DifficultySelector");
        difficultySelector = difficultySelectorObject.AddComponent<DifficultySelector>();
    }

    [Test]
    public void AlEntrarEnLaIsla_MuestraElBotonYRegistraLaIsla()
    {
        InvokePrivate(islandTrigger, "OnTriggerEnter2D", playerCollider);

        Assert.IsTrue(playButtonObject.activeSelf, "El boton de jugar no se activo al entrar en la isla");
        Assert.AreEqual("isla_suma", gameSession.IslaActual, "La isla no se registro en GameSession");
    }

    [Test]
    public void AlSalirDeLaIsla_OcultaElBoton()
    {
        playButtonObject.SetActive(true);
        InvokePrivate(islandTrigger, "OnTriggerExit2D", playerCollider);

        Assert.IsFalse(playButtonObject.activeSelf, "El boton de jugar no se oculto al salir de la isla");
    }

    [Test]
    public void AlElegirFacil_ActualizaLaDificultad()
    {
        difficultySelector.OnFacilSelected();

        Assert.AreEqual(Dificultad.Facil, gameSession.DificultadActual, "La dificultad no cambio a Facil");
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(difficultySelectorObject);
        UnityEngine.Object.DestroyImmediate(islandObject);
        UnityEngine.Object.DestroyImmediate(playButtonObject);
        UnityEngine.Object.DestroyImmediate(playerObject);
        UnityEngine.Object.DestroyImmediate(gameSessionObject);
        ResetGameSessionSingleton();
    }

    private static void ResetGameSessionSingleton()
    {
        FieldInfo instanceField = typeof(GameSession).GetField("<Instance>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
        if (instanceField != null)
        {
            instanceField.SetValue(null, null);
        }
    }

    private static void InvokePrivate(object target, string methodName, params object[] arguments)
    {
        MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        Assert.IsNotNull(method, $"No se encontro el metodo {methodName} en {target.GetType().Name}");
        method.Invoke(target, arguments);
    }
}