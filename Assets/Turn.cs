using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    public float angle = 15;
    public float speed = 1;

    void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * speed) * angle);
// print(""Mathf.Sin(Time.deltaTime));
//        print(Mathf.Sin(Time.deltaTime * speed) * angle);
    }
}
