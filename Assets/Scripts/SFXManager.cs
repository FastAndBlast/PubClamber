using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public List<AudioClip> clips;

    [SerializeField]
    GameObject audioSourcePrefab;

    public static SFXManager instance;

    public List<int> playingClips;

    public List<float> clipVolume;

    public void Awake()
    {
        instance = this;
    }

    public void PlaySFX(int index)
    {
        foreach (int x in playingClips)
        {
            if (x == index)
            {
                return;
            }
        }

        playingClips.Add(index);

        //print("SFX: " + index.ToString());

        GameObject sourceInstance = Instantiate(audioSourcePrefab);

        sourceInstance.GetComponent<AudioSource>().clip = clips[index];

        sourceInstance.GetComponent<AudioSource>().volume *= clipVolume[index];

        sourceInstance.GetComponent<DestroyTimer>().time = clips[index].length + 0.1f;

        sourceInstance.GetComponent<AudioSource>().Play();

        sourceInstance.GetComponent<AudioObject>().index = index;

        sourceInstance.transform.parent = Camera.main.transform;
        sourceInstance.transform.localPosition = Vector3.zero;
    }


}
