using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Road road;
    public float baseSpeed;
    public float currentSpeed;
    public int index;
    private GameObject currentTarget;
    private Collider targetCollider;
    private int waypointNum = 1;
    
    private void Start()
    {
        TargetWaypoint(1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.paused)
        {
            return;
        }    

        transform.LookAt(currentTarget.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);

        TrafficLight trafficLight = currentTarget.GetComponent<TrafficLight>();
        if (trafficLight != null)
        {
            if (trafficLight.red)
            {
                float dist = (trafficLight.transform.position - transform.position).magnitude;
                currentSpeed = baseSpeed * GetSlowdownFactor(dist);
            }
            else
            {
                currentSpeed = baseSpeed;
            }
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        if (index > 0)
        // There is a car infront of us
        {
            Car nextCar = road.cars[index - 1];

            // Lazy straight line distance
            float dist = (nextCar.transform.position - transform.position).magnitude;
            float currentSpeed2 = baseSpeed * GetSlowdownFactor(dist);
            currentSpeed = Mathf.Min(currentSpeed, currentSpeed2);
        }

        float changeY = transform.position.y - Mathf.MoveTowards(transform.position.y, currentTarget.transform.position.y, Time.deltaTime * 10);

        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetCollider.transform.position, currentSpeed);

        GetComponent<Rigidbody>().MovePosition(newPosition + Vector3.up * changeY);

        //Vector3 positionNoY = new Vector3(transform.position.x, 0, transform.position.z);
        //Vector3 colliderPositionNoY = new Vector3(targetCollider.transform.position.x, 0, targetCollider.transform.position.z);

        //if (Vector3.Distance(positionNoY, colliderPositionNoY) < 0.1f)
        //{
        //    NextWaypoint();
        //}
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other == targetCollider)
        {
            NextWaypoint();
        }
    }
    

    public void NextWaypoint()
    {
        waypointNum++;
        if (waypointNum < road.waypoints.Count)
        {
            TargetWaypoint(waypointNum);
        }
        else
        {
            // Cars cannot pass each other
            road.cars.Remove(this);
            road.UpdateIndex(-1);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //print("Hit by a car");
            SFXManager.instance.PlaySFX(Random.Range(17, 20));
            if (GameManager.instance.player.GetComponent<WalkingManager>().on)
            {
                BodyFunctions.instance.Die("Hit by a car", "Tip: Wait for the green light");
            }
        }
    }

    private void TargetWaypoint(int n) 
    {
        currentTarget = road.waypoints[n];
        targetCollider = currentTarget.GetComponent<Collider>();
    }

    private float GetSlowdownFactor(float distance)
    // Linear decrease from 15 to 3 metres
    {
        return Mathf.Clamp01((distance - 5) / 12);
    }
}
