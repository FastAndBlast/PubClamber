using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip start;
    public List<AudioClip> loops;

    [SerializeField]
    GameObject audioSourcePrefab;

    public static MusicManager instance;

    float loopTimer = 4f;

    public void Awake()
    {
        //start is 5 seconds long
        //play 4 seconds before the previous
        //loops are ~40 seconds long
        instance = this;
    }

    public void Start()
    {
        GameObject sourceInstance = Instantiate(audioSourcePrefab);

        sourceInstance.GetComponent<AudioSource>().clip = start;
        sourceInstance.GetComponent<AudioSource>().Play();
        sourceInstance.GetComponent<DestroyTimer>().time = loops[GameManager.instance.level].length + 0.1f;

        sourceInstance.transform.parent = Camera.main.transform;
        sourceInstance.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (loopTimer > 0)
        {
            loopTimer -= Time.deltaTime;
        }
        else
        {
            GameObject sourceInstance = Instantiate(audioSourcePrefab);

            sourceInstance.GetComponent<AudioSource>().clip = loops[GameManager.instance.level];
            sourceInstance.GetComponent<AudioSource>().Play();
            sourceInstance.GetComponent<DestroyTimer>().time = loops[GameManager.instance.level].length + 0.1f;
            

            sourceInstance.transform.parent = Camera.main.transform;
            sourceInstance.transform.localPosition = Vector3.zero;

            loopTimer = loops[GameManager.instance.level].length - 4f;
        }
    }
}
