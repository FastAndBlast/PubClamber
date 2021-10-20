using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public float startTime;
    public float redTime = 6f;
    // Mechanically red
    public float orangeTime;
    public float greenTime;
    private float orangeTransition;
    private float redTransition;
    public bool red;
    private float currentTime;

    public List<GameObject> attachedObjects;

    float timeInBetweenBeeps = 3f;
    float timePassed = 0f;

    private bool pastStart = false;

    public bool sound;

    // Start is called before the first frame update
    void Start()
    {
        orangeTransition = greenTime + orangeTime;
        redTransition = orangeTransition + redTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.paused)
        {
            float dtime = Time.deltaTime;
            currentTime += dtime;
        }
        if (!pastStart)
        {
            if (currentTime < startTime)
            {
                red = true;
                SetColor(2);
                return;
            }
            else
            {
                currentTime = 0;
                pastStart = true;
                return;
            }
        }

        if (currentTime < greenTime)
        {
            // green
            red = false;
            SetColor(0);

            // red
            //red = true;
            //SetColor(2);
        }
        else
        {
            if (currentTime < orangeTransition)
            {
                // orange
                red = true;
                SetColor(1);

                // green
                //red = false;
                //SetColor(0);
            }
            else
            {
                if (currentTime < redTransition)
                {
                    // red
                    red = true;
                    SetColor(2);

                    // orange
                    //red = true;
                    //SetColor(1);
                }
                else
                {
                    // green
                    red = false;
                    SetColor(0);
                    currentTime = 0;

                    // red transition
                    //currentTime = 0;
                    //red = true;
                    //SetColor(2);
                }
            }
        }
    }

    public void SetColor(int col)
    {
        //Material mat = gameObject.GetComponent<MeshRenderer>().material;
        if (col == 0)
        {
            //mat.color = Color.green;
            foreach (GameObject attachedObject in attachedObjects)
            {
                attachedObject.transform.Find("RedPedestrian").gameObject.SetActive(true);
                attachedObject.transform.Find("GreenPedestrian").gameObject.SetActive(false);
                attachedObject.transform.Find("Red").gameObject.SetActive(false);
                attachedObject.transform.Find("Yellow").gameObject.SetActive(false);
                attachedObject.transform.Find("Green").gameObject.SetActive(true);

                if (sound)
                {
                    attachedObject.GetComponent<AudioSource>().clip = SFXManager.instance.clips[16];
                    attachedObject.GetComponent<AudioSource>().loop = false;
                    if (!attachedObject.GetComponent<AudioSource>().isPlaying && timePassed > timeInBetweenBeeps)
                    {
                        attachedObject.GetComponent<AudioSource>().Play();
                        timePassed = 0f;
                    }
                    else
                    {
                        timeInBetweenBeeps -= Time.deltaTime;
                    }
                }
            }
        }
        else if (col == 1)
        {
            //mat.color = Color.yellow;
            foreach (GameObject attachedObject in attachedObjects)
            {
                attachedObject.transform.Find("RedPedestrian").gameObject.SetActive(true);
                attachedObject.transform.Find("GreenPedestrian").gameObject.SetActive(false);
                attachedObject.transform.Find("Red").gameObject.SetActive(false);
                attachedObject.transform.Find("Yellow").gameObject.SetActive(true);
                attachedObject.transform.Find("Green").gameObject.SetActive(false);

                if (sound)
                {
                    attachedObject.GetComponent<AudioSource>().clip = SFXManager.instance.clips[16];
                    attachedObject.GetComponent<AudioSource>().loop = false;

                    if (!attachedObject.GetComponent<AudioSource>().isPlaying && timePassed > timeInBetweenBeeps)
                    {
                        attachedObject.GetComponent<AudioSource>().Play();
                        timePassed = 0f;
                    }
                    else
                    {
                        timeInBetweenBeeps -= Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            //mat.color = Color.red;
            foreach (GameObject attachedObject in attachedObjects)
            {
                attachedObject.transform.Find("RedPedestrian").gameObject.SetActive(false);
                attachedObject.transform.Find("GreenPedestrian").gameObject.SetActive(true);
                attachedObject.transform.Find("Red").gameObject.SetActive(true);
                attachedObject.transform.Find("Yellow").gameObject.SetActive(false);
                attachedObject.transform.Find("Green").gameObject.SetActive(false);

                if (sound)
                {
                    attachedObject.GetComponent<AudioSource>().clip = SFXManager.instance.clips[15]; //brrrr
                    attachedObject.GetComponent<AudioSource>().loop = true;

                    if (!attachedObject.GetComponent<AudioSource>().isPlaying)// && Vector3.Distance(GameManager.instance.player))
                    {
                        attachedObject.GetComponent<AudioSource>().Play();
                    }
                    timePassed = timeInBetweenBeeps;
                }
            }
        }

        
    }
}
