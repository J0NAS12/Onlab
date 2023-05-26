using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class GameValues : MonoBehaviour
{
    public static bool startGame = false;
    public static bool listHasChanged = false;
    public static bool playersChanged = false;
    public static WebSocket socket;
    public static int spidersLeft = 0;
    public static PlayerData me = new PlayerData{
        lobbyID = null,
        id = "id",
        name = null,
        lobbyName = null,
        kills = 0,
        wins = 0,
        alive = true
    };
    public static List<BulletData> bulletList = new List<BulletData>();
    public static List<HitData> hitList = new List<HitData>();

    public static MazeData maze;
    public static List<GameData> lobbies = new List<GameData>();

    public static List<PlayerData> lobbyPlayers = new List<PlayerData>();


    public void ChangeName(string value){
        me.name = value;
    }

    public void ChangeLobbyName(string value){
        me.lobbyName = value;
    }

}
