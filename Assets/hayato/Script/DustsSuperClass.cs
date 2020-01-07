using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustsSuperClass : MonoBehaviour {

    public float _moveSpeed = 6f;

    public Rigidbody2D rb;

    private Vector3 player;

	public void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("IronPlanet").transform.position;
        rb.AddForce(player.normalized * _moveSpeed, ForceMode2D.Impulse);
    }

    public void FixedUpdate()
    {
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
