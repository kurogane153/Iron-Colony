using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseWall : MonoBehaviour {

    private Rigidbody2D rb;
    private Vector2 playerVelocity;
    private bool collapseOK;
    private bool isCollapse;
    private BoxCollider2D[] cols;

	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.centerOfMass = new Vector3(0, -1, 0);
        cols = gameObject.GetComponents<BoxCollider2D>();
        
    }

    void Update () {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mairo" && collapseOK && !isCollapse) {
            rb.AddTorque(-75.0f, ForceMode2D.Impulse);
            Debug.Log("あ");
            isCollapse = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mairo") {
            playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            Debug.Log(playerVelocity);
            if (5.8f <= playerVelocity.x) {
                collapseOK = true;
            }
        } else if(collision.tag == "CollapseWallStopTrigger") {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            cols[1].enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
