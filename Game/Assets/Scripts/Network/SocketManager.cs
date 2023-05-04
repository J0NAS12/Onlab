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
        if (GameValues.socket == null)
        {
            socket = new WebSocket("ws://localhost:8080");
            socket.Connect();
            GameValues.socket = socket;

            //WebSocket onMessage function
            socket.OnMessage += (sender, e) =>
            {

                if (e.IsText)
                {
                    JObject jsonObj = JObject.Parse(e.Data);
                    Debug.Log("method:" + (string)jsonObj["method"]);
                    switch (((string)jsonObj["method"]))
                    {
                        case "game":

                            var playerData = JsonUtility.FromJson<PlayerData>(e.Data);
                            GameValues.lobbyPlayers[playerData.index] = playerData;
                            Debug.Log(GameValues.lobbyPlayers);
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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.transform.position);
        if (socket == null)
        {
            return;
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        //socket.Close();
    }



}