using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovableMagnetContoroller : MonoBehaviour {

    PositionConstraint positionConstraint;
    [SerializeField] private float ConstraintEnableCounter = 1f;
    private float posConstraintReEnableTime;
    private bool isMagStickReleased = false;
    public float offsetOnStick = 0.66f;

    SpriteRenderer sprite;
    Color color;

    public Sprite StickImage;
    private Sprite NormalImage;

    Rigidbody2D rb;

    void Start () {
        positionConstraint = GetComponent<PositionConstraint>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        color = sprite.color;
        NormalImage = sprite.sprite;
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
	}

    public void SetPosConstraintEnable()
    {
        if (!isMagStickReleased) positionConstraint.enabled = true;
        
        if (transform.position.x - GameObject.Find("Mairo").transform.position.x < 0) {
            positionConstraint.translationOffset = new Vector3(-offsetOnStick, 0, 0);
        } else {
            positionConstraint.translationOffset = new Vector3(offsetOnStick, 0, 0);
        }

        Change_MyImage_Stick();
        rb.velocity = new Vector2(0, 0);
     }

    public void SetPosConstraintDisable()
    {
        positionConstraint.enabled = false;
        isMagStickReleased = true;
        posConstraintReEnableTime = ConstraintEnableCounter;
        Change_MyImage_Normal();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mairo") {
            VelocitySync(new Vector2(0, 0));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mairo") {
            VelocitySync(new Vector2(0,0));
        }
    }

    public void VelocitySync(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
}
