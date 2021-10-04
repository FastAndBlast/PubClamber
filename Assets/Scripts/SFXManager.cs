using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public List<AudioClip> clips;

    [SerializeField]
    GameObject audioSourcePrefab;

    public static SFXManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void PlaySFX(int index)
    {
        GameObject sourceInstance = Instantiate(audioSourcePrefab);

        sourceInstance.GetComponent<AudioSource>().clip = clips[index];

        sourceInstance.GetComponent<DestroyTimer>().time = clips[index].length + 0.1f;

        sourceInstance.GetComponent<AudioSource>().Play();

        sourceInstance.transform.parent = Camera.main.transform;
        sourceInstance.transform.localPosition = Vector3.zero;
    }


}
