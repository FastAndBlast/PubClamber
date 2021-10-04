using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public List<Sprite> buttonIcons;

    public List<AudioClip> clips;

    public void Play()
    {
        SceneMaster.instance.ChangeScene(SceneMaster.firstLevelScene);
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        SceneMaster.instance.ChangeScene(SceneMaster.levelSelectScene);
    }

    public void Mute()
    {
        GameManager.instance.Mute();

        Transform canvas = GameObject.FindWithTag("MainCanvas").transform;

        int mutedIndex = GameManager.muted ? 0 : 1;

        canvas.GetChild(0).Find("Mute").GetComponent<Image>().sprite = buttonIcons[mutedIndex];
    }

    public void Profanity()
    {
        GameManager.instance.profanity = !GameManager.instance.profanity;

        Transform canvas = GameObject.FindWithTag("MainCanvas").transform;

        int profanityIndex = GameManager.instance.profanity ? 2 : 3;

        canvas.GetChild(0).Find("Profanity").GetComponent<Image>().sprite = buttonIcons[profanityIndex];
        
        if (GameManager.instance.profanity)
        {
            GetComponent<AudioSource>().clip = clips[Random.Range(0, 3)];
            GetComponent<AudioSource>().Play();
        }
    }
}
