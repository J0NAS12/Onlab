using System;
using System.Collections.Generic;

[Serializable]
public class GameData : IData
{
    public string roomID;
    public string roomName;
    public string ID;
    public List<PlayerData> players;
    public MazeData maze;

}