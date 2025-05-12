using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStats
{
    public Color color;
    public float size;
    public float totalRatio = 0f;
    public int observaciones = 0;

    public float Score => observaciones == 0 ? 0 : totalRatio / observaciones;
}


