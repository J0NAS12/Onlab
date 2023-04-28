using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class GameValues : MonoBehaviour
{
    public static int PlayerCounter = 2;

    public static bool listHasChanged = false;

    public static WebSocket socket;
    public static int spidersLeft = 2;
    public static PlayerData me = new PlayerData{
        lobbyID = null,
        id = "id",
        name = null,
        lobbyName = null
    };
 
    public static MazeData maze;
    public static List<LobbyData> lobbies = new List<LobbyData>();

    public static List<PlayerData> lobbyPlayers = new List<PlayerData>();

    public void ChangePlayerCounter(int value){
        PlayerCounter = value + 2;
    }

    public void ChangeName(string value){
        me.name = value;
    }

    public void ChangeLobbyName(string value){
        me.lobbyName = value;
    }

}
