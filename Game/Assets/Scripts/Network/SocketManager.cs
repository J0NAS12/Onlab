using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using TMPro;
using System;

public class SocketManager : MonoBehaviour
{
    public static WebSocket socket;
    public GameObject reconnect;
    public TextMeshProUGUI errormessage;
    public GameObject connectedPanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
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
        try{
            socket.Connect();
        }finally{
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
    }


    public void Connect(){

        if (GameValues.socket == null)
        {
            errormessage.text = "Connecting...";
            Debug.Log("Connecting to: " + "ws://" + GameValues.serverip);
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
                                GameValues.roomPlayers[playerData.index] = playerData;
                            }
                            break;
                        case "bullet":
                            var bulletData = JsonUtility.FromJson<BulletData>(e.Data);
                            if(bulletData.shooter.id != GameValues.me.id){
                                GameValues.bulletList.Add(bulletData);
                            }
                            break;
                        case "createGame":
                            var roomData = JsonUtility.FromJson<GameData>(e.Data);
                            GameValues.me.roomID = roomData.roomID;
                            GameValues.roomPlayers = (roomData.players);
                            GameValues.playersChanged = true;
                            break;
                        case "getRooms":
                            var roomsData = JsonUtility.FromJson<GameList>(e.Data);
                            GameValues.rooms = roomsData.rooms;
                            GameValues.listHasChanged = true;
                            break;
                        case "updateGame":
                            var updated = JsonUtility.FromJson<GameData>(e.Data);
                            GameValues.roomPlayers = updated.players;
                            GameValues.me.index = GameValues.roomPlayers.IndexOf(GameValues.roomPlayers.Find(x=>x.id == GameValues.me.id));
                            GameValues.playersChanged = true;
                            break;
                        case "id":
                            GameValues.me.id = (string)jsonObj["id"];
                            break;
                        case "startGame":
                            for(var i=0;i<GameValues.roomPlayers.Count; i++){
                                GameValues.roomPlayers[i].alive = true;
                            }
                            GameValues.spidersLeft = GameValues.roomPlayers.Count;
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
                GameValues.socket = null;
            };
            Reconnect();

        }
    }



}