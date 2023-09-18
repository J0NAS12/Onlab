using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateMap : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;
    public GameObject round;
    public GameObject me;
    public GameObject opponents;
    public Vector3[] positions;
    public static List<GameObject> players = new List<GameObject>();

    public SocketManager socketManager;

    // Start is called before the first frame update
    void Start()
    {
        GameValues.spidersLeft = GameValues.lobbyPlayers.Count;
        if (GameValues.maze.cells == null || GameValues.maze.cells.Count == 0)
        {
            mazeInstance = Instantiate(mazePrefab) as Maze;
            mazeInstance.Generate();
            GameValues.maze.cells = new List<MazeCellData>();
            foreach (var item in mazeInstance.cells)
            {
                var data = new MazeCellData
                {
                    north = item.GetEdge(MazeDirection.North).Type,
                    south = item.GetEdge(MazeDirection.South).Type,
                    east = item.GetEdge(MazeDirection.East).Type,
                    west = item.GetEdge(MazeDirection.West).Type,
                    room = item.room.settingsIndex,
                    position = item.coordinates
                };
                GameValues.maze.cells.Add(data);
            }
            var gameStart = new GameData
            {
                method = "startGame",
                lobbyID = GameValues.me.lobbyID,
                maze = GameValues.maze
            };
            GameValues.socket.Send(JsonUtility.ToJson(gameStart));
        }
        else
        {
            mazeInstance = Instantiate(mazePrefab) as Maze;
            mazeInstance.Load();
        }


        for (int i = 0; i < GameValues.lobbyPlayers.Count; i++)
        {
            GameValues.lobbyPlayers[i].position = positions[i];
            GameObject spider;
            if (i == GameValues.me.index)
            {
                spider = Instantiate(me) as GameObject;
            }
            else
            {
                spider = Instantiate(opponents) as GameObject;
                spider.GetComponent<OpponentMovement>().index = i;
            }
            spider.name = i.ToString();
            players.Add(spider);
            spider.transform.position = positions[i];
        }
    }

    private void FixedUpdate()
    {
        try
        {
            while (GameValues.hitList.Count != 0)
            {
                var hit = GameValues.hitList[0];
                GameValues.hitList.Remove(hit);
                Destroy(players[hit.player.index]);
                GameValues.spidersLeft--;
                GameValues.lobbyPlayers[hit.player.index].alive = false;
                GameValues.lobbyPlayers[hit.shooter].kills++;
                GameValues.me.kills = GameValues.lobbyPlayers[GameValues.me.index].kills;
            }
            while (GameValues.bulletList.Count != 0)
            {
                var bullet = GameValues.bulletList[0];
                GameValues.bulletList.Remove(bullet);
                GameObject spawnedRound = Instantiate(
                    round,
                    bullet.startPoint,
                    bullet.rotation
                );
                spawnedRound.name = bullet.shooter.index.ToString();
                Rigidbody rb = spawnedRound.GetComponent<Rigidbody>();
                rb.velocity = spawnedRound.transform.forward * bullet.speed;
                double timestamp = Clock.getTime();
                spawnedRound.transform.position += spawnedRound.transform.forward * bullet.speed * (float)(timestamp - bullet.timestamp);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}