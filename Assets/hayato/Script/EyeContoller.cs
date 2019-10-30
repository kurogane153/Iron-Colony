using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeContoller : MonoBehaviour {

    InputManager inputManager;
    [SerializeField] private readonly float MaxEyeXPosition = 0.07f;
    [SerializeField] private readonly float EyeMoveSpeed = 0.001f;

    void Start () {
        inputManager = InputManager.Instance;
    }
	
    void FixedUpdate()
    {
        if (transform.localPosition.x <= MaxEyeXPosition && transform.localPosition.x >= -MaxEyeXPosition)
        {
            transform.Translate(inputManager.MoveKey * EyeMoveSpeed, 0, 0);
        }

        if(inputManager.MoveKey == 0)
        {
            if(0.00000f < transform.localPosition.x)
            {
                transform.Translate(-EyeMoveSpeed, 0, 0);
            }else if(transform.localPosition.x < 0.00000f)
            {
                transform.Translate(EyeMoveSpeed, 0, 0);
            }
            
        }
    }
}
