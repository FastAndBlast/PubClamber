using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StenchSource : MonoBehaviour
{
    public float stenchScore;
    public float range;
    private BodyFunctions player;
    // Start is called before the first frame update
    void Start()
    {
        player = BodyFunctions.instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!GameManager.paused)
        {
            if ((player.transform.position - transform.position).magnitude < range)
            {
                player.nearbyStench = Mathf.Max(player.nearbyStench, stenchScore);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
