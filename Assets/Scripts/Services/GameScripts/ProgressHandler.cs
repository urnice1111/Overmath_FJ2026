using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using NUnit.Framework.Constraints;

public class ProgressHandler : MonoBehaviour
{
    [System.Serializable]
    public class IntentoPregunta
    {
        public int id_pregunta;
        public string respuesta_usuario;
        public bool es_correcto;
        public float tiempo_respuesta_seg;
    }

    [System.Serializable]
    public class PartidaData
    {
        public int id_cuenta;       // viene de GameSession.Instance.userId
        public int score_max;        // PuntajedePregunta.Instance.PuntosActuales
        public float tiempo_seg;     // TiempoJuego.Instance.TiempoJugado
        public string nombreIsla;
        public string dificultad;
        public string resultado; // "victoria" o "derrota"
        public List<IntentoPregunta> intentos;
    }
    
    private string apiUrl = "https://udqzin2siulhcshfje2amhkiey0pkadb.lambda-url.us-east-1.on.aws//save_progress";

    public void GuardarPartida(PartidaData partida)
    {
        string jsonData = JsonUtility.ToJson(partida);
        Debug.Log("JSON enviado: " + jsonData);
        StartCoroutine(PostRequest(apiUrl, jsonData));
    }

    IEnumerator PostRequest(string url, string json)
    {
        using UnityWebRequest www = UnityWebRequest.Post(url, json, "application/json");


        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Progreso guardado correctamente: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al guardar progreso: " + www.error);
        }
    }
}
