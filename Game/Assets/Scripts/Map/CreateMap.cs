using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;

    public GameObject me;

    public GameObject opponents;
    public Vector3[] positions;

    public SocketManager socketManager;

    // Start is called before the first frame update
    void Start()
    {
        GameValues.spidersLeft = GameValues.PlayerCounter;
        if (GameValues.maze.cells == null)
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
                        position = item.coordinates
                    };
                    GameValues.maze.cells.Add(data);
            }
            var gameStart = new LobbyData
            {
                method = "startGame",
                lobbyID = GameValues.me.lobbyID,
                maze = GameValues.maze
            };
            GameValues.socket.Send(JsonUtility.ToJson(gameStart));
        }
        else
        {
            Debug.Log("Loaded the scene");
            mazeInstance = Instantiate(mazePrefab) as Maze;
            mazeInstance.Load();
        }


        for (int i = 0; i < GameValues.PlayerCounter; i++)
        {
            GameObject spider;
            if (i == 0)
            {
             spider = Instantiate(me) as GameObject;
                socketManager.player = spider;
            }
            else{
                spider = Instantiate(opponents) as GameObject;
            }
            spider.transform.position = positions[i];
        }


    }
}