using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

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
        public int id_jugador;       // viene de GameSession.Instance.userId
        public int score_max;        // PuntajedePregunta.Instance.PuntosActuales
        public float tiempo_seg;     // TiempoJuego.Instance.TiempoJugado
        public string fecha_hora;    // System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        public int nivel;         // DifficultySelector.Instance.NivelActual
        public List<IntentoPregunta> intentos;
    }
    
    private string apiUrl = "https://udqzin2siulhcshfje2amhkiey0pkadb.lambda-url.us-east-1.on.aws/save_progress";

    public void GuardarPartida(PartidaData partida)
    {
        string jsonData = JsonUtility.ToJson(partida);
        Debug.Log("JSON enviado: " + jsonData);
        StartCoroutine(PostRequest(apiUrl, jsonData));
    }

    IEnumerator PostRequest(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Progreso guardado correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al guardar progreso: " + request.error);
        }
    }
}
