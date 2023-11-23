using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using TMPro;

public class SocketManager : MonoBehaviour
{
    public static WebSocket socket;
    public PlayerData playerData;
    public GameObject reconnect;
    public TextMeshProUGUI errormessage;
    public GameObject connectedPanel;

    // Start is called before the first frame update
    void Start()
    {
        reconnect.SetActive(false);
        errormessage.text = "";
        connectedPanel.SetActive(true);
        Connect();
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        //socket.Close();
    }

    public void Reconnect(){
        socket.Connect();
        if(socket.IsAlive){
            Clock.startSync();
            reconnect.SetActive(false);
            errormessage.text = "";
            connectedPanel.SetActive(true);
        }
        else{
            GameValues.socket = null;
            reconnect.SetActive(true);
            Debug.Log("Finished");
            errormessage.text = "Couldn't connect to the server.";
            connectedPanel.SetActive(false);
        }
    }


    public void Connect(){

        if (GameValues.socket == null)
        {
            errormessage.text = "Connecting...";
            Debug.Log("Connecting");
            socket = new WebSocket("ws://" + GameValues.serverip);
            GameValues.socket = socket;

            //WebSocket onMessage function
            socket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    JObject jsonObj = JObject.Parse(e.Data);
                    switch ((string)jsonObj["method"])
                    {
                        case "hit":
                            var hitData = JsonUtility.FromJson<HitData>(e.Data);
                            GameValues.hitList.Add(hitData);
                            break;
                        case "game":
                            var playerData = JsonUtility.FromJson<PlayerData>(e.Data);
                            if(playerData.id != GameValues.me.id){
                                GameValues.lobbyPlayers[playerData.index] = playerData;
                            }
                            break;
                        case "bullet":
                            var bulletData = JsonUtility.FromJson<BulletData>(e.Data);
                            if(bulletData.shooter.id != GameValues.me.id){
                                GameValues.bulletList.Add(bulletData);
                            }
                            break;
                        case "createGame":
                            var lobbyData = JsonUtility.FromJson<GameData>(e.Data);
                            GameValues.me.lobbyID = lobbyData.lobbyID;
                            GameValues.lobbyPlayers = (lobbyData.players);
                            GameValues.playersChanged = true;
                            break;
                        case "getGames":
                            var lobbiesData = JsonUtility.FromJson<GameList>(e.Data);
                            GameValues.lobbies = lobbiesData.lobbies;
                            GameValues.listHasChanged = true;
                            break;
                        case "updateGame":
                            var updated = JsonUtility.FromJson<GameData>(e.Data);
                            GameValues.lobbyPlayers = updated.players;
                            GameValues.me.index = GameValues.lobbyPlayers.IndexOf(GameValues.lobbyPlayers.Find(x=>x.id == GameValues.me.id));
                            GameValues.playersChanged = true;
                            break;
                        case "id":
                            GameValues.me.id = (string)jsonObj["id"];
                            break;
                        case "startGame":
                            for(var i=0;i<GameValues.lobbyPlayers.Count; i++){
                                GameValues.lobbyPlayers[i].alive = true;
                            }
                            GameValues.spidersLeft = GameValues.lobbyPlayers.Count;
                            var data = JsonUtility.FromJson<GameData>(e.Data);
                            GameValues.maze = data.maze;
                            GameValues.startGame = true;
                            break;
                        case "clock":
                            Clock.synchronize((double)jsonObj["timestamp"]);
                            break;
                        default:
                            Debug.Log("no method: " + (string)jsonObj["method"]);
                            break;
                    }
                }
            };

            //If server connection closes (not client originated)
            socket.OnClose += (sender, e) =>
            {

                Debug.Log(e.Code);
                Debug.Log(e.Reason);
                Debug.Log("Connection Closed!");

            };
            Reconnect();

        }
    }



}