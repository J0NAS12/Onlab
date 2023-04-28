using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUpdate : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI playersList;
    public TextMeshProUGUI lobbyName;
    void Start()
    {
        if(GameValues.me.lobbyID == null){
            var lobbyData = new LobbyData{
                method = "createLobby",
                ID = GameValues.me.id,
                lobbyName = GameValues.me.lobbyName,
                players = new List<PlayerData>()
            };
            lobbyData.players.Add(GameValues.me);
            GameValues.socket.Send(JsonUtility.ToJson(lobbyData));
        }
    }

    // Update is called once per frame
    void Update()
    {
        lobbyName.text = GameValues.me.lobbyName;
        string list = "Players:\n";
        foreach(var p in GameValues.lobbyPlayers){
            list += p.name + "\n";
        }
        playersList.text = list;
    }

    public void StartGame(){
        SceneManager.LoadScene("Game");
    }

    public void LeaveLobby(){
        var lobbyLeave = new LobbyData{
            method = "leaveLobby",
            ID = GameValues.me.id,
            lobbyID = GameValues.me.lobbyID,
            lobbyName = GameValues.me.lobbyName
        };
        GameValues.socket.Send(JsonUtility.ToJson(lobbyLeave));
        GameValues.me.lobbyName = null;
        GameValues.me.lobbyID = null;
        SceneManager.LoadScene("Menu");
    }

}
