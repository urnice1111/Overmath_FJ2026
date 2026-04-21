using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameFlowPlayModeTests
{
    private GameObject puntajeObj;
    private GameObject tiempoObj;
    private GameObject preguntaObj;
    private GameObject gameFlowObj;
    private GameObject loseCanvasObj;
    private GameObject winCanvasObj;
    private GameFlowManager gameFlowManager;
    private LosePopupUI losePopupUI;

    private const BindingFlags NonPublic = BindingFlags.NonPublic | BindingFlags.Instance;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Crear instancias de singletons
        puntajeObj = new GameObject("PuntajedePregunta");
        puntajeObj.AddComponent<PuntajedePregunta>();

        tiempoObj = new GameObject("TiempoJuego");
        tiempoObj.AddComponent<TiempoJuego>();

        preguntaObj = new GameObject("PreguntaManager");
        preguntaObj.AddComponent<PreguntaManager>();

        // Crear canvases
        loseCanvasObj = new GameObject("LoseCanvas");
        losePopupUI = loseCanvasObj.AddComponent<LosePopupUI>();

        winCanvasObj = new GameObject("WinCanvas");

        // Crear GameFlowManager y asignar referencias
        gameFlowObj = new GameObject("GameFlowManager");
        gameFlowManager = gameFlowObj.AddComponent<GameFlowManager>();
        typeof(GameFlowManager).GetField("LoseCanvas", NonPublic).SetValue(gameFlowManager, loseCanvasObj);
        typeof(GameFlowManager).GetField("WinCanvas", NonPublic).SetValue(gameFlowManager, winCanvasObj);
        // resultadosUI se deja null ya que no se usa en derrota

        yield return null; // Esperar a que Start se ejecute
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (puntajeObj != null) Object.DestroyImmediate(puntajeObj);
        if (tiempoObj != null) Object.DestroyImmediate(tiempoObj);
        if (preguntaObj != null) Object.DestroyImmediate(preguntaObj);
        if (gameFlowObj != null) Object.DestroyImmediate(gameFlowObj);
        if (loseCanvasObj != null) Object.DestroyImmediate(loseCanvasObj);
        if (winCanvasObj != null) Object.DestroyImmediate(winCanvasObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TiempoAgotado_PuntajeInsuficiente_ActivaLosePopup()
    {
        // Configurar puntaje insuficiente
        typeof(PuntajedePregunta).GetProperty("PuntosActuales", NonPublic).SetValue(PuntajedePregunta.Instance, 50);

        // Agotar tiempo
        TiempoJuego.Instance.AjustarTiempo(-1000f);

        yield return null; // Esperar a Update

        Assert.IsTrue(loseCanvasObj.activeSelf, "LoseCanvas debería activarse cuando tiempo se agota y puntaje es insuficiente");
    }

    [UnityTest]
    public IEnumerator VidaAgotada_PuntajeInsuficiente_ActivaLosePopup()
    {
        // Configurar puntaje insuficiente
        typeof(PuntajedePregunta).GetProperty("PuntosActuales", NonPublic).SetValue(PuntajedePregunta.Instance, 50);

        // Agotar vida/tiempo
        TiempoJuego.Instance.AjustarTiempo(-1000f);

        yield return null; // Esperar a Update

        Assert.IsTrue(loseCanvasObj.activeSelf, "LoseCanvas debería activarse cuando vida/tiempo se agota y puntaje es insuficiente");
    }

    [UnityTest]
    public IEnumerator TiempoAgotado_PuntajeSuficiente_NoActivaLosePopup()
    {
        // Configurar puntaje suficiente
        typeof(PuntajedePregunta).GetProperty("PuntosActuales", NonPublic).SetValue(PuntajedePregunta.Instance, 100);

        // Agotar tiempo
        TiempoJuego.Instance.AjustarTiempo(-1000f);

        yield return null; // Esperar a Update

        Assert.IsFalse(loseCanvasObj.activeSelf, "LoseCanvas NO debería activarse cuando tiempo se agota pero puntaje es suficiente");
    }
}</content>
<parameter name="filePath">C:\Users\regih\OneDrive\Escritorio\Overmath_FJ2026\Assets\Tests\PlayMode\GameFlowPlayModeTests.cs
