using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flightScript : MonoBehaviour
{
    public float speed;
    public float banking;
    public Rigidbody rb;
    public bool camFollowing;
    public bool camRotating;
    public bool engineOn;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0.01f,0.01f,0.01f);
        speed = transform.InverseTransformDirection(rb.velocity).z * 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        speed = transform.InverseTransformDirection(rb.velocity).z * 0.75f;

        if(Input.GetKeyDown("5")){camFollowing = !camFollowing;}
        if(Input.GetKeyDown("6")){camRotating = !camRotating;}
        if(Input.GetKeyDown("7")){engineOn = !engineOn;}
    }
}
