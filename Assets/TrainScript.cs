using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour {

    private Rigidbody2D rb;
    public bool NSFlag;    // 自分自身が何極かのフラグ。TRUEがN極。FALSEがS極。
    public bool isConflictRail;    // レールと触れているかのフラグ

    InputManager inputManager;

    void Start () {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        isConflictRail = true;
        NSFlag = true;
	}
	
	void Update () {
        if (inputManager.JumpKey == 1 && isConflictRail) {
            if (NSFlag) {
                // 自分の極をS極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, 75.0f), ForceMode2D.Impulse);
                NSFlag = false;
            } else {
                // 自分の極をN極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, -75.0f), ForceMode2D.Impulse);
                NSFlag = true;
            }
        } else if (!isConflictRail) {
            if (NSFlag) {
                // 自分の極をS極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, -75.0f), ForceMode2D.Force);
                
            } else {
                // 自分の極をN極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, 75.0f), ForceMode2D.Force);
                
            }
        }
    }

    private void FixedUpdate()
    {
        if(rb.velocity.x < 20f) {
            rb.velocity = new Vector2(rb.velocity.x + 0.05f, rb.velocity.y);
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "RailObject") {
            
            if (isConflictRail) {
                rb.AddForce(new Vector2(-75.0f, 0), ForceMode2D.Impulse);
            } else if (!isConflictRail) {
                if (NSFlag) {
                    // 自分の極をS極に変える。
                    rb.AddForce(new Vector2(-75.0f, 100.0f), ForceMode2D.Impulse);

                } else {
                    // 自分の極をN極に変える。
                    rb.AddForce(new Vector2(-75.0f, -100.0f), ForceMode2D.Impulse);

                }
            }
        } else if(collision.gameObject.tag == "Rail") {
            isConflictRail = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rail") {
            isConflictRail = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rail") {
            isConflictRail = false;
        }
    }
}
