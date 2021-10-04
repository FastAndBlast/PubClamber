using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip start;
    public List<AudioClip> loops;

    public AudioClip menuMusic;

    [SerializeField]
    GameObject audioSourcePrefab;

    public static MusicManager instance;

    float loopTimer = 5f;

    float[] voiceLineLength = new float[6] { 7.6f, 8.5f, 4.5f, 6.6f, 8.1f, 6.7f };

    public void Awake()
    {
        //start is 1 seconds long
        //play 4 seconds before the previous
        //loops are ~40 seconds long
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Start()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (loopTimer > 0)
        {
            loopTimer -= Time.deltaTime;
        }
        else
        {
            AudioClip playedClip = loops[GameManager.instance.level];

            if (SceneManager.GetActiveScene().buildIndex == SceneMaster.levelSelectScene || SceneManager.GetActiveScene().buildIndex == SceneMaster.mainMenuScene)
            {
                playedClip = menuMusic;
            }
            GameObject sourceInstance = Instantiate(audioSourcePrefab);

            sourceInstance.GetComponent<AudioSource>().clip = playedClip;
            sourceInstance.GetComponent<AudioSource>().Play();
            sourceInstance.GetComponent<DestroyTimer>().time = playedClip.length + 0.1f;

            sourceInstance.transform.parent = Camera.main.transform;
            sourceInstance.transform.localPosition = Vector3.zero;

            loopTimer = loops[GameManager.instance.level].length - 4f;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loopTimer = 1f;
        if (scene.buildIndex >= SceneMaster.firstLevelScene && scene.buildIndex < SceneMaster.lastLevelScene)
        {
            loopTimer += voiceLineLength[GameManager.instance.level];

            GameObject sourceInstance = Instantiate(audioSourcePrefab);

            sourceInstance.GetComponent<AudioSource>().clip = start;
            sourceInstance.GetComponent<AudioSource>().PlayDelayed(loopTimer - 1);
            sourceInstance.GetComponent<DestroyTimer>().time = loops[GameManager.instance.level].length + 0.1f;

            sourceInstance.transform.parent = Camera.main.transform;
            sourceInstance.transform.localPosition = Vector3.zero;

            
        }
    }
}
