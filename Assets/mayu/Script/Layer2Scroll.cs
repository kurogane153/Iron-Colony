using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer2Scroll : MonoBehaviour {

    float back_x = 0;
    private GameObject player;
    private Rigidbody2D rb;
    private float speed = 0.02f;
    public int spriteCount = 3;

    void Start()
    {
        player = GameObject.Find("Mairo");
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (back_x != player.transform.position.x )
        {
            if (player.transform.position.x > back_x && rb.velocity.x >= 0.9f)
            {
                transform.position += Vector3.right * speed * 2.0f;
            }
            else if (player.transform.position.x < back_x && rb.velocity.x <= -0.9f)
            {
                transform.position += Vector3.left * speed * 2.0f;
            }
        }
        else if (back_x == player.transform.position.x)
        {
            transform.position += Vector3.left * 0.0f;
            transform.position += Vector3.right * 0.0f;
        }
        back_x = player.transform.position.x;
    }

    void OnBecameInvisible()
    {
        // スプライトの幅を取得
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        // 幅ｘ個数分だけ右へ移動
        transform.position += Vector3.left * width * spriteCount;
    }
}
