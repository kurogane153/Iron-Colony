using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgSize : MonoBehaviour {

    private SpriteRenderer sr;
    //spriteの縦横幅
    private float width;
    private float height;
    //画像のpixelperUnit
    private float pixelperUnit = 100f;
    //画像サイズ
    private float pixel_width = 64f;
    private float pixel_height = 64f;

    private Vector2 RiSize;

    void Start () {
        //spriteの情報取得
        sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
        height = sr.bounds.size.y;
        RiSize.y = pixel_height / height;
        RiSize.x = pixel_width / width;
        sr.size.Scale(RiSize);
    }
	void Update () {
		
	}
    void ChangeStateToHold()
    {
        RiSize.y = pixel_height / height;
        RiSize.x = pixel_width / width;
        sr.size.Scale(RiSize);
    }
}
