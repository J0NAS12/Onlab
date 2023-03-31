using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public Maze mazePrefab;
	private Maze mazeInstance;

    public GameObject[] players;
    public Vector3[] positions;
    
    // Start is called before the first frame update
    void Start()
    {
        GameValues.spidersLeft = GameValues.PlayerCounter;
        Debug.Log(GameValues.spidersLeft);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
        for(int i=0; i<GameValues.PlayerCounter; i++){
            var spider = Instantiate(players[i]) as GameObject;
            spider.transform.position = positions[i];
        }


    }
}