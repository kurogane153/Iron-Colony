using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovableMagnetContoroller : MonoBehaviour {

    PositionConstraint positionConstraint;
    [SerializeField] private float ConstraintEnableCounter = 1f;
    private float posConstraintReEnableTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(0 < posConstraintReEnableTime) {
            posConstraintReEnableTime -= Time.deltaTime;
            if(posConstraintReEnableTime <= 0) {
                SetPosConstraintEnable();
            }
        }
	}

    public void SetPosConstraintEnable()
    {
        positionConstraint.enabled = true;
    }

    public void SetPosConstraintDisable()
    {
        positionConstraint.enabled = false;
        posConstraintReEnableTime = ConstraintEnableCounter;
    }
}
