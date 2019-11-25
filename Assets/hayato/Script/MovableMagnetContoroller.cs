using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovableMagnetContoroller : MonoBehaviour {

    PositionConstraint positionConstraint;
    [SerializeField] private float ConstraintEnableCounter = 1f;
    private float posConstraintReEnableTime;
    private bool isMagStickReleased = false;

    SpriteRenderer sprite;
    Color color;

    void Start () {
        positionConstraint = GetComponent<PositionConstraint>();
        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;
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
        positionConstraint.translationOffset = new Vector3(gameObject.transform.position.x ,0 , 0) - new Vector3(GameObject.Find("Mairo").gameObject.transform.position.x, 0, 0);
        if (!isMagStickReleased) positionConstraint.enabled = true;
     }

    public void SetPosConstraintDisable()
    {
        positionConstraint.enabled = false;
        isMagStickReleased = true;
        posConstraintReEnableTime = ConstraintEnableCounter;
    }

    //半透明にする関数
    private void Transparentize_half()
    {
        Color semitransparentColor = color;
        semitransparentColor.a = 0.5f;
        sprite.color = semitransparentColor;
    }


}
