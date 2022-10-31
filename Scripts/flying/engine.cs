using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class engine : MonoBehaviour
{
    public float thrust;
    public AnimationCurve thrustCurve;
    public float throttleSpeed;
    flightScript script;
    Rigidbody rb;
    public float throttle;

    void Start()
    {
        throttle = 0f;
        rb = GetComponent<Rigidbody>();
        script = GetComponent<flightScript>();
    }

    void Update()
    {
        if(script.engineOn){
            transform.Find("Particle System").GetComponent<ParticleSystem>().Play();
            if(Input.GetKey("left shift")){throttle += throttleSpeed*Time.deltaTime;}
            if(Input.GetKey("left ctrl" )){throttle -= throttleSpeed*Time.deltaTime;}
            if(Input.GetKey("z")){throttle = 1f;}
            if(Input.GetKey("x")){throttle = 0f;}
            throttle = Mathf.Clamp(throttle,0f,1f);
        }
        else{
            throttle = 0f;
            transform.Find("Particle System").GetComponent<ParticleSystem>().Stop();
        }
    }
    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * throttle * thrust * thrustCurve.Evaluate(script.speed), ForceMode.Acceleration);
    }
}
