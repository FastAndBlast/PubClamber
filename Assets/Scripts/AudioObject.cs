using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public int index;
    private void OnDestroy()
    {
        SFXManager.instance.playingClips.Remove(index);
    }
}
