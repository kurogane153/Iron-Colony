using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class testmayu : MonoBehaviour
{
    float back_x = 0;
    private GameObject player;
    public float speed = 10;
    public int spriteCount = 3;

    void Start()
    {
        player = GameObject.Find("mairo");
    }

    void Update()
    {
        if (back_x != player.transform.position.x)
        {
            if (player.transform.position.x > 0)
            {
                // 左へ移動
                transform.position += Vector3.left * speed * player.transform.position.x;
            }
        }
        back_x = player.transform.position.x;
    }

    void OnBecameInvisible()
    {
        // スプライトの幅を取得
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        // 幅ｘ個数分だけ右へ移動
        transform.position += Vector3.right * width * spriteCount;
    }
}