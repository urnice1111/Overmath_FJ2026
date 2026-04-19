//Get to show n top players in the score board. Reference to a text and reload. 
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
        public int score_global;
        public string nombre_usuario;
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

        string url = "https://q623ldzsbzpk3j6nktpzcvqi7y0qrpsr.lambda-url.us-east-1.on.aws/get_scoreboard";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {

                string textoToDisplay = "<mspace=0.55em>"; 
                textoToDisplay += "Jugador".PadRight(18) + "Score\n\n";


                string textoPlano = www.downloadHandler.text;

                textoPlano = "{\"top_players\":" + textoPlano + "}";
                
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
