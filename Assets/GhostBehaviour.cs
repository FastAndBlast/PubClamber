using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehaviour : MonoBehaviour
{
    public float speed = 0.5f;
    public float lifetime = 10f;

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        Destroy(gameObject, lifetime);
    }
}
