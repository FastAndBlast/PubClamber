using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CreditSceneManager : MonoBehaviour
{
    public Transform scrollTransform;

    public GameObject audioSourcePrefab;

    public GameObject skipButton;

    public AudioClip voiceLine;
    public AudioClip crashSoundeffect;

    VideoPlayer cutScene;

    float time = 0; //15

    public float targetY = 500;

    float scroll = 0f;

    private void Start()
    {
        //GameObject audioSource = Instantiate(audioSourcePrefab);

        //audioSource.GetComponent<AudioSource>().clip = voiceLine;
        //audioSource.GetComponent<AudioSource>().Play();
        //audioSource.GetComponent<DestroyTimer>().enabled = false;

        if (crashSoundeffect)
        {
            GameObject audioSource2 = Instantiate(audioSourcePrefab);

            //audioSource.GetComponent<AudioSource>().clip = crashSoundeffect;
            //audioSource.GetComponent<AudioSource>().PlayDelayed(voiceLine.length);
        }

        cutScene = scrollTransform.parent.Find("CutScene").GetComponent<VideoPlayer>();
    }

    void Update()
    {
        time += Time.deltaTime;

        //Mathf.MoveTowards(scrollTransform.localPosition.y, targetY, Input.mouseScrollDelta.y * 100);

        //print(Input.mouseScrollDelta.y);
        
        scroll = Mathf.MoveTowards(scroll, 0, Time.deltaTime);

        if (Input.mouseScrollDelta.y != 0)
        {
            scroll = Input.mouseScrollDelta.y;
            scroll /= Mathf.Abs(scroll);
            scroll *= 0.5f;

            //print(scroll);
        }

        if (time > cutScene.length - 2f)//voiceLine.length)
        {
            //scrollTransform.parent.Find("CutScene").gameObject.SetActive(false);
            scrollTransform.parent.Find("BlackScreen").gameObject.SetActive(true);

            float newY = Mathf.MoveTowards(scrollTransform.localPosition.y, targetY, Time.deltaTime * 25);

            
            newY += scroll * 4f;
            

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
