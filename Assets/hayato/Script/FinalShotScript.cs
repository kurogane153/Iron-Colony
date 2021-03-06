﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalShotScript : MonoBehaviour {

    [SerializeField] private GameObject _teslaEnergySoul;   // テスラキャノン発射時に生成される弾オブジェクト
    [SerializeField] private Color _teslaUltraChargeColor;

    void Start () {
        StartCoroutine("FinalShot");
	}
	
    private IEnumerator FinalShot()
    {
        Pauser.Pause();
        SoundManager.Instance.PlaySeByName("hero1");
        yield return new WaitForSeconds(1.6f);
        //ここにアニメーター再生の処理を書く
        Pauser.Resume();
        SoundManager.Instance.PlaySeByName("beamgun1");
        GameObject instantTeslaEnergySoul = Instantiate(_teslaEnergySoul, transform.position, Quaternion.identity) as GameObject;
        instantTeslaEnergySoul.GetComponent<EnergySoulScript>().SetParameter(true, _teslaUltraChargeColor, 100);
    }

    
}
