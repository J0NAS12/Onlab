using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class updateStats : MonoBehaviour
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
            GameValues.lobbyPlayers.Find(x => x.alive).wins++;
            GameValues.me.wins = GameValues.lobbyPlayers[GameValues.me.index].wins;
            StartCoroutine(Order());

        }
    }
    IEnumerator Order()
    {
        winner.text = "Winner: " + GameValues.lobbyPlayers.Find(x => x.alive).name;
        GameValues.startGame = false;
        GameValues.playersChanged = true;
        Debug.Log(GameValues.lobbyPlayers.Count);
        GameValues.maze.cells = null;
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("Lobby");
    }

}
