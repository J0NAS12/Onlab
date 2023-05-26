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
        if(GameValues.me.lobbyID == null){
            var gameData = new GameData{
                method = "createGame",
                ID = GameValues.me.id,
                lobbyName = GameValues.me.lobbyName,
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
            lobbyName.text = GameValues.me.lobbyName;
            string list = "Players:\tKills\tWins\n";
            foreach(var p in GameValues.lobbyPlayers){
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
        var lobbyLeave = new GameData{
            method = "leaveGame",
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
