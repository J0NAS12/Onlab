using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUpdate : MonoBehaviour
{

    public TextMeshProUGUI spidersLeft;
    public TextMeshProUGUI winner;
    private bool started = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spidersLeft.text = "Spiders Left: " + GameValues.spidersLeft;

        if (GameValues.spidersLeft <= 1 && !started)
        {
            started = true;
            GameValues.roomPlayers.Find(x => x.alive).wins++;
            GameValues.me.wins = GameValues.roomPlayers[GameValues.me.index].wins;
            StartCoroutine(GameOver());

        }
    }
    IEnumerator GameOver()
    {
        winner.text = "Winner: " + GameValues.roomPlayers.Find(x => x.alive).name;
        GameValues.startGame = false;
        GameValues.playersChanged = true;
        Debug.Log(GameValues.roomPlayers.Count);
        GameValues.maze.cells = null;
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("Room");
    }

}
