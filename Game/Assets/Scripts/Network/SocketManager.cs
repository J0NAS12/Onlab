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
    public GameObject player;
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
                        case "createLobby":
                            var lobbyData = JsonUtility.FromJson<LobbyData>(e.Data);
                            GameValues.me.lobbyID = lobbyData.lobbyID;
                            GameValues.lobbyPlayers = (lobbyData.players);
                            GameValues.playersChanged = true;
                            break;
                        case "getLobbies":
                            var lobbiesData = JsonUtility.FromJson<LobbyList>(e.Data);
                            GameValues.lobbies = lobbiesData.lobbies;
                            GameValues.listHasChanged = true;
                            break;
                        case "updateLobby":
                            var updated = JsonUtility.FromJson<LobbyData>(e.Data);
                            GameValues.lobbyPlayers = updated.players;
                            GameValues.me.index = GameValues.lobbyPlayers.IndexOf(GameValues.lobbyPlayers.Find(x=>x.id == GameValues.me.id));
                            GameValues.playersChanged = true;
                            break;
                        case "id":
                            GameValues.me.id = (string)jsonObj["id"];
                            break;
                        case "startGame":
                            var data = JsonUtility.FromJson<LobbyData>(e.Data);
                            GameValues.maze = data.maze;
                            Debug.Log("start game");
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

        //If player is correctly configured, begin sending player data to server
        if (player != null && playerData.id != "")
        {
            //Grab player current position and rotation data
            playerData.xPos = player.transform.position.x;
            playerData.yRot = player.transform.rotation.y;
            playerData.zPos = player.transform.position.z;

            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            //Debug.Log(timestamp);
            playerData.timestamp = timestamp;

            string playerDataJSON = JsonUtility.ToJson(playerData);
            socket.Send(playerDataJSON);
        }
    }

    private void OnDestroy()
    {
        //Close socket when exiting application
        //socket.Close();
    }



}