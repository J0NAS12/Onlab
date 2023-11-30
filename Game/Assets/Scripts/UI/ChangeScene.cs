using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void MenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitApp()
    {
        Application.Quit();
    }



}