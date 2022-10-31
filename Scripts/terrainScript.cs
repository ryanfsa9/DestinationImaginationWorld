using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class terrainScript : MonoBehaviour
{
    public Terrain terrain;
    public TerrainData Original;
    public TerrainData withHole;
    public Vector2 raiseLocation;
    public int raiseRadius;
    public float raiseHeight;
    private float[,] originalHeights;
    private float[,] raisedHeights;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2();
        originalHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
        terrain.detailObjectDistance = 5000;
        Terrain[] subTerrainArray = terrain.GetComponentsInChildren<Terrain>();
        foreach(Terrain ter in subTerrainArray){
            ter.detailObjectDistance = 5000;
        }
    }

    void Update()
    {
        if     (Input.GetKey("n")) {terrain.terrainData.SetHeights(0, 0, originalHeights);
                                    terrain.terrainData = Original;
                                    originalHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
                                    updateHeight();}
        else if(Input.GetKey("m"))  {terrain.terrainData.SetHeights(0, 0, originalHeights);
                                    terrain.terrainData = withHole;
                                    originalHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
                                    updateHeight();}
        direction[0] = 0;
        direction[1] = 0;
        if(Input.GetKey(KeyCode.UpArrow))    {raiseLocation[0] -= 50f*Time.deltaTime; direction[1] += 1; updateHeight();}
        if(Input.GetKey(KeyCode.DownArrow))  {raiseLocation[0] += 50f*Time.deltaTime; direction[1] -= 1; updateHeight();}
        if(Input.GetKey(KeyCode.LeftArrow))  {raiseLocation[1] -= 50f*Time.deltaTime; direction[0] += 1; updateHeight();}
        if(Input.GetKey(KeyCode.RightArrow)) {raiseLocation[1] += 50f*Time.deltaTime; direction[0] -= 1; updateHeight();}
    }
    void updateHeight()
    {
        raisedHeights = new float[raiseRadius*2,raiseRadius*2];
        for(int x = 0; x<raiseRadius*2; x++){
            for(int y = 0; y<raiseRadius*2; y++){
                float xPos = x*2-raiseRadius*2;
                float yPos = y*2-raiseRadius*2;

                if(direction[0] == -1){xPos *= -1f;}
                if(direction[1] == -1){yPos *= -1f;}
                if(direction[0] !=  0){
                    if(xPos>0){xPos *= 0.4f;}
                    else{xPos *= 1.5f;}
                    yPos*= 1.5f;
                }
                if(direction[1] !=  0){
                    if(yPos>0){yPos *= 0.4f;}
                    else{yPos *= 1.5f;}
                    xPos*= 1.5f;
                }
                
                
                float distance = Mathf.Sqrt(xPos*xPos+yPos*yPos);
                if(distance > raiseRadius){
                    distance = 0;
                }
                else{
                    distance = Mathf.Cos(distance/raiseRadius*Mathf.PI)+1f;
                }
                raisedHeights[x,y] = originalHeights[(int)raiseLocation[1]+x,(int)raiseLocation[0]+y]+distance/100f*raiseHeight;
            }
        }
        
        terrain.terrainData.SetHeights((int)raiseLocation[0], (int)raiseLocation[1], raisedHeights);
    }
    private void OnDestroy()
    {
        terrain.terrainData.SetHeights(0, 0, originalHeights);
    }
}
