using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BodyFunctions : MonoBehaviour
{
    public static BodyFunctions instance;

    public float blinkBlurTime;
    public float blinkBlindTime;
    private float timeSinceLeftBlink;
    private float timeSinceRightBlink;

    public float lungCapacity;
    public float airConsumptionRate;
    public float airIntakeRate;
    public float breathOutRate;
    private float airInLungs;
    private float CO2InLungs;
    public float warningAirFrac;
    public float easyModeCoeff;
    private int breathingState; // -1 out 0 none 1 in

    public float stenchCap;
    public float currentStenchLevel;
    public float stenchDecay;
    public float nearbyStench;
    public float heldCoefficient;

    public float burpMaxTimeBetween;
    public float burpMinTimeBetween;
    public float timeSinceBurpNeeded;
    public float maxTimeSinceBurp;

    public bool blinkingEnabled;
    public bool breathingEnabled;
    public bool stenchEnabled;
    public bool burpingEnabled;
    public bool breathingEasyMode;
    public bool blinkingEasyMode;
    public bool noseHeld;

    public GameObject rightBlurVolume;
    public GameObject leftBlurVolume;
    public GameObject leftBlinder;
    public GameObject rightBlinder;

    public SFXManager sfx;

    // Start is called before the first frame update
    void Start()
    {
        airInLungs = lungCapacity;
        CO2InLungs = 0;

        timeSinceLeftBlink = 0;
        timeSinceRightBlink = 0;
    }

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // This is updated in LateUpdate
        nearbyStench = 0;
    }
    
    void LateUpdate()
    // This needs to run after StenchSource or stench is always 0
    {
        if (!GameManager.paused)
        {
            CheckInputs();
            UpdateQuantities();
            CheckCaps();

            if (blinkingEnabled)
            {
                BlurEyes();
            }
        }
    }

    void UpdateQuantities()
    // apply all of our rates
    {
        float dtime = Time.deltaTime;

        if (blinkingEnabled)
        {
            timeSinceLeftBlink += dtime;
            timeSinceRightBlink += dtime;
        }

        if (burpingEnabled)
        {
            timeSinceBurpNeeded += dtime;
        }

        if (breathingEnabled)
        {
            airInLungs -= airConsumptionRate * dtime;
            CO2InLungs += airConsumptionRate * dtime;
            if (breathingState == 1)
            // bretahing in
            {
                if (breathingEasyMode)
                {
                    if (airInLungs < lungCapacity - CO2InLungs)
                    {
                        airInLungs += Mathf.Min(airIntakeRate * dtime, airInLungs);
                        CO2InLungs += airIntakeRate * easyModeCoeff * dtime;
                    }
                    CO2InLungs = CO2InLungs = Mathf.Max(CO2InLungs - breathOutRate * dtime, 0);
                }
                else
                {
                    if (airInLungs + CO2InLungs < lungCapacity)
                    {
                        airInLungs = Mathf.Min(airInLungs + airIntakeRate, lungCapacity - CO2InLungs);
                    }
                }
            }
            else
            {
                if (breathingState == -1)
                {
                    float airFrac = airInLungs / (airInLungs + CO2InLungs);
                    float CO2Frac = CO2InLungs / (airInLungs + CO2InLungs);

                    airInLungs -= airFrac * breathOutRate * dtime;
                    CO2InLungs = Mathf.Max(CO2InLungs - CO2Frac * breathOutRate * dtime, 0);
                }
            }
        }

        if (noseHeld)
        {
            currentStenchLevel -= Mathf.Max(stenchDecay * heldCoefficient * dtime, 0);
        }
        else
        {
            currentStenchLevel += nearbyStench * dtime;
            currentStenchLevel -= Mathf.Max(stenchDecay * dtime, 0);
        }

    }

    void CheckCaps()
    {
        // TODO ADD warnings here
        if (airInLungs < warningAirFrac * lungCapacity)
        {
            int chokingIndex = Random.Range(9, 11);
            sfx.PlaySFX(chokingIndex);
        }

        if (airInLungs < 0)
        {
            Die("Asphixiated to death");
            print("Asphixiated to death");
            return;
        }

        if (currentStenchLevel > stenchCap)
        {
            Die("Passed out from stench");
            print("Passed out from stench");
            return;
        }

        if (timeSinceBurpNeeded > maxTimeSinceBurp)
        {
            Die("Passed out from not burping");
            print("Passed out from not burping");
            return;
        }
        else
        {
            if (timeSinceBurpNeeded > 0)
            {
                int hiccupIndex = Random.Range(12, 14);
                sfx.PlaySFX(hiccupIndex);
            }
        }
    }

    void BlurEyes()
    {
        bool leftBlurred = false;
        bool rightBlurred = false;
        bool leftBlind = false;
        bool rightBlind = false;

        if (timeSinceLeftBlink > blinkBlindTime)
        {
            leftBlind = true;
        }
        else
        {
            leftBlind = false;
            if (timeSinceLeftBlink > blinkBlurTime)
            {
                // Blur left eye
                leftBlurred = true;
            }
            else
            {
                leftBlurred = false;
            }
        }

        if (timeSinceRightBlink > blinkBlindTime)
        {
            rightBlind = true;
        }
        else
        {
            rightBlind = false;
            if (timeSinceRightBlink > blinkBlurTime)
            {
                // Blur right eye
                rightBlurred = true;
            }
            else
            {
                rightBlurred = false;
            }
        }

        if (blinkingEasyMode)
        // If easy mode ignore the right eye
        {
            rightBlurred = leftBlurred;
            rightBlind = leftBlind;
        }

        leftBlinder.SetActive(leftBlind);
        leftBlurVolume.SetActive(leftBlurred);
        rightBlinder.SetActive(rightBlind);
        rightBlurVolume.SetActive(rightBlurred);
    }

    void CheckInputs()
    {
        if (Input.GetButtonDown("LeftBlink"))
        {
            timeSinceLeftBlink = 0;
        }

        if (!blinkingEasyMode)
        {
            if (Input.GetButtonDown("RightBlink"))
            {
                timeSinceRightBlink = 0;
            }
        }

        if (Input.GetButtonDown("Burp"))
        {
            if (timeSinceBurpNeeded > 0)
            {
                int burpIndex = Random.Range(0, 2);
                sfx.PlaySFX(burpIndex);
                timeSinceBurpNeeded = Random.Range(-burpMaxTimeBetween , -burpMinTimeBetween);
            }
            else
            {
                // could put a punishment for spamming here
            }
        }

        if (Input.GetButton("BreatheIn"))
        {
            int breatheInIndex = Random.Range(6, 8);
            sfx.PlaySFX(breatheInIndex);
            breathingState = 1;
        }
        else 
        {
            if (Input.GetButton("BreatheOut"))
            {
                int breatheOutIndex = Random.Range(3, 5);
                sfx.PlaySFX(breatheOutIndex);
                breathingState = -1;
            }
            else
            {
                breathingState = 0;
            }
        }

        if (Input.GetButton("HoldNose"))
        {
            noseHeld = true;
            // Can't breathe while nose held
            breathingState = 0;
        }
        else
        {
            noseHeld = false;
        }
    }

    public void Die(string causeOfDeath)
    {
        if (GetComponent<Player>())
        {
            GetComponent<Player>().Die(causeOfDeath);
        }
    }
}
