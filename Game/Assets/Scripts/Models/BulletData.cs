using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BulletData : IData
{
    public PlayerData shooter;
    public Vector3 startPoint;
    public Quaternion rotation;
    public float speed;
    public double timestamp;
}