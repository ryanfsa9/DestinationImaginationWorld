using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeLight : MonoBehaviour
{
    public Material smokeMat;
    public Color brightColor;
    public Color darkColor;
    public GameObject sun;
    public AnimationCurve curve;
    public float light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //light = curve.Evaluate(Mathf.Abs(90-sun.transform.rotation.x)/90);
        light = sun.transform.localEulerAngles.x/90f;
        if(light>1f){light -= 4f;}
        smokeMat.SetColor("_Color", Color.Lerp(darkColor,brightColor,curve.Evaluate((light+1)/2)));

    }
}
