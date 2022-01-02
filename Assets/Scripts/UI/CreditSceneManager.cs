using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditSceneManager : MonoBehaviour
{
    public Transform scrollTransform;

    public GameObject audioSourcePrefab;

    public GameObject skipButton;

    public AudioClip voiceLine;
    public AudioClip crashSoundeffect;

    float time = 15;

    public float targetY = 500;

    private void Start()
    {
        GameObject audioSource = Instantiate(audioSourcePrefab);

        audioSource.GetComponent<AudioSource>().clip = voiceLine;
        audioSource.GetComponent<AudioSource>().Play();
        audioSource.GetComponent<DestroyTimer>().enabled = false;

        if (crashSoundeffect)
        {
            GameObject audioSource2 = Instantiate(audioSourcePrefab);

            audioSource.GetComponent<AudioSource>().clip = crashSoundeffect;
            audioSource.GetComponent<AudioSource>().PlayDelayed(voiceLine.length);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > voiceLine.length)
        {
            float newY = Mathf.MoveTowards(scrollTransform.localPosition.y, targetY, Time.deltaTime * 25);

            scrollTransform.localPosition = new Vector3(scrollTransform.localPosition.x, newY, scrollTransform.localPosition.z);
        }

        if (Input.anyKeyDown)
        {
            skipButton.SetActive(true);
        }
    }

    public void Exit()
    {
        SceneMaster.instance.ChangeScene(0);
    }
}
