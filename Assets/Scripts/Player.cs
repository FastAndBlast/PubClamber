using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //string causeOfDeathString = "Unlucky";

    public void Die(string causeOfDeath, string tip)
    {
        //causeOfDeathString = causeOfDeath;
        //Destroy(this, 0.5f);

        GetComponent<WalkingManager>().Die(causeOfDeath, tip);
        //print("huh");
    }

    //private void OnDestroy()
    //{
    //    GameManager.instance.PlayerDeath(causeOfDeathString);
    //}

}
