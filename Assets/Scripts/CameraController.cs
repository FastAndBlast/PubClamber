using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    Vector3 signEulerAngles = new Vector3(0, 177, 0);
    Vector3 signPositionOffset; // = - new Vector3(-3.12021589f, 0.245577797f, -0.618911028f);

    //public float speed;
    public float inverseSpeed = 1;

    Vector3 offset;

    Vector3 velocity;

    bool trackSign = false;

    private void Start()
    {
        if (target)
        {
            offset = transform.position - target.position;
        }
    }


    private void LateUpdate()
    {
        if (target && !trackSign)
        {
            //float distance = Vector3.Distance(transform.position, target.position + offset);

            //transform.position = Vector3.MoveTowards(transform.position, target.position + offset, (speed + distance * distance / 10) * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, inverseSpeed);
        }
        else if (target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position + signPositionOffset, ref velocity, inverseSpeed);
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, signEulerAngles, Time.deltaTime * 60);
        }
    }

    public void FocusSign(Transform signTransform)
    {
        target = signTransform;
        trackSign = true;
    }
}
