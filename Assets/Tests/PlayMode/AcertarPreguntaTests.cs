using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AcertarPreguntaTests
{
    private PreguntaManager preguntaManager;
    private GameSession gameSession;
    private GameObject createdGameSessionObject;
    private GameObject preguntaManagerObject;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        var existingSession = Object.FindAnyObjectByType<GameSession>();
        if (existingSession == null)
        {
            createdGameSessionObject = new GameObject("GameSession");
            gameSession = createdGameSessionObject.AddComponent<GameSession>();
        }
        else
        {
            gameSession = existingSession;
        }

        preguntaManagerObject = new GameObject("PreguntaManager");
        preguntaManager = preguntaManagerObject.AddComponent<PreguntaManager>();
        preguntaManager.CargarPreguntaTutorial(0);

        yield return null;

        Assert.IsNotNull(preguntaManager, "No se encontró PreguntaManager en el objeto de prueba.");
        Assert.IsFalse(string.IsNullOrEmpty(preguntaManager.PreguntaActual.problema), "PreguntaActual no se cargó correctamente en modo tutorial.");
    }

    [UnityTest]
    public IEnumerator AcertarPregunta_IncremnetaTotalCorrectas()
    {
        
        int initialCorrect = preguntaManager.TotalCorrectas;

        int initialAnswered = preguntaManager.TotalContestadas;

        int respuestaCorrecta = preguntaManager.PreguntaActual.respuesta_correcta;

        
        preguntaManager.RegistrarRespuesta(respuestaCorrecta);
        yield return null;

        Assert.AreEqual(initialCorrect + 1, preguntaManager.TotalCorrectas, 
            "TotalCorrectas no aumentó al acertar la pregunta.");

        // TODO 6: Verificar que TotalContestadas aumentó en 1
        Assert.AreEqual(initialAnswered + 1, preguntaManager.TotalContestadas,
            "TotalContestadas no aumentó.");
    }

    [UnityTest]
    public IEnumerator RespuestaIncorrecta_NoIncrementaTotalCorrectas()
    {
        
        int initialCorrect = preguntaManager.TotalCorrectas;

        int initialAnswered = preguntaManager.TotalContestadas;

        int respuestaIncorrecta = preguntaManager.PreguntaActual.respuesta_correcta + 1;

        preguntaManager.RegistrarRespuesta(respuestaIncorrecta);
        yield return null;

        Assert.AreEqual(initialCorrect, preguntaManager.TotalCorrectas,
            "TotalCorrectas cambió al responder incorrectamente.");

        Assert.AreEqual(initialAnswered + 1, preguntaManager.TotalContestadas,
            "TotalContestadas no se incrementó al responder incorrectamente.");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (createdGameSessionObject != null)
            Object.DestroyImmediate(createdGameSessionObject);

        if (preguntaManagerObject != null)
            Object.DestroyImmediate(preguntaManagerObject);

        yield return null;
    }
}

