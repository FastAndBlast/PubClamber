using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
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
        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
    }

    public void Profanity()
    {
        GameManager.instance.profanity = !GameManager.instance.profanity;
    }
}
