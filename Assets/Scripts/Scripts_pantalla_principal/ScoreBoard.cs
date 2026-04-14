using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class ScoreBoard : MonoBehaviour
{
    [System.Serializable]
    public struct TopJugadores
    {
        public Jugador[] top_players;
    }

    [System.Serializable]
    public struct Jugador
    {
        public string nombre_usuario;
        public int score_global;
    }

    [SerializeField] TextMeshProUGUI text;


    private void Start()
    {
     text.text = "Getting top players...";
     CallForScoreBoard();   
    }

    private void CallForScoreBoard()
    {
        StartCoroutine(GetTopPlayers());
    }

    private IEnumerator GetTopPlayers()
    {

        string url = "http://localhost:3000/get_scoreboard";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {

                string textoToDisplay = "<mspace=0.55em>"; 
                textoToDisplay += "Jugador".PadRight(18) + "Score\n\n";


                string textoPlano = www.downloadHandler.text;

                TopJugadores topJugadores = JsonUtility.FromJson<TopJugadores>(textoPlano);

                if (topJugadores.top_players != null)
                {
                    for (int i = 0; i < topJugadores.top_players.Length; i++)
                    {
                        string name = topJugadores.top_players[i].nombre_usuario.PadRight(18);
                        string score = topJugadores.top_players[i].score_global.ToString();
                        textoToDisplay += name + score + "\n";
                    }
                    textoToDisplay += "</mspace>";
                    text.text = textoToDisplay;
                } else
                {
                    Debug.LogError("lol");
                }
                
            }
            else
            {
                Debug.LogError("Network error: " + www.error);
            }
        }
    }
}
