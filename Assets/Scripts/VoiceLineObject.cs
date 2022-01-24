using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineObject : MonoBehaviour
{
    //Dictionary<AudioClip, int> dict;

    public List<AudioClip> clips;

    //public List<int> timeSignatures;

    //float time = 0;

    public GameObject audioSourcePrefab;

    //List<AudioClip> playedClips = new List<AudioClip>();


    public float delay = 0f;

    void Start()
    {
        if (delay > 0)
        {
            return;
        }

        if (GameManager.instance.profanity)
        {
            PlayVoiceline(clips[0]);
        }
        else
        {
            PlayVoiceline(clips[1]);
        }

        /*
        for (int i = 0; i < clips.Count; i++)
        {
            if (timeSignatures[i] == 0)
            {
                PlayVoiceline(clips[i]);
            }
            
        }
        */
    }

    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
            {
                if (GameManager.instance.profanity)
                {
                    PlayVoiceline(clips[0]);
                }
                else
                {
                    PlayVoiceline(clips[1]);
                }
            }
        }
    }

    //private void Update()
    //{
    /*
    time += Time.deltaTime;

    for (int i = 0; i < clips.Count; i++)
    {
        if (!playedClips.Contains(clips[i]) && timeSignatures[i] < time)
        {
            PlayVoiceline(clips[i]);
        }
    }
    */
    //}

    public void PlayVoiceline(AudioClip clip)
    {
        GameObject sourceInstance = Instantiate(audioSourcePrefab);

        sourceInstance.GetComponent<AudioSource>().clip = clip;

        sourceInstance.GetComponent<DestroyTimer>().time = clip.length + 0.1f;

        sourceInstance.GetComponent<AudioSource>().Play();

        sourceInstance.transform.parent = Camera.main.transform;
        sourceInstance.transform.localPosition = Vector3.zero;
        //playedClips.Add(clip);
    }
}
