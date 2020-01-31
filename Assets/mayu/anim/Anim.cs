using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    Animator _animator;

    // Use this for initialization
    void Start()
    {

        _animator = GetComponent<Animator>();
    }

    public void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            //_animator.SetBool("key", true);
            _animator.SetTrigger("New Trigger");
            
        }
        else
        {
            //_animator.SetBool("key", false);
        }

    }

}
