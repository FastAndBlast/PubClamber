using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < 3)
        {
            GameManager.instance.EndOfLevel();
        }
    }
}
