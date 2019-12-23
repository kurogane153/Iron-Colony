using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovableMagnetContoroller : MonoBehaviour {

    [SerializeField] private float ConstraintEnableCounter = 1f;
    private float posConstraintReEnableTime;
    private bool isMagStickReleased = false;
    private bool isMagSticked = false;
    public float offsetOnStick = 0.66f;
    private float offset;

    SpriteRenderer sprite;
    Color color;

    public Sprite StickImage;
    private Sprite NormalImage;

    Rigidbody2D rb;

    private Transform player;

    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        color = sprite.color;
        NormalImage = sprite.sprite;
        player = GameObject.Find("Mairo").gameObject.transform;
    }
	
	void Update () {
		if(0 < posConstraintReEnableTime) {
            posConstraintReEnableTime -= Time.deltaTime;
            Transparentize_half();
            if (posConstraintReEnableTime <= 0) {
                sprite.color = color;
                isMagStickReleased = false;
            }
        }
        if (isMagSticked) {
            PositionUpdate();
        }
	}

    public void SetPosConstraintEnable()
    {
        if (transform.position.x - GameObject.Find("Mairo").transform.position.x < 0) {
            offset = -offsetOnStick;
        } else {
            offset = offsetOnStick;
        }
        isMagSticked = true;
        Change_MyImage_Stick();
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        gameObject.layer = 14;
     }

    public void SetPosConstraintDisable()
    {
        isMagSticked = false;
        isMagStickReleased = false;
        posConstraintReEnableTime = ConstraintEnableCounter;
        Change_MyImage_Normal();
        rb.gravityScale = 1;
        gameObject.layer = 12;
    }

    //半透明にする関数
    private void Transparentize_half()
    {
        Color semitransparentColor = color;
        semitransparentColor.a = 0.5f;
        sprite.color = semitransparentColor;
    }

    private void Change_MyImage_Normal()
    {
        sprite.sprite = NormalImage;
    }

    private void Change_MyImage_Stick()
    {
        sprite.sprite = StickImage;
    }

    private void VelocitySync(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    private void PositionUpdate()
    {
        transform.position = new Vector3(player.position.x + offset, player.position.y, player.position.z);
    }
}
