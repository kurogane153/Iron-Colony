using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossScript : MonoBehaviour {

    private float bossHp;
    private int dustThrowCount;
    private bool isCrazyMode;
    private float dustThrowTimer;

    [SerializeField] private GameObject StarDust;
    [SerializeField] private GameObject PowerDust;

    [SerializeField] private float _bossStartHP = 300f;

    [SerializeField] private float _dustThrowRate_Normal = 8f;
    [SerializeField] private float _dustThrowRate_Crazy = 5f;

    [SerializeField] private int _powerDustThrowTime_Normal = 3;
    [SerializeField] private int _powerDustThrowTime_Crazy = 5;

    void Start () {
        bossHp = _bossStartHP;
	}
	
	void Update () {
        dustThrowTimer += Time.deltaTime;
        if (isCrazyMode) {
            if (_dustThrowRate_Crazy <= dustThrowTimer) {
                ++dustThrowCount;
                Debug.Log("ダスト投てき " + dustThrowCount + "回目");
                Instantiate(InstantDust());
                dustThrowTimer = 0;
            }
        } else {
            if (_dustThrowRate_Normal <= dustThrowTimer) {
                ++dustThrowCount;
                Debug.Log("ダスト投てき " + dustThrowCount + "回目");
                Instantiate(InstantDust());
                dustThrowTimer = 0;
            }
            HPCheck();
        }
        
	}

    private GameObject InstantDust()
    {
        if (isCrazyMode) {
            if(dustThrowCount == _powerDustThrowTime_Crazy) {
                Debug.Log("発狂モードのラスボスがパワーダストを投げた！");
                dustThrowCount = 0;
                return PowerDust;
            } else {
                Debug.Log("発狂モードのラスボスがふつうのダストを投げた！");
                return StarDust;
            }
        } else {
            if(dustThrowCount == _powerDustThrowTime_Normal) {
                Debug.Log("通常モードのラスボスがパワーダストを投げた！");
                bossHp -= 200f;
                dustThrowCount = 0;
                return PowerDust;
            } else {
                Debug.Log("通常モードのラスボスがふつうのダストを投げた！");
                return StarDust;
            }
        }
    }

    private void HPCheck()
    {
        if(bossHp <= _bossStartHP / 3) {
            isCrazyMode = true;
            Debug.Log("ラスボスは発狂モードになった！！");
        }
    }
}
