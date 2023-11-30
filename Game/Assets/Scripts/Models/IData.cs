using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IData
{
    public string method;
    public void SendToServer(){
        GameValues.socket.Send(JsonUtility.ToJson(this));
    }
}
