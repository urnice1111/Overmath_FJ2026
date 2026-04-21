/*using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class GameFlowPlayModeTests
{
    private GameObject puntajeObj;
    private GameObject tiempoObj;
    private GameObject preguntaObj;
    private GameObject gameFlowObj;
    private GameObject loseCanvasObj;
    private GameObject winCanvasObj;
    private GameObject gameSessionObj;
    private GameObject resultadosObj;
    private GameFlowManager gameFlowManager;
    private LosePopupUI losePopupUI;
    private resultadosUI resultadosUIComp;

    private const BindingFlags NonPublic = BindingFlags.NonPublic | BindingFlags.Instance;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Crear instancias de singletons
        gameSessionObj = new GameObject("GameSession");
        gameSessionObj.AddComponent<GameSession>();
        GameSession.Instance.IsTutorial = true;

        puntajeObj = new GameObject("PuntajedePregunta");
        puntajeObj.AddComponent<PuntajedePregunta>();

        tiempoObj = new GameObject("TiempoJuego");
        tiempoObj.AddComponent<TiempoJuego>();

        preguntaObj = new GameObject("PreguntaManager");
        preguntaObj.AddComponent<PreguntaManager>();

        // Crear canvases
        loseCanvasObj = new GameObject("LoseCanvas");
        loseCanvasObj.AddComponent<RectTransform>();
        losePopupUI = loseCanvasObj.AddComponent<LosePopupUI>();
        typeof(LosePopupUI).GetField("popupContent", NonPublic).SetValue(losePopupUI, loseCanvasObj.GetComponent<RectTransform>());

        winCanvasObj = new GameObject("WinCanvas");

        // Crear GameFlowManager y asignar referencias
        gameFlowObj = new GameObject("GameFlowManager");
        gameFlowManager = gameFlowObj.AddComponent<GameFlowManager>();
        typeof(GameFlowManager).GetField("LoseCanvas", NonPublic).SetValue(gameFlowManager, loseCanvasObj);
        typeof(GameFlowManager).GetField("WinCanvas", NonPublic).SetValue(gameFlowManager, winCanvasObj);

        resultadosObj = new GameObject("ResultadosUI");
        resultadosUIComp = resultadosObj.AddComponent<resultadosUI>();
        // Asignar TMP dummy
        var puntajeTMP = resultadosObj.AddComponent<TextMeshProUGUI>();
        var tiempoTMP = resultadosObj.AddComponent<TextMeshProUGUI>();
        var contestadasTMP = resultadosObj.AddComponent<TextMeshProUGUI>();
        var correctasTMP = resultadosObj.AddComponent<TextMeshProUGUI>();
        typeof(resultadosUI).GetField("puntaje", NonPublic).SetValue(resultadosUIComp, puntajeTMP);
        typeof(resultadosUI).GetField("tiempo", NonPublic).SetValue(resultadosUIComp, tiempoTMP);
        typeof(resultadosUI).GetField("contestadas", NonPublic).SetValue(resultadosUIComp, contestadasTMP);
        typeof(resultadosUI).GetField("correctas", NonPublic).SetValue(resultadosUIComp, correctasTMP);
        typeof(GameFlowManager).GetField("resultadosUI", NonPublic).SetValue(gameFlowManager, resultadosUIComp);

        yield return null; // Esperar a que Start se ejecute
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (gameSessionObj != null) Object.DestroyImmediate(gameSessionObj);
        if (puntajeObj != null) Object.DestroyImmediate(puntajeObj);
        if (tiempoObj != null) Object.DestroyImmediate(tiempoObj);
        if (preguntaObj != null) Object.DestroyImmediate(preguntaObj);
        if (gameFlowObj != null) Object.DestroyImmediate(gameFlowObj);
        if (loseCanvasObj != null) Object.DestroyImmediate(loseCanvasObj);
        if (winCanvasObj != null) Object.DestroyImmediate(winCanvasObj);
        if (resultadosObj != null) Object.DestroyImmediate(resultadosObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TiempoAgotado_PuntajeInsuficiente_ActivaLosePopup()
    {
        // Configurar puntaje insuficiente
        PuntajedePregunta.Instance.ReiniciarPuntaje();
        typeof(PuntajedePregunta).GetMethod("SumarPuntos", NonPublic).Invoke(PuntajedePregunta.Instance, new object[] { 50 });

        // Agotar tiempo
        TiempoJuego.Instance.AjustarTiempo(-1000f);

        yield return null; // Esperar a Update

        Assert.IsTrue(loseCanvasObj.activeSelf, "LoseCanvas debería activarse cuando tiempo se agota y puntaje es insuficiente");
    }

    [UnityTest]
    public IEnumerator VidaAgotada_PuntajeInsuficiente_ActivaLosePopup()
    {
        // Configurar puntaje insuficiente
        PuntajedePregunta.Instance.ReiniciarPuntaje();
        typeof(PuntajedePregunta).GetMethod("SumarPuntos", NonPublic).Invoke(PuntajedePregunta.Instance, new object[] { 50 });

        // Agotar vida/tiempo
        TiempoJuego.Instance.AjustarTiempo(-1000f);

        yield return null; // Esperar a Update

        Assert.IsTrue(loseCanvasObj.activeSelf, "LoseCanvas debería activarse cuando vida/tiempo se agota y puntaje es insuficiente");
    }

    [UnityTest]
    public IEnumerator TiempoAgotado_PuntajeSuficiente_NoActivaLosePopup()
    {
        // Configurar puntaje suficiente
        PuntajedePregunta.Instance.ReiniciarPuntaje();
        typeof(PuntajedePregunta).GetMethod("SumarPuntos", NonPublic).Invoke(PuntajedePregunta.Instance, new object[] { 100 });

        // Agotar tiempo
        TiempoJuego.Instance.AjustarTiempo(-1000f);

        yield return null; // Esperar a Update

        Assert.IsFalse(loseCanvasObj.activeSelf, "LoseCanvas NO debería activarse cuando tiempo se agota pero puntaje es suficiente");
    }
}*/