using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    GameObject target;

    Transform arrowTransform;

    private void Start()
    {
        target = GameObject.Find("EndOfLevelBox");
        arrowTransform = transform.GetChild(0);
    }

    void Update()
    {
        transform.up = target.transform.position - transform.position;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        arrowTransform.localPosition = new Vector3(0, Mathf.Sin(Time.time * 3) * 0.25f + 2, 0);

    }
}
