using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustsSuperClass : MonoBehaviour {

    public float _moveSpeed = 6f;

    public Rigidbody2D rb;

    private Vector3 player;
    private Vector3 myPos;
    Vector2 target;


    public void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("IronPlanet").transform.position;
        myPos = gameObject.transform.position;
        target = player - myPos;
        rb.velocity = target.normalized * _moveSpeed;
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
