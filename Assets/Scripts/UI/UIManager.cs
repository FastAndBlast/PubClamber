using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    bool paused;

    public Transform fadePanel;

    public Transform darkPanel;

    public Transform greenPanel;

    public Transform pauseMenu;
    public Transform deathMenu;

    public List<Sprite> buttonIcons;


    /*public bool muted
    {
        get
        {
            return !(AudioListener.volume > 0);
        }
        set
        {
            if (value)
            {
                AudioListener.volume = 0;
            }
            else
            {
                AudioListener.volume = 1;
            }
        }
    }*/

    public static UIManager instance;

    public float fade = 1;

    bool fading;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Transform canvas = GameObject.FindWithTag("MainCanvas").transform;
        pauseMenu = canvas.Find("PauseMenu");
        deathMenu = canvas.Find("DeathMenu");
        darkPanel = canvas.Find("DarkPanel");
        fadePanel = canvas.Find("FadePanel");
        greenPanel = canvas.Find("GreenPanel");

        //Update Profanity UI & Mute UI

        UpdateIcons();

        FadeFromBlack();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        if (fading)
        {
            fade += Time.deltaTime;
        }
        else
        {
            fade -= Time.deltaTime;

            if (fade < 1)
            {
                GameManager.instance.OnFadedBack();
            }
        }
        Color col = fadePanel.GetComponent<Image>().color;
        col.a = fade;
        fadePanel.GetComponent<Image>().color = col;

        if (paused)
        {
            greenPanel.gameObject.SetActive(false);
        }
        else
        {
            Color greenCol = greenPanel.GetComponent<Image>().color;

            greenCol.a = (BodyFunctions.instance.currentStenchLevel / BodyFunctions.instance.stenchCap) * 0.3f;

            greenPanel.gameObject.SetActive(true);
            greenPanel.GetComponent<Image>().color = greenCol;
        }

    }

    public void FadeToBlack(float newFade=0)
    {
        fade = newFade;
        fading = true;
    }

    public void FadeFromBlack()
    {
        //fade = newFade;
        fading = false;
    }

    public void Death(string causeOfDeath, string tip)
    {
        deathMenu.gameObject.SetActive(true);
        deathMenu.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = causeOfDeath;
        deathMenu.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = tip;

        UpdateIcons();
    }

    public void Pause()
    {
        if (!deathMenu.gameObject.activeInHierarchy)
        {
            paused = !paused;
            GameManager.instance.Pause();

            //Close / Open pause menu
            pauseMenu.gameObject.SetActive(paused);

            UpdateIcons();
        }
        //Menu.Find("ButtonPanel").Find("Profanity").GetComponent<Image>().sprite = buttonIcons[index];
        
    }

    public void UpdateIcons()
    {
        if (paused || deathMenu.gameObject.activeInHierarchy)
        {
            darkPanel.gameObject.SetActive(true);
        }
        else
        {
            darkPanel.gameObject.SetActive(false);
        }

        int profanityIndex = GameManager.instance.profanity ? 2 : 3;
        int mutedIndex = GameManager.muted ? 0 : 1;

        pauseMenu.Find("ButtonPanel").Find("Profanity").GetComponent<Image>().sprite = buttonIcons[profanityIndex];
        pauseMenu.Find("ButtonPanel").Find("Mute").GetComponent<Image>().sprite = buttonIcons[mutedIndex];
        deathMenu.Find("ButtonPanel").Find("Profanity").GetComponent<Image>().sprite = buttonIcons[profanityIndex];
        deathMenu.Find("ButtonPanel").Find("Mute").GetComponent<Image>().sprite = buttonIcons[mutedIndex];

        int x = 0;
        foreach (Transform helpTransform in pauseMenu.Find("HelpPanel"))
        {
            if (x == GameManager.instance.level - 1)
            {
                helpTransform.gameObject.SetActive(true);
            }
            else
            {
                helpTransform.gameObject.SetActive(false);
            }
            x++;
        }

        x = 0;
        foreach (Transform helpTransform in deathMenu.Find("HelpPanel"))
        {
            if (x == GameManager.instance.level - 1)
            {
                helpTransform.gameObject.SetActive(true);
            }
            else
            {
                helpTransform.gameObject.SetActive(false);
            }
            x++;
        }
    }

    public void LevelSelect()
    {
        SceneMaster.instance.ChangeScene(SceneMaster.mainMenuScene);
    }

    public void Profanity()
    {
        GameManager.instance.profanity = !GameManager.instance.profanity;
        //TODO: Update profanity UI

        if (GameManager.instance.profanity)
        {
            SFXManager.instance.PlaySFX(Random.Range(25, 28));
        }

        UpdateIcons();
    }

    public void Restart()
    {
        pauseMenu.gameObject.SetActive(false);
        deathMenu.gameObject.SetActive(false);
        GameManager.instance.Restart();
        //print("call");

        UpdateIcons();
    }

    public void Mute()
    {
        GameManager.instance.Mute();
        UpdateIcons();
    }


}
