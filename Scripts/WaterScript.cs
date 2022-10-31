using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public Material waterMat;
    public Material lavaMat;
    public Material fuelMat;
    public Material oceanMat;
    public Vector2  waterSpeed;
    public Vector2  lavaSpeed;
    public Vector4  oceanSpeed;
    private float offset;
    private float fuelOff;
    void Start()
    {
        offset = 0f;
        fuelOff = 0f;
    }

    void Update()
    {
        offset += Time.deltaTime;
        fuelOff = Mathf.PerlinNoise(offset/2f,0f);

        waterMat.SetTextureOffset("_MainTex"        , new Vector2(0,offset) * waterSpeed[0] / 100f);
        waterMat.SetTextureOffset("_DetailAlbedoMap", new Vector2(45f,offset) * waterSpeed[1] / 100f);

        lavaMat.SetTextureOffset ("_MainTex"        , new Vector2(0,offset) *  lavaSpeed[0] / 100f);
        lavaMat.SetTextureOffset ("_DetailAlbedoMap", new Vector2(45f,offset) *  lavaSpeed[1] / 100f);

        fuelMat.SetTextureOffset ("_MainTex"        , new Vector2(0,fuelOff) / 10f);
        fuelMat.SetTextureOffset ("_DetailAlbedoMap", new Vector2(fuelOff+45f,0) / 10f);

        oceanMat.SetTextureOffset("_BumpMap1"        , new Vector2(offset*oceanSpeed[0],offset*oceanSpeed[1]) / 100f);
        oceanMat.SetTextureOffset("_BumpMap2"        , new Vector2(offset*oceanSpeed[2],offset*oceanSpeed[3]) / 100f);
        oceanMat.SetTextureOffset("_BumpMap3"        , new Vector2(offset*-oceanSpeed[0],offset*-oceanSpeed[2]) / 100f);
    }
}