using System;
using System.Collections.Generic;

[Serializable]
public struct GameData
{
    public string method;
    public string roomID;
    public string roomName;
    public string ID;
    public List<PlayerData> players;
    public MazeData maze;

}