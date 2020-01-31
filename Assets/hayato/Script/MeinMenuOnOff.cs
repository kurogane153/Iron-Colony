using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeinMenuOnOff : MonoBehaviour {

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        animator.SetBool("Open", !animator.GetBool("Open"));
    }

    private void OnDisable()
    {
        animator.SetBool("Open", !animator.GetBool("Open"));
    }
}
