using System;
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

    private Component gameSession;
    private Component islandTrigger;
    private Component difficultySelector;
    private Collider2D playerCollider;

    [SetUp]
    public void SetUp()
    {
        ResetGameSessionSingleton();

        gameSessionObject = new GameObject("GameSession");
        gameSession = gameSessionObject.AddComponent(GetRuntimeType("GameSession"));

        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerCollider = playerObject.AddComponent<BoxCollider2D>();

        playButtonObject = new GameObject("PlayButton");
        playButtonObject.SetActive(false);

        islandObject = new GameObject("isla_suma");
        islandObject.AddComponent<BoxCollider2D>().isTrigger = true;
        islandTrigger = islandObject.AddComponent(GetRuntimeType("IslandTrigger"));
        SetField(islandTrigger, "playButtonUI", playButtonObject);

        difficultySelectorObject = new GameObject("DifficultySelector");
        difficultySelector = difficultySelectorObject.AddComponent(GetRuntimeType("DifficultySelector"));
    }

    [Test]
    public void AlEntrarEnLaIsla_MuestraElBotonYRegistraLaIsla()
    {
        InvokePrivate(islandTrigger, "OnTriggerEnter2D", playerCollider);

        Assert.IsTrue(playButtonObject.activeSelf, "El boton de jugar no se activo al entrar en la isla");
        Assert.AreEqual("isla_suma", GetPropertyValue(gameSession, "IslaActual")?.ToString(), "La isla no se registro en GameSession");
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
        InvokePrivate(difficultySelector, "OnFacilSelected");

        Assert.AreEqual("Facil", GetPropertyValue(gameSession, "DificultadActual")?.ToString(), "La dificultad no cambio a Facil");
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
        Type gameSessionType = GetRuntimeType("GameSession");
        FieldInfo instanceField = gameSessionType.GetField("<Instance>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
        if (instanceField != null)
        {
            instanceField.SetValue(null, null);
        }
    }

    private static Type GetRuntimeType(string typeName)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        Assert.Fail($"No se encontro el tipo runtime {typeName}.");
        return null;
    }

    private static object GetPropertyValue(object target, string propertyName)
    {
        Type targetType = target.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        PropertyInfo propertyInfo = targetType.GetProperty(propertyName, flags);
        Assert.IsNotNull(propertyInfo, $"No se encontro la propiedad {propertyName} en {targetType.Name}");
        return propertyInfo.GetValue(target);
    }

    private static void SetField(object target, string fieldName, object value)
    {
        Type targetType = target.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        FieldInfo fieldInfo = targetType.GetField(fieldName, flags);
        Assert.IsNotNull(fieldInfo, $"No se encontro el campo {fieldName} en {targetType.Name}");
        fieldInfo.SetValue(target, value);
    }

    private static void InvokePrivate(object target, string methodName, params object[] arguments)
    {
        Type targetType = target.GetType();
        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        MethodInfo method = targetType.GetMethod(methodName, flags);
        Assert.IsNotNull(method, $"No se encontro el metodo {methodName} en {targetType.Name}");
        method.Invoke(target, arguments);
    }
}