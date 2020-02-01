using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        // IronColonyによってSetActive(true)されたときにアニメーショントリガーをONにして一度だけアニメーションする。
        _animator.SetTrigger("New Trigger");
    }

}
