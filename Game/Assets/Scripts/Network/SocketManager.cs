using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour
{
    public static WebSocket socket;
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.transform.position);
        if (socket == null || socket.ReadyState == WebSocketState.Closed)
        {
            Connect();
            return;
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        //socket.Close();
    }


    private void Connect(){
        if (GameValues.socket == null)
        {
            socket = new WebSocket("ws://152.66.181.88:8080");
            socket.Connect();
            GameValues.socket = socket;

            //WebSocket onMessage function
            socket.OnMessage += (sender, e) =>
            {

                if (e.IsText)
                {
                    JObject jsonObj = JObject.Parse(e.Data);
                    switch (((string)jsonObj["method"]))
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
        }
    }



}