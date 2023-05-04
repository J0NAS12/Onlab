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

    public static List<GameObject> players = new List<GameObject>();

    public SocketManager socketManager;

    // Start is called before the first frame update
    void Start()
    {
        GameValues.spidersLeft = GameValues.PlayerCounter;
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
            mazeInstance = Instantiate(mazePrefab) as Maze;
            mazeInstance.Load();
        }


        for (int i = 0; i < GameValues.lobbyPlayers.Count; i++)
        {
            GameObject spider;
            if (i == GameValues.me.index)
            {
             spider = Instantiate(me) as GameObject;
            }
            else{
                spider = Instantiate(opponents) as GameObject;
            }
            players.Add(spider);
            spider.transform.position = positions[i];
        }
    }

    public static void MovePlayer(PlayerData playerData){
        if(GameValues.me.index != playerData.index){
            var desiredDir = playerData.movement;
            var player = players[playerData.index];
            if (desiredDir != Vector3.zero)
            {
                Vector3 currentDir = player.transform.rotation * Vector3.forward;
                player.transform.LookAt(Vector3.Lerp(currentDir, desiredDir, PlayerMovement.rotationSpeed * Time.fixedDeltaTime) + player.transform.position);
                float forceMultiplier = Vector3.Dot(desiredDir.normalized, currentDir);
                //since we never want to apply a negative force if the character is facing opposite to the direction we will move it
                //we make sure forceMultipler is not negative.
                if (forceMultiplier < 0) forceMultiplier = 0;
                var rb = player.GetComponent<Rigidbody>();
                //we add the force in the direction character is facing
                rb.AddForce(currentDir * forceMultiplier * PlayerMovement.speed * Time.fixedDeltaTime);
                rb.drag = PlayerMovement.drag;
            }
        }
    }
}