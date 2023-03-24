using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public Maze mazePrefab;
	private Maze mazeInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
    }
}