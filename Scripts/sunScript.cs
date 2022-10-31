using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if     ( Input.GetKey("z") && !Input.GetKey("x"))  {transform.Rotate(new Vector3( 20*Time.deltaTime,0,0), Space.World);}
        else if(!Input.GetKey("z") &&  Input.GetKey("x"))  {transform.Rotate(new Vector3(-20*Time.deltaTime,0,0), Space.World);}

        if     ( Input.GetKey("v") && !Input.GetKey("c"))  {transform.Rotate(new Vector3(0, 20*Time.deltaTime,0), Space.World);}
        else if(!Input.GetKey("v") &&  Input.GetKey("c"))  {transform.Rotate(new Vector3(0,-20*Time.deltaTime,0), Space.World);}
    }
}
