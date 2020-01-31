using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMonsterScript : MonoBehaviour
{

    [SerializeField, Range(0f, 20f)] private float _maxSpeed = 10f;
    [SerializeField, Range(0f, 3f)] private float _accel = 0.02f;
    [SerializeField, Range(0f, 5f)] private float _onGetBoosterAddMaxSpeed = 1.5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (rb.velocity.x < _maxSpeed) {
            rb.velocity = new Vector2(rb.velocity.x + _accel, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "RailObject") {
            Destroy(collision.gameObject);
        } else if (collision.tag == "Booster") {
            _maxSpeed += _onGetBoosterAddMaxSpeed;
            Destroy(collision.gameObject);
        }
    }
}