using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController: MonoBehaviour
{
    public Slider speedSlider;
    public Slider TrafficDensitySlider;

    public Text speed;
    public Text Density;

	Driver driver;
    public List<GameObject> cars = new List<GameObject>();
    public Transform road;

    List<Transform> DrivingLanes = new List<Transform>();

    Vector3 pos;
    GameObject vehicle;

    List<Driver> inFrontDriver = new List<Driver>();
    List<float> timer = new List<float>();

    float averageSpeed;
    public float averageTime;

    private void Start()
    {
        averageSpeed = 30f + 20f * speedSlider.value;
        speed.text = ((int)averageSpeed).ToString() + " m/s";
        averageTime = 0.8f + 3f * (1 - TrafficDensitySlider.value);
        if (TrafficDensitySlider.value * 100f <= 33)
        {
            Density.text = "Light";
        }
        if (TrafficDensitySlider.value * 100f > 33 && TrafficDensitySlider.value * 100f <= 66)
        {
            Density.text = "Medium";
        }
        if (TrafficDensitySlider.value * 100f > 66)
        {
            Density.text = "Heavy";
        }

        for (int i = 0; i < road.childCount; i++)
        {
            DrivingLanes.Add(road.GetChild(i));
            pos = DrivingLanes[i].position - 330f * DrivingLanes[i].forward;
            vehicle = Instantiate(cars[(int)Random.Range(0, 10)]);
            vehicle.transform.position = pos;
            vehicle.transform.rotation = DrivingLanes[i].rotation;
            timer.Add(Random.Range(averageTime - 0.4f, averageTime + 0.4f));
            driver = vehicle.GetComponent<Driver>();
            driver.speed = Random.Range(averageSpeed - 5f, averageSpeed + 5f);
            inFrontDriver.Add(driver);
        }

    }

    private void Update()
    {
        //read from slider and set the outputs
        averageSpeed = 20f + 20f * speedSlider.value;
        speed.text = ((int)averageSpeed).ToString() + " m/s";
        averageTime = 0.5f + 5f * (1 - TrafficDensitySlider.value);

        if (TrafficDensitySlider.value * 100f <= 33)
        {
            Density.text = "Light";
        }
        if (TrafficDensitySlider.value * 100f > 33 && TrafficDensitySlider.value * 100f <= 66)
        {
            Density.text = "Medium";
        }
        if (TrafficDensitySlider.value * 100f > 66)
        {
            Density.text = "Heavy";
        }

        for (int i = 0; i < timer.Count; i++)
        {
            //tick the timer
            timer[i] -= Time.deltaTime;

            //check if time has expired
            if (timer[i] < 0)
            {
                pos = DrivingLanes[i].position - 330f * DrivingLanes[i].forward;
                vehicle = Instantiate(cars[(int)Random.Range(0, 10)]);
                vehicle.transform.position = pos;
                vehicle.transform.rotation = DrivingLanes[i].rotation;
                timer[i] = Random.Range(averageTime - 0.4f, averageTime + 0.4f);
                driver = vehicle.GetComponent<Driver>();
                setspeed(inFrontDriver[i]);
                inFrontDriver[i] = driver;
            }

        }

    }

    //the maxspeed of the driver should be the speed at which it will collide with the driver infront at slightly ahead of end of the lane
    private void setspeed(Driver frontDriver)
    {
        driver.speed = (int)Random.Range(averageSpeed - 5f, averageSpeed + 5f);
        float maxTime = 1.25f + driver.laneLength / driver.speed;
        float maxspeed = frontDriver.speed + frontDriver.distanceCovered / maxTime;
        if (maxspeed < averageSpeed + 5f)
        {
            driver.speed = (int)Random.Range(averageSpeed - 5f, maxspeed);
        }
    }


    //to make the software interesting, traffic density variation and speed variation, should judge minmax speed and minmax time delay between spawning

    //also I thought of a way to implement lane scaling with cursor. use UI image 2d position instead of the lane gameojects. each image will have a
    //solid colour and there will be a legend to indicate colour for each lane. We should also display every lane width, road width, and 
    //road length. all will be done in the UI manager. and a hotkey may be used to view lane from top. simulation can also be
    //toggled in the UI manager. ***dont forget to implement min width for each lane***
    //also user should have the ability in UI to change these parameters for each lane anytime rather than only at instantiation
    //note that for parking lanes only a one time instantiation should be used and only parameter should be traffic density
    //and editing speed variation will not be allowed for parking lanes.
    //it may also be convenient to display average gap between vehicle for each lane. maybe use a link list for the vehicles
    //by adding a driver type called frontDriver in the driver script.
    //Maybe even add a paramater called vehicle safety gap for each lane. this will allow us to alter instantiation time to not only
    //avoid collision but avoid breaking the safety gap. But user should know traffic density will be affected by safety gap.
    //maybe even mention the minimum and maximum average safety gap that can occur based on traffic simulation. This can be used to constraint
    //the user input for safety gap. to avoid breaking the safety gap, vehicle speed should be set to front vehicle speed once
    //the safety gap is achieved
}
