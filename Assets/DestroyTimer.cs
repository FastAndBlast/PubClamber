using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float time;

    public bool runsWhilePaused = true;

    private void Update()
    {
        if (time > 0)
        {
            if (runsWhilePaused || !GameManager.paused)
            {
                time -= Time.deltaTime;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
