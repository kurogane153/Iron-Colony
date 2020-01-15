using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowFlower : MonoBehaviour {

    private bool isTouchMaxGrowPoint;   // 成長できる最高点に触れたか

	void Start () {
		
	}
	
	void Update () {
		if(!isTouchMaxGrowPoint) {
            transform.Translate(0, 0.01f, 0);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isTouchMaxGrowPoint && collision.tag == "FlowerMaxGrowPoint") {
            isTouchMaxGrowPoint = true;
            Debug.Log("ぐれた");
        }
    }
}
