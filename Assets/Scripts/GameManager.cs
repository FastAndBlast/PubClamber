using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool paused;

    public GameObject player;

    public int level;
    public int maxLevel
    {
        get
        {
            return PlayerPrefs.GetInt("MaxLevel", 0);
        }
        set
        {
            PlayerPrefs.SetInt("MaxLevel", value);
        }
    }

    public static GameManager instance;

    public bool profanity = false;

    public List<GameObject> playerCharacterPrefabs;

    //public Transform spawnPoint;

    public static bool muted;

    float endOfLevelTimerMax = 5f;
    float endOfLevelTimer = 0;
    bool endOfLevel = false;

    [HideInInspector]
    public float deathDelay = -1;

    string lastCauseOfDeath;
    string lastTip = "Tip: Git good";

    public bool fadedBackPing = true;

    private Transform spawnPoint;

    GameObject mainCameraParent
    {
        get
        {
            return GameObject.FindWithTag("CameraParent");
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (endOfLevel)
        {
            if (endOfLevelTimer > 0)
            {
                endOfLevelTimer -= Time.deltaTime;
            }
            else
            {
                //Switch to next level
                if (level < SceneMaster.lastLevelScene)
                {
                    level++;
                    if (level > maxLevel)
                    {
                        maxLevel = level;
                    }

                    SceneMaster.instance.ChangeScene(SceneMaster.firstLevelScene + level);
                }
                else
                {
                    SceneMaster.instance.ChangeScene(SceneMaster.mainMenuScene); //TODO change to credit scene
                }
            }
            if (endOfLevelTimer < 1)
            {
                UIManager.instance.FadeToBlack();
            }
        }

        if (deathDelay > 0)
        {
            deathDelay -= Time.deltaTime;
            if (deathDelay < 0)
            {
                UIManager.instance.Death(lastCauseOfDeath, lastTip);
                deathDelay = -1;
            }
        }
    }

    public void Checkpoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }


    public void PlayerDeath(string causeOfDeath, string tip)
    {
        //UIManager.instance.Death(causeOfDeath);
        if (deathDelay <= 0)
        {
            deathDelay = 0.5f;
            lastCauseOfDeath = causeOfDeath;
            lastTip = tip;
        }
    }

    public void EndOfLevel()
    {
        // Wait 2 seconds for zoom in
        // Fade to black
        // -------------
        // Start level at black
        // Play voice line at start of level
        endOfLevel = true;
        endOfLevelTimer = endOfLevelTimerMax;
        paused = true;

        mainCameraParent.GetComponent<CameraController>().FocusSign(GameObject.FindWithTag("Sign").transform.GetChild(0));
    }

    public void Restart()
    {
        foreach (Enemy enemy in Enemy.enemyList)
        {
            enemy.Restart();
        }
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (player)
        {
            Destroy(player);
        }

        GameObject playerInstance = Instantiate(playerCharacterPrefabs[level]);

        playerInstance.transform.position = spawnPoint.position;

        player = playerInstance;

        mainCameraParent.GetComponent<CameraController>().target = player.transform;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawnPoint = GameObject.FindWithTag("SpawnPoint").transform;

        Enemy.enemyList = new List<Enemy>();
        endOfLevel = false;
        if (scene.buildIndex == SceneMaster.mainMenuScene || scene.buildIndex == SceneMaster.levelSelectScene)
        {
            paused = false;
            fadedBackPing = true;
        }

        if (scene.buildIndex >= SceneMaster.firstLevelScene && scene.buildIndex <= SceneMaster.lastLevelScene)
        {
            fadedBackPing = true;
            SpawnPlayer();
            paused = true;
        }
        AudioListener.volume = muted ? 0 : 1;
    }

    public void OnFadedBack()
    {
        if (fadedBackPing)
        {
            paused = false;
            fadedBackPing = false;
        }
    }

    public void Mute()
    {
        muted = !muted;

        if (muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void Pause()
    {
        paused = !paused;
    }
}
