//Tests - PantallaPrincipalTests.cs
using NUnit.Framework;
using UnityEngine;

public class PantallaPrincipalTests
{
    private GameObject gameSessionObject;
    private GameSession gameSession;

    [SetUp]
    public void SetUp()
    {
        gameSessionObject = new GameObject("GameSession");
        gameSession = gameSessionObject.AddComponent<GameSession>();
    }

    [Test]
    public void GameSession_GuardaIslaYDificultad()
    {
        gameSession.SetIsla("isla_suma");
        gameSession.SetDificultad(Dificultad.Facil);

        Assert.AreEqual("isla_suma", GameSession.Instance.IslaActual);
        Assert.AreEqual(Dificultad.Facil, GameSession.Instance.DificultadActual);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameSessionObject);
    }
}