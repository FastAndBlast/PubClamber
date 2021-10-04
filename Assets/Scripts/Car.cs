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
        transform.LookAt(currentTarget.transform);

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

        transform.position += transform.forward * currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == targetCollider)
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
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Hit by a car");
            BodyFunctions.instance.Die("Hit by a car");
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
        return Mathf.Clamp01((distance - 3) / 12);
    }
}
