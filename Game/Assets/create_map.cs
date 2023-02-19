using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_map : MonoBehaviour
{
    int size = 10;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {

        for(int i = -size; i<size; i++){
            int[,] list = new int[4, 2]{ { i, -size }, { -size, i }, { i, size }, { size, i } };
        for(int j = 0; j<4; j++){
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Light>();
            cube.transform.position = new Vector3(list[j,0], 0, list[j,1]);
            cube.transform.localScale = new Vector3(1, 2, 1);
            cube.GetComponent<MeshRenderer> ().material = material;
        }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
