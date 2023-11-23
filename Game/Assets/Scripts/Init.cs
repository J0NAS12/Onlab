using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Init : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField Playername;
    public TMP_InputField Gamename;

    void Start()
    {
        Playername.text = GameValues.me.name;
    }
}
