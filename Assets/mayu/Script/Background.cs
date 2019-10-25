using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    // スクロールするスピード
    public float speed = 0.1f;
    private GameObject playey;

    void Start()
    {
        playey = GameObject.Find("mairo");
    }

    void Update()
    {
        // 時間によってYの値が0から1に変化していく。1になったら0に戻り、繰り返す。
        float x = Mathf.Repeat(playey.transform.position.x * speed, 1);

        // Yの値がずれていくオフセットを作成
        Vector2 offset = new Vector2(x, 0);

        // マテリアルにオフセットを設定する
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
