using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMove : MonoBehaviour
{
    
    void Update()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + Vector3.forward * Time.deltaTime);
    }
}
