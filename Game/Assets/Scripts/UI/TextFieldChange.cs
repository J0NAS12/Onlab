using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFieldChange : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField playerName;
    public TMP_InputField roomName;

    void Start()
    {
        playerName.text = GameValues.me.name;
    }

    public void ChangeName(string value){
        GameValues.me.name = value;
    }

    public void ChangeIp(string value){
        GameValues.serverip = value;
    }

        public void ChangeRoomName(string value){
        GameValues.me.roomName = value;
    }
}
