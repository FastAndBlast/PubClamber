using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 signRotation;
    public Vector3 signPositionOffset;

    //public float speed;
    public float inverseSpeed = 1;

    Vector3 offset;

    Vector3 velocity;

    private void Start()
    {
        if (target)
        {
            offset = transform.position - target.position;
        }
    }


    private void LateUpdate()
    {
        if (target)
        {
            //float distance = Vector3.Distance(transform.position, target.position + offset);

            //transform.position = Vector3.MoveTowards(transform.position, target.position + offset, (speed + distance * distance / 10) * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, inverseSpeed);
        }
    }
}
