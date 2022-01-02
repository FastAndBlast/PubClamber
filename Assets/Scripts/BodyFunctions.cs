using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum GestureType { Wave, FlipOff };

public class BodyFunctions : MonoBehaviour
{
    public static BodyFunctions instance;

    public float blinkBlurTime;
    public float blinkBlindTime;
    //private float timeSinceLeftBlink;
    //private float timeSinceRightBlink;
    private float timeSinceBlink;

    private bool eyesClosed = false;
    private float blinkTimer;


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

    public bool gesturesEnabled;
    public GestureType gesture;
    public float gestureTime;

    public GameObject blurVolume;
    public GameObject blinder;

    //public GameObject rightBlurVolume;
    //public GameObject leftBlurVolume;
    //public GameObject leftBlinder;
    //public GameObject rightBlinder;

    //public SFXManager sfx;

    // Start is called before the first frame update
    void Start()
    {
        ResetValues();


        if (!blinder)
        {
            blinder = GameObject.FindWithTag("CameraParent").transform.Find("Camera").Find("Blinder").gameObject;
            blurVolume = GameObject.FindWithTag("CameraParent").transform.Find("Camera").Find("Blur").gameObject;
        }
        

        /*
        if (!leftBlinder)
        {
            leftBlinder = GameObject.FindWithTag("CameraParent").transform.Find("LeftCamera").Find("Blinder").gameObject;
            leftBlurVolume = GameObject.FindWithTag("CameraParent").transform.Find("LeftCamera").Find("LeftBlur").gameObject;
            rightBlinder = GameObject.FindWithTag("CameraParent").transform.Find("RightCamera").Find("Blinder").gameObject;
            rightBlurVolume = GameObject.FindWithTag("CameraParent").transform.Find("RightCamera").Find("RightBlur").gameObject;
        }
        */
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
        if (!GameManager.paused && !UIManager.instance.deathMenu.gameObject.activeInHierarchy && !(GameManager.instance.deathDelay > 0))
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
            //timeSinceLeftBlink += dtime;
            //timeSinceRightBlink += dtime;
            timeSinceBlink += dtime;

            if (blinkingEasyMode && blinkTimer > 0)
            {
                blinkTimer -= dtime;
                if (blinkTimer <= 0)
                {
                    eyesClosed = false;
                }
            }
        }

        if (gesturesEnabled && gestureTime > 0)
        {
            gestureTime -= dtime;
            if (gesture == GestureType.FlipOff)
            {
                transform.Find("Canvas").Find("FlipOff").gameObject.SetActive(true);
                transform.Find("Canvas").Find("Wave").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("Canvas").Find("FlipOff").gameObject.SetActive(false);
                transform.Find("Canvas").Find("Wave").gameObject.SetActive(true);
            }
        }
        else
        {
            transform.Find("Canvas").Find("FlipOff").gameObject.SetActive(false);
            transform.Find("Canvas").Find("Wave").gameObject.SetActive(false);
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
            // breathing in
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
        /*
        GameObject leftCamera = GameObject.FindWithTag("CameraParent").transform.Find("LeftCamera").gameObject;

        if (leftCamera.GetComponent<Volume>().profile.TryGet<Vignette>(out var vignette))
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = airInLungs / lungCapacity;
        }
        */

        // TODO ADD warnings here
        if (airInLungs < warningAirFrac * lungCapacity)
        {
            int chokingIndex = Random.Range(9, 11);
            SFXManager.instance.PlaySFX(chokingIndex);
        }

        if (airInLungs < 0)
        {
            Die("Asphixiated to death", "Tip: Hold E to breath");
            print("Asphixiated to death");
            return;
        }

        if (currentStenchLevel > stenchCap)
        {
            Die("Passed out from stench", "Tip: Hold SPACE to hold your nose");
            print("Passed out from stench");
            return;
        }

        if (timeSinceBurpNeeded > maxTimeSinceBurp)
        {
            Die("Passed out from not burping", "Tip: Press V to burp");
            print("Passed out from not burping");
            return;
        }
        else
        {
            if (timeSinceBurpNeeded > 0)
            {
                int hiccupIndex = Random.Range(12, 14);
                SFXManager.instance.PlaySFX(hiccupIndex);
            }
        }
    }

    void BlurEyes()
    {
        //bool leftBlurred = false;
        //bool rightBlurred = false;
        //bool leftBlind = false;
        //bool rightBlind = false;
        bool blurred = false;
        bool blind = false;

        if (timeSinceBlink > blinkBlindTime || eyesClosed)
        {
            blind = true;
        }
        else
        {
            if (timeSinceBlink > blinkBlurTime)
            {
                blurred = true;
            }
        }

        blinder.SetActive(blind);
        blurVolume.SetActive(blurred);

        /*
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
        */
    }

    void CheckInputs()
    {
        if (Input.GetButtonDown("CloseEyes"))
        {
            eyesClosed = true;
            if (blinkingEasyMode)
            {
                blinkTimer = 0.3f;
            }
        }

        if (eyesClosed)
        {
            timeSinceBlink = 0;
        }

        if (Input.GetButtonDown("OpenEyes") && !blinkingEasyMode)
        {
            eyesClosed = false;
        }
        /*
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
        */

        if (Input.GetButtonDown("Wave"))
        {
            if (gesturesEnabled)
            {
                gesture = GestureType.Wave;
                gestureTime = 1.5f;
            }
        }

        if (Input.GetButtonDown("FlipOff"))
        {
            if (gesturesEnabled)
            {
                gesture = GestureType.FlipOff;
                gestureTime = 1.5f;
                if (noseHeld)
                {
                    if (GameManager.instance.profanity)
                    {
                        SFXManager.instance.PlaySFX(Random.Range(34, 37));
                    }
                    else
                    {
                        SFXManager.instance.PlaySFX(Random.Range(37, 40));
                    }
                }
                else
                {
                    if (GameManager.instance.profanity)
                    {
                        SFXManager.instance.PlaySFX(Random.Range(28, 31));
                    }
                    else
                    {
                        SFXManager.instance.PlaySFX(Random.Range(31, 34));
                    }
                }
            }
        }

        if (Input.GetButtonDown("Burp"))
        {
            if (timeSinceBurpNeeded > 0)
            {
                int burpIndex = Random.Range(0, 2);
                SFXManager.instance.PlaySFX(burpIndex);
                timeSinceBurpNeeded = Random.Range(-burpMaxTimeBetween , -burpMinTimeBetween);
            }
            else
            {
                // could put a punishment for spamming here
            }
        }

        if (Input.GetButton("BreatheIn"))
        {
            if (breathingEnabled)
            {
                int breatheInIndex = Random.Range(6, 8);
                SFXManager.instance.PlaySFX(breatheInIndex);
                breathingState = 1;
            }
        }
        else 
        {
            if (Input.GetButton("BreatheOut"))
            {
                if (breathingEnabled)
                {
                    int breatheOutIndex = Random.Range(3, 5);
                SFXManager.instance.PlaySFX(breatheOutIndex);
                breathingState = -1;
                }
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

    public void ResetValues()
    {
        currentStenchLevel = 0;

        airInLungs = lungCapacity;
        CO2InLungs = 0;

        //timeSinceLeftBlink = 0;
        //timeSinceRightBlink = 0;
        timeSinceBlink = 0;
        eyesClosed = false;

        timeSinceBurpNeeded = -burpMaxTimeBetween;
    }

    public void Die(string causeOfDeath, string tip)
    {
        //print("Died");
        //print(causeOfDeath);
        if (GetComponent<WalkingManager>())
        {
            GetComponent<WalkingManager>().Die(causeOfDeath, tip);
        }
    }
}
