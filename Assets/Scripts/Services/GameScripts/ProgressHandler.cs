using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProgressHandler : MonoBehaviour
{
    private string apiUrl = "https://tuservidor.com/save_progress";

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

    public void GuardarPartida(List<IntentoPregunta> intentos)
    {
        PartidaData partida = new PartidaData
        {
            id_jugador = GameSession.Instance.userId,
            score_max = PuntajedePregunta.Instance.PuntosActuales,
            tiempo_seg = TiempoJuego.Instance.TiempoJugado,
            fecha_hora = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            nivel = GameSession.Instance.GetNivel(),
            intentos = intentos
        };

        string jsonData = JsonUtility.ToJson(partida);
        StartCoroutine(PostPartida(jsonData));
    }

    private IEnumerator PostPartida(string jsonData)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(
                   "https://udqzin2siulhcshfje2amhkiey0pkadb.lambda-url.us-east-1.on.aws/save_progress",
                   "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

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

}
