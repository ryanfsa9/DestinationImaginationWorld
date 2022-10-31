using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raise : MonoBehaviour
{
    public GameObject water;
    public float waterStartHeight;
    public GameObject launchPad;
    public float launchPadStartHeight;
    public Vector2 limits;
    public Vector2 speeds;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        if     ( Input.GetKey("u") && !Input.GetKey("j"))  {water.transform.Translate(new Vector3(0f, 1f,0f)*Time.deltaTime*speeds[0]);}
        else if(!Input.GetKey("u") &&  Input.GetKey("j"))  {water.transform.Translate(new Vector3(0f,-1f,0f)*Time.deltaTime*speeds[0]);}

        if     ( Input.GetKey("i") && !Input.GetKey("k"))  {launchPad.transform.Translate(new Vector3(0f, 1f,0f)*Time.deltaTime*speeds[1]); particle1.enableEmission = true; particle2.enableEmission = true;}
        else if(!Input.GetKey("i") &&  Input.GetKey("k"))  {launchPad.transform.Translate(new Vector3(0f,-1f,0f)*Time.deltaTime*speeds[1]);}

        else if( Input.GetKey("o")){reset();}

        else{particle1.enableEmission = false; particle2.enableEmission = false;}

        if(water.transform.position.y > limits[0]){water.transform.position = new Vector3(water.transform.position.x, limits[0], water.transform.position.z);}
        if(launchPad.transform.position.y > limits[1]){launchPad.transform.position = new Vector3(launchPad.transform.position.x, limits[1], launchPad.transform.position.z);}
    }
    public void reset(){
        water.transform.position = new Vector3(water.transform.position.x, waterStartHeight, water.transform.position.z);
        launchPad.transform.position = new Vector3(launchPad.transform.position.x, launchPadStartHeight, launchPad.transform.position.z);
    }
}
