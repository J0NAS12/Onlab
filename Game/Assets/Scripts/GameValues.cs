using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameValues : MonoBehaviour
{
    public static int PlayerCounter = 2;
    public static int spidersLeft = 2;

    public void ChangePlayerCounter(int value){
        PlayerCounter = value + 2;
    }
}
