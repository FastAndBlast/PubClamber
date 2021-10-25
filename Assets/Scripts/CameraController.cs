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

    public Vector3 boundingBoxPos;
    public Vector2 boundingBoxSize;

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

        Vector3 clamped = new Vector3(
            Mathf.Clamp(transform.position.x, boundingBoxPos.x - boundingBoxSize.x / 2, boundingBoxPos.x + boundingBoxSize.x / 2),
            transform.position.y,
            Mathf.Clamp(transform.position.z, boundingBoxPos.z - boundingBoxSize.y / 2, boundingBoxPos.z + boundingBoxSize.y / 2));

        transform.position = clamped;
    }

    public void FocusSign(Transform signTransform)
    {
        target = signTransform;
        trackSign = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(boundingBoxPos, new Vector3(boundingBoxSize.x, 0, boundingBoxSize.y));
    }
}
