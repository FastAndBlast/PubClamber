using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool paused;

    public GameObject player;

    public int level;

    public static GameManager instance;

    public bool profanity = false;

    public List<GameObject> playerCharacterPrefabs;

    //public Transform spawnPoint;

    public static bool muted;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
        }
    }

    public void PlayerDeath(string causeOfDeath)
    {

    }

    public void SpawnPlayer()
    {
        if (player)
        {
            Destroy(player);
        }

        Transform spawnPoint = GameObject.FindWithTag("SpawnPoint").transform;

        GameObject playerInstance = Instantiate(playerCharacterPrefabs[level]);

        playerInstance.transform.position = spawnPoint.position;

        player = playerInstance;

        Camera.main.GetComponent<CameraController>().target = player.transform;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex  > 1 && scene.buildIndex < 7)
        {
            SpawnPlayer();
        }

        AudioListener.volume = muted ? 0 : 1;
    }

    public void Mute()
    {
        muted = !muted;

        if (muted)
        {
            AudioListener.volume = 0;
        }
        else if (muted)
        {
            AudioListener.volume = 1;
        }
    }

    public void Pause()
    {
        paused = !paused;
    }
}
