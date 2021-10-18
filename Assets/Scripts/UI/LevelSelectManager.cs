using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    private List<GameObject> levels = new List<GameObject>();

    public Color textEnabled;
    public Color textDisabled;

    public GameObject canvas;

    private void Awake()
    {
        //canvas = GameObject.FindWithTag("MainCanvas");

        foreach (PopOutUIEffect effect in canvas.GetComponentsInChildren<PopOutUIEffect>())
        {
            levels.Add(effect.gameObject);
        }

        for (int i = 0; i < GameManager.instance.maxLevel + 1; i++)
        {
            levels[i].GetComponent<PopOutUIEffect>().on = true;
            levels[i].GetComponentInChildren<TextMeshProUGUI>().color = textEnabled;
        }
    }

    public void SelectLevel(int level)
    {
        GameManager.instance.level = level;
        SceneMaster.instance.ChangeScene(SceneMaster.firstLevelScene + level);
    }

    public void MainMenu()
    {
        SceneMaster.instance.ChangeScene(SceneMaster.mainMenuScene);
    }
}
