using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{

    private float startPos;
    private float length;
    public Camera cam;
    public float parallaxEffect;


    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));//1-paralax bc <1 means move left and more paralax more movement left
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if(temp > startPos + length)
        {
            startPos += length;
        }else if (temp < startPos - length)
        {
            startPos -= length;
        }

    }
}
