using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXdir : MonoBehaviour {

    public float length = 4.0f; //移動する振幅
    public float speed = 2.0f;  //移動するスピード　大きくすると早くなる
    private Vector2 startPos;   //ゲーム開始時のポジションを入れる変数
    public bool negative = false;   //空のチェックボックスが表示されます。


	void Start () {
        startPos = this.transform.position;
        if(negative == true)
        {
            speed = -speed;
        }
	}
	
	void FixedUpdate () {
        transform.position = new Vector2((Mathf.Sin((Time.time) * speed) * length + startPos.x), startPos.y);
	}
}
