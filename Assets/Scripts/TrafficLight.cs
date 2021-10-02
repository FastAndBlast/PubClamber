using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public float startTime;
    public float redTime;
    // Mechanically red
    public float orangeTime;
    public float greenTime;
    private float orangeTransition;
    private float redTransition;
    public bool red;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        orangeTransition = greenTime + orangeTime;
        redTransition = orangeTransition + redTime;
    }

    // Update is called once per frame
    void Update()
    {
        Material mat = gameObject.GetComponent<MeshRenderer>().material;
        float dtime = Time.deltaTime;
        currentTime += dtime;
        if (currentTime < greenTime)
        {
            // red
            red = true;
            mat.color = Color.red;
        }
        else
        {
            if (currentTime < orangeTransition)
            {
                // green
                red = false;
                mat.color = Color.green;
            }
            else
            {
                if (currentTime < redTransition)
                {
                    // orange
                    red = true;
                    mat.color = Color.yellow;
                }
                else
                {
                    currentTime = 0;
                    red = true;
                    mat.color = Color.red;
                }
            }
        }
    }
}
