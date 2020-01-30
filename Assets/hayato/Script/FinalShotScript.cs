using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalShotScript : MonoBehaviour {

	void Start () {
        StartCoroutine("FinalShot");
	}
	
    private IEnumerator FinalShot()
    {
        yield return new WaitForSeconds(0.1f);
        Pauser.Pause();
        yield return new WaitForSeconds(1.6f);
        //ここにアニメーター再生の処理を書く
        Pauser.Resume();
    }

    
}
