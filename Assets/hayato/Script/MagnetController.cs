using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour {

    PointEffector2D effector2D;
    private float myForceMagunitude;
    [SerializeField] private float effectorEnabledCounter = 1f;
    public float effectorEnabledTime;

    private enum MagPole
    {
        None,
        N_mag,
        S_mag
    }
    [SerializeField] MagPole Pole = MagPole.None;

    private void Awake()
    {
        effector2D = GetComponent<PointEffector2D>();
        myForceMagunitude = effector2D.forceMagnitude;
    }

    void Update ()
    {
		
	}

    // プレイヤーがジャンプしたときにPointEffector2Dを無効化されたときの処理。
    // 有効化までの時間はプレイヤー側が設定している。
    // こちらは、無効化されている間に時間を加算、設定された時間以上になったら
    // effectorを有効化させている。
    private void FixedUpdate()
    {
        if(effector2D.enabled == false) {
            effectorEnabledTime += Time.deltaTime;
            if(effectorEnabledTime >= effectorEnabledCounter) {
                effector2D.enabled = true;
            }
        }
    }

    // プレイヤーのNかS極がトリガーに触れている間呼び出される
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 自分の極とプレイヤーの極を比較して、吸引か反発か切り替えている。
        if(collision.tag == "N_mag") {
            if(Pole == MagPole.N_mag) {
                effector2D.forceMagnitude = -myForceMagunitude;
            }else if(Pole == MagPole.S_mag) {
                effector2D.forceMagnitude = myForceMagunitude;
            }

        }else if(collision.tag == "S_mag") {
            if (Pole == MagPole.N_mag) {
                effector2D.forceMagnitude = myForceMagunitude;
            } else if (Pole == MagPole.S_mag) {
                effector2D.forceMagnitude = -myForceMagunitude;
            }
        }
    }

    
}
