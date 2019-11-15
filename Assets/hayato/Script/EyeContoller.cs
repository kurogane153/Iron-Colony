using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeContoller : MonoBehaviour {

    InputManager inputManager;
    [SerializeField] private readonly float MaxEyeXPosition = 0.07f;
    [SerializeField] private readonly float EyeMoveSpeed = 0.001f;
    private float eyeX;


    void Start () {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        // 親からの回転の影響を無効化している
        //gameObject.transform.rotation = Quaternion.Euler(0,0,0);
    }

    void FixedUpdate()
    {
        // 目を移動する方向に寄らせる処理。
        // 最大の寄れる位置に来るまで、入力キーの方向×speedで移動させている
        if (eyeX <= MaxEyeXPosition && eyeX >= -MaxEyeXPosition)
        {
            eyeX = inputManager.MoveKey * EyeMoveSpeed;
            transform.Translate(eyeX, 0, 0, Space.Self);
        }

        // 移動キーを離したときは、元の位置に徐々に戻す処理。
        if(inputManager.MoveKey == 0)
        {
            if(0.00000f < eyeX)
            {
                eyeX -= EyeMoveSpeed;
            }
            else if(eyeX < 0.00000f)
            {
                eyeX += EyeMoveSpeed;
            }
            transform.Translate(eyeX, 0, 0, Space.Self);
        }
    }

}
