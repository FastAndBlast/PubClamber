using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    public GameObject audioSourcePrefab;
    public AudioClip phoneClip;

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            Destroy(gameObject);
        }
        if (other.GetComponentInParent<WalkingManager>())
        {
            GameObject sourceInstance = Instantiate(audioSourcePrefab);

            sourceInstance.GetComponent<AudioSource>().clip = phoneClip;
            sourceInstance.GetComponent<AudioSource>().volume = 0.2f;
            sourceInstance.GetComponent<AudioSource>().Play();
            sourceInstance.GetComponent<DestroyTimer>().time = phoneClip.length + 0.1f;
            triggered = true;
        }
    }




}
