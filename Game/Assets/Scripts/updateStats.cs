using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class updateStats : MonoBehaviour
{

    public TextMeshProUGUI spidersLeft;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spidersLeft.text = "Spiders Left: " + GameValues.spidersLeft;
    }
}
