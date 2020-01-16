using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test99 : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Mairo") {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log(rb.velocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mairo") {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log(rb.velocity);
        }
    }

    // マイロが6.0でぶつかってきたら壊れるでいいんじゃね
}
