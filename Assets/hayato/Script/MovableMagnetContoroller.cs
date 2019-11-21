using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovableMagnetContoroller : MonoBehaviour {

    PositionConstraint positionConstraint;
    BoxCollider2D boxCollider;
    [SerializeField] private float ConstraintEnableCounter = 1f;
    private float posConstraintReEnableTime;
    private bool isMagStickReleased = false;

    SpriteRenderer sprite;
    Color color;

    // Use this for initialization
    void Start () {
        positionConstraint = GetComponent<PositionConstraint>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;
    }
	
	// Update is called once per frame
	void Update () {
		if(0 < posConstraintReEnableTime) {
            posConstraintReEnableTime -= Time.deltaTime;
            var semitransparentColor = color;
            semitransparentColor.a = 0.5f;
            sprite.color = semitransparentColor;
            if (posConstraintReEnableTime <= 0) {
                sprite.color = color;
                isMagStickReleased = false;
            }
        }
	}

    public void SetPosConstraintEnable()
    {
        if(!isMagStickReleased) positionConstraint.enabled = true;

    }

    public void SetPosConstraintDisable()
    {
        positionConstraint.enabled = false;
        isMagStickReleased = true;
        posConstraintReEnableTime = ConstraintEnableCounter;
    }

    
}
