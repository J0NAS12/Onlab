using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitData : IData
{
    public string roomID;
    public PlayerData player;
    public int shooter;

}
