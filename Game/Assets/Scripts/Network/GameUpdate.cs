using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUpdate : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI playersList;
    public TextMeshProUGUI lobbyName;
    void Start()
    {
        if(GameValues.me.roomID == null){
            var gameData = new GameData{
                method = "createRoom",
                ID = GameValues.me.id,
                roomName = GameValues.me.roomName,
                players = new List<PlayerData>()
        };
        gameData.players.Add(GameValues.me);
        GameValues.socket.Send(JsonUtility.ToJson(gameData));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameValues.startGame)
        {
            GameValues.startGame = false;
            SceneManager.LoadScene("Game");
        }
        if(GameValues.playersChanged){
            GameValues.playersChanged = false;
            lobbyName.text = GameValues.me.roomName;
            string list = "Players:\tKills\tWins\n";
            foreach(var p in GameValues.roomPlayers){
                list += p.name + "\t" + p.kills + "\t" + p.wins + "\n";
            }
            playersList.text = list;
        }
    }

    public void StartGame(){
        SceneManager.LoadScene("Game");
    }

    public void LeaveLobby(){
        GameValues.me.alive = true;
        GameValues.me.kills = 0;
        GameValues.me.wins = 0;
        var roomLeave = new GameData{
            method = "leaveRoom",
            ID = GameValues.me.id,
            roomID = GameValues.me.roomID,
            roomName = GameValues.me.roomName
        };
        GameValues.socket.Send(JsonUtility.ToJson(roomLeave));
        GameValues.me.roomName = null;
        GameValues.me.roomID = null;
        SceneManager.LoadScene("Menu");
    }

}
