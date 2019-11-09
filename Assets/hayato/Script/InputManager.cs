using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    protected static readonly string[] findTags =
    {
        "InputManager",
    };

    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {

                Type type = typeof(InputManager);

                foreach (var tag in findTags)
                {
                    GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

                    for (int j = 0; j < objs.Length; j++)
                    {
                        instance = (InputManager)objs[j].GetComponent(type);
                        if (instance != null)
                            return instance;
                    }
                }

                Debug.LogWarning(string.Format("{0} is not found", type.Name));
            }

            return instance;
        }
    }

    void Awake()
    {
        CheckInstance();
    }

    private bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (InputManager)this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this);
        return false;
    }

    /* -- Horizontal入力 --------------------------------------------------------------------------- */
    private float moveKey = 0;
    public float MoveKey
    {
        get { return moveKey; }
    }

    /* -- Jump入力 --------------------------------------------------------------------------------- */
    private int jumpKey = 0;
    public int JumpKey
    {
        get { return jumpKey; }
    }

    /* -- 左（反時計回り）回転入力 --------------------------------------------------------------------------------- */
    private bool rotateLeftKey = false;
    public bool RotateLeftKey
    {
        get { return rotateLeftKey; }
    }

    /* -- 右（時計回り）回転入力 --------------------------------------------------------------------------------- */
    private bool rotateRightKey = false;
    public bool RotateRightKey
    {
        get { return rotateRightKey; }
    }

    void Update()
    {
        // 移動
        moveKey = Input.GetAxisRaw("Horizontal");
        rotateLeftKey = Input.GetButtonDown("Rotate_Left");
        rotateRightKey = Input.GetButtonDown("Rotate_Right");
        // ジャンプ
        if (Input.GetButtonDown("Jump"))
        {
            jumpKey = 1;
        }
        else if (Input.GetButton("Jump"))
        {
            jumpKey = 2;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumpKey = 0;
        }
    }
}