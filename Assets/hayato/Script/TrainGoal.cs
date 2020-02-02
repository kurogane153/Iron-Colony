using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGoal : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.StopSe();
            FadeManager.Instance.LoadScene("CommandRoom", 1f);
        }
    }

}
