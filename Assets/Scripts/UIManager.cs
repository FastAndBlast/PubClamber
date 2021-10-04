using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    bool paused;

    public Transform darkPanel;
    public Transform pauseMenu;
    public Transform deathMenu;

    public List<Sprite> buttonIcons;

    public bool muted
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
    }

    private void Start()
    {
        Transform canvas = GameObject.FindWithTag("MainCanvas").transform;
        pauseMenu = canvas.Find("PauseMenu");
        pauseMenu = canvas.Find("DeathMenu");
        darkPanel = canvas.Find("DarkPanel");

        //Update Profanity UI & Mute UI

        UpdateIcons();
    }

    public void Death(string causeOfDeath)
    {
        deathMenu.gameObject.SetActive(true);
        deathMenu.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = causeOfDeath;

        UpdateIcons();
    }

    public void Pause()
    {
        paused = !paused;
        GameManager.instance.Pause();

        //Close / Open pause menu
        pauseMenu.gameObject.SetActive(paused);

        UpdateIcons();
        //Menu.Find("ButtonPanel").Find("Profanity").GetComponent<Image>().sprite = buttonIcons[index];
        
    }

    public void UpdateIcons()
    {
        int profanityIndex = GameManager.instance.profanity ? 2 : 3;
        int mutedIndex = muted ? 0 : 1;

        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.Find("ButtonPanel").Find("Profanity").GetComponent<Image>().sprite = buttonIcons[profanityIndex];
            pauseMenu.Find("ButtonPanel").Find("Mute").GetComponent<Image>().sprite = buttonIcons[mutedIndex];
        }
        else if (deathMenu.gameObject.activeSelf)
        {
            deathMenu.Find("ButtonPanel").Find("Profanity").GetComponent<Image>().sprite = buttonIcons[profanityIndex];
            deathMenu.Find("ButtonPanel").Find("Mute").GetComponent<Image>().sprite = buttonIcons[mutedIndex];
        }
    }

    public void LevelSelect()
    {
        SceneMaster.instance.ChangeScene(SceneMaster.levelSelectScene);
    }

    public void Profanity()
    {
        GameManager.instance.profanity = !GameManager.instance.profanity;
        //TODO: Update profanity UI

        UpdateIcons();
    }

    public void Restart()
    {
        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.gameObject.SetActive(false);
        }
        else if (deathMenu.gameObject.activeSelf)
        {
            deathMenu.gameObject.SetActive(false);
        }
        GameManager.instance.SpawnPlayer();
    }

    public void Mute()
    {
        muted = !muted;
        UpdateIcons();
    }


}
