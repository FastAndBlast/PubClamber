using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
    public static int mainMenuScene = 0;
    public static int levelSelectScene = 1;
    public static int firstLevelScene = 2;
    public static int lastLevelScene = 8;

    //public static int mainGameScene = 2;
    //public int endCreditScene;

    public static SceneMaster instance;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeScene(int newScene)
    {
        SceneManager.LoadScene(newScene);
    }
}
