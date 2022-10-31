using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carScript : MonoBehaviour
{
    public bool drivingCar;
    public bool powered;
    public GameObject car;
    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider BL;
    public WheelCollider BR;
    public float steering;
    public float drive;
    public float speed;
    public Material trail;
    float offset;
    public float offSpeed;
    public GameObject helmet;
    public GameObject barrel;
    void Start()
    {
        steering = 0f;
        car.GetComponent<Rigidbody>().centerOfMass = new Vector3(0,-1,0);
        car.GetComponent<Rigidbody>().inertiaTensor = new Vector3(100000,100000,100000);
        offset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("l")){powered = !powered;}
        if(Input.GetKeyDown(";")){drivingCar = !drivingCar;}
        if(Input.GetKeyDown(",")){helmet.SetActive(!helmet.activeSelf);}
        if(Input.GetKeyDown(".")){barrel.SetActive(!barrel.activeSelf);}
        offset -= gameObject.GetComponent<Rigidbody>().velocity.magnitude * offSpeed * Time.deltaTime;
        drive = 0f;
        transform.Find("P1").gameObject.SetActive(powered);
        transform.Find("P2").gameObject.SetActive(powered);
        if(powered){
            if(Input.GetKey("a"))  {steering -= 20 * Time.deltaTime;}
            if(Input.GetKey("d"))  {steering += 20 * Time.deltaTime;}
            if(Input.GetKey("w"))  {drive = 1f;}
            if(Input.GetKey("s"))  {drive = -1f;}
            BL.brakeTorque = 0f;
            BR.brakeTorque = 0f;
        }
        else{
            BL.brakeTorque = 10000f;
            BR.brakeTorque = 10000f;
        }
        trail.SetTextureOffset("_MainTex", new Vector2(offset,0));
    }
    void FixedUpdate()
    {
        FL.steerAngle = steering;
        FR.steerAngle = steering;
        FL.gameObject.transform.localEulerAngles = new Vector3(0f, steering - 90f, 90f);
        FR.gameObject.transform.localEulerAngles = new Vector3(0f, steering - 90f, 90f);
        FL.motorTorque = drive * speed;
        FR.motorTorque = drive * speed;
        BL.motorTorque = drive * speed;
        BR.motorTorque = drive * speed;
        //FR.motorTorque = -drive*100000;
        //car.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,-50f,drive), ForceMode.Acceleration);
    }
}
