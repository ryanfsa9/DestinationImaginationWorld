using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float RotSpeed;
    Vector3 camOff;
    public Vector3 startPos;
    public Vector3 startRot;
    public Vector3 endPos;
    public Vector3 endRot;
    public float animationSpeed;
    public AnimationCurve aCurve;
    public AnimationCurve rotationCurve;
    bool buildingDestroyed;
    bool greenMode;
    private Vector3 m;
    bool p;
    float aTime;
    carScript cScript;
    flightScript fScript;
    public GameObject bighouse;
    public GameObject bighousedestroyed;

    [Range(0.01f, 0.5f)]
    public float moveSpeed;

    [Range(0.0f, 1.0f)]
    public float rotateSpeed;

    public Vector3 carPos;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        buildingDestroyed = false;
        greenMode = false;
        bighousedestroyed.SetActive(false);
        rb = this.GetComponent<Rigidbody>();
        Cursor.visible = false;
        cScript = GameObject.Find("rover").GetComponent<carScript>();
        fScript = GameObject.Find("jet").GetComponent<flightScript>();
        camOff = (transform.position - fScript.transform.position)/2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("0")){
            if(buildingDestroyed){
                bighousedestroyed.SetActive(false);
                bighouse.SetActive(true);
            }
            else{
                bighousedestroyed.SetActive(true);
                bighouse.SetActive(false);
            }
            buildingDestroyed = !buildingDestroyed;
        }
        if(Input.GetKeyDown("1")){
            greenMode = !greenMode;
            if(greenMode){
                GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
                GetComponent<Camera>().farClipPlane = 500f;
            }
            else{
                GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                GetComponent<Camera>().farClipPlane = 1000000f;
            }
        }
        m = new Vector3();
        if(Input.GetKeyDown("8")){p = true; aTime = 0; RenderSettings.fogColor = new Color(1,0,0,1);}
        if(p){
            transform.position = Vector3.Lerp(startPos,endPos,aCurve.Evaluate(aTime));
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot,endRot,rotationCurve.Evaluate(aTime)));
            aTime += animationSpeed*Time.deltaTime;
            if(aTime >= 1f){p = false; RenderSettings.fogColor = new Color(1,1,0.239f,1f);}
        }
        else if(!cScript.drivingCar && !fScript.camFollowing){
            if(!cScript.powered && !fScript.engineOn){
                transform.parent = null;
                if(Input.GetKey("f"))  {moveSpeed *= 1.01f;}
                if(Input.GetKey("g"))  {moveSpeed *= 0.99f;}
                if(moveSpeed>=0.5f){moveSpeed = 0.5f;}
                if(moveSpeed<=0.005f){moveSpeed = 0.005f;}


                if     ( Input.GetKey("w") && !Input.GetKey("s"))  {m[2] =  1;}
                else if(!Input.GetKey("w") &&  Input.GetKey("s"))  {m[2] = -1;}

                if     ( Input.GetKey("d") && !Input.GetKey("a"))  {m[0] =  1;}
                else if(!Input.GetKey("d") &&  Input.GetKey("a"))  {m[0] = -1;}

                if     ( Input.GetKey("space") && !Input.GetKey("left shift"))  {m[1] =  1;}
                else if(!Input.GetKey("space") &&  Input.GetKey("left shift"))  {m[1] = -1;}

                if     ( Input.GetKey("escape"))  {Application.Quit();}


                transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed * 200f);
                Quaternion q = transform.rotation;
                q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
                transform.rotation = q;
                rb.mass =  moveSpeed;
            }
        }
        else if(cScript.drivingCar){
            transform.parent = cScript.transform;
            transform.localPosition = carPos;
            transform.localRotation = Quaternion.Euler(10f,0,0);
        }
        else if(!fScript.camRotating){
            transform.parent = null;
            transform.LookAt(fScript.transform);
            transform.position = Vector3.Lerp(transform.position,fScript.transform.TransformPoint(new Vector3(0f,1f,-10f)),15f*Time.deltaTime);
        }
        else{
            transform.parent = null;
            transform.LookAt(fScript.transform);
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotSpeed, Vector3.up);
            camOff = camTurnAngle * camOff;
            Vector3 newPos = fScript.transform.position + camOff;
            transform.position = newPos;
            rb.velocity = fScript.rb.velocity;
        }
    }
    void FixedUpdate()
    {
        rb.AddRelativeForce(m*Time.deltaTime*200f);
    }
}
