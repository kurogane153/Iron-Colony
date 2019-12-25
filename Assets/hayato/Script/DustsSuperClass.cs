using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustsSuperClass : MonoBehaviour {

    public float _moveSpeed = -20f;

    public Rigidbody2D rb;

	public void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    public void FixedUpdate()
    {
        rb.AddForce(new Vector2(_moveSpeed, 0));
        if (!GetComponent<Renderer>().isVisible) {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "IronPlanet") {
            DestroySelf();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "CannonChargeUnit") {
            DestroySelf();
        }
    }
}
