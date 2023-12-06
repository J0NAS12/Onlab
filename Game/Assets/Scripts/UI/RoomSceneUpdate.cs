using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomSceneUpdate : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI playersList;
    public TextMeshProUGUI roomName;

    public Button startButton;
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
        gameData.SendToServer();
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
            roomName.text = GameValues.me.roomName;
            string list = $"{"Players",-15}\tKills\tWins\n";
            foreach(var p in GameValues.roomPlayers){
                list += $"{p.name,-15}\t{p.kills,-5}\t{p.wins,-4}\n";
            }
            playersList.text = list;
            if(GameValues.roomPlayers.Count > 1)
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }
    }

    public void StartGame(){
        SceneManager.LoadScene("Game");
    }

    public void LeaveRoom(){
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
