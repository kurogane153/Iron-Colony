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

    [SerializeField] private float _dustThrowRate_Normal = 8f;
    [SerializeField] private float _dustThrowRate_Crazy = 5f;

    [SerializeField] private int _powerDustThrowTime_Normal = 3;
    [SerializeField] private int _powerDustThrowTime_Crazy = 5;

    void Start () {
		
	}
	
	void Update () {
        dustThrowTimer += Time.deltaTime;
		if(_dustThrowRate_Normal <= dustThrowTimer) {
            Instantiate(StarDust);
            dustThrowTimer = 0;
        }
	}
}
