using System;
using System.Collections.Generic;

[Serializable]
public struct GameData
{
    public string method;
    public string lobbyID;
    public string lobbyName;
    public string ID;
    public List<PlayerData> players;

    public MazeData maze;

}