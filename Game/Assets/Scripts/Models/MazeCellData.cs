using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MazeCellData
{
    public IntVector2 position;
    public int north;
    public int south;
    public int west;
    public int east;

}

