using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_map : MonoBehaviour
{
    int size = 10;
    public Material material;
    public Material wall;
    // Start is called before the first frame update
    void Start()
    {
        CreateWall();
        //CreateCrates();

    }

    void CreateCrates(){
        int i = 1;
        for(int j = 0; j<4; j++){
            int[,] list = new int[4, 2]{ { i, -size }, { -size, i }, { i, size }, { size, i } };
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Light>();
            cube.transform.position = new Vector3(list[j,0], 0, list[j,1]);
            cube.transform.localScale = new Vector3(1, 2, 1);
            cube.GetComponent<MeshRenderer> ().material = material;
        }
    }

    void CreateWall(){
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.transform.position = new Vector3(0,0,-25);
        cube2.transform.position = new Vector3(-25,0,0);
        cube1.transform.eulerAngles = new Vector3(0,90,0);
        cube3.transform.position = new Vector3(0,0,25);
        cube4.transform.position = new Vector3(25,0,0);
        cube3.transform.eulerAngles = new Vector3(0,90,0);
        GameObject[] walls = new GameObject[4]{cube1, cube2, cube3, cube4};
        foreach (var cube in walls)
        {
            cube.AddComponent<Light>();
            cube.GetComponent<MeshRenderer>().material = wall;
            cube.transform.localScale = new Vector3(1,4,50);
        }
    }
}
