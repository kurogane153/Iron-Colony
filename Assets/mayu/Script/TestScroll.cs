using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TestScroll : MonoBehaviour
{
    float back_x = 0;
    private GameObject player;
    public float speed = 10;
    public int spriteCount = 3;

    void Start()
    {
        player = GameObject.Find("Mairo");
    }

    void Update()
    {
        if (back_x != player.transform.position.x)
        {
            if (player.transform.position.x > 0)
            {
                // 左へ移動
                transform.position += Vector3.right * speed * player.transform.position.x;
            }
        }else if (back_x == player.transform.position.x)
        {
            transform.position +=Vector3.right * 0.0f;
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