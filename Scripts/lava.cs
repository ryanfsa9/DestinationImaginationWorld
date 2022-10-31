using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava : MonoBehaviour
{
    public Vector2 lavaSpeed;
    public TerrainLayer lavaLayer;
    private float xOff;
    private float yOff;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        xOff += Time.deltaTime;
        yOff += Time.deltaTime;
        if(xOff >= lavaLayer.tileSize[0])  {xOff = 0;}
        if(yOff >= lavaLayer.tileSize[0])  {yOff = 0;}

        lavaLayer.tileOffset = new Vector2(xOff*lavaSpeed[0],yOff*lavaSpeed[1]);
    }
}