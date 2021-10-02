using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public List<GameObject> waypoints;
    public List<GameObject> carPrefabs;
    public List<Car> cars;

    public float timeBetweenCars;
    private int carsSpawned = 0;
    private float timeSinceCar;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            waypoints.Add(child.gameObject);
        }
        timeSinceCar = timeBetweenCars;
    }

    // Update is called once per frame
    void Update()
    {
        float dtime = Time.deltaTime;
        timeSinceCar += dtime;

        if (timeSinceCar > timeBetweenCars)
        {
            // Spawn a new car
            GameObject newCar = Instantiate(carPrefabs[carsSpawned % carPrefabs.Count]);
            newCar.transform.position = waypoints[0].transform.position;
            newCar.transform.parent = transform;
            Car car = newCar.GetComponent<Car>();
            car.index = cars.Count;
            cars.Add(car);
            car.road = this;

            carsSpawned++;
            timeSinceCar = 0;
        }
    }

    public void UpdateIndex(int n)
    // Move the index of all cars by n, inteded for use after adding more cars
    {
        foreach (Car car in cars)
        {
            car.index += n;
        }
    }
}
