using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string method;
    public string id;
    public string roomID;
    public string roomName;
    public int index;
    public string name;
    public bool alive;
    public int kills;
    public int wins;
    public Quaternion rotation;
    public Vector3 position;
    public Vector3 velocity;
    public bool newData;
    public double timestamp;
}