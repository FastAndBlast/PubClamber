using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        GetComponentInParent<WalkingManager>().Collided();


        //Transform topLevelParent = other.gameObject.transform;
        //while (true)
        //{
        //    if (topLevelParent.parent)
        //    {
        //        topLevelParent = other.gameObject.transform.parent;
        //    }
        //    else
        //    {
        //        break;
        //    }
        //}


        //if (!other.GetComponent<Rigidbody>() && topLevelParent != GetComponentInParent<WalkingManager>().transform)
        //{
        //GetComponentInParent<WalkingManager>().Collided(other.ClosestPoint(transform.position));
        //GetComponentInParent<WalkingManager>().Stumble();
        //}
    }
}
