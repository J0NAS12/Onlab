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
    public void RoomScene()
    {
        SceneManager.LoadScene("Room");
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