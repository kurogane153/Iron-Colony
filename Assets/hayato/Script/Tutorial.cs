using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    [SerializeField] private GameObject _nextTutorial;
    [SerializeField] private float _diplayTime = 6f;
    private float disableTime;

    private void FixedUpdate()
    {
        disableTime += Time.deltaTime;
        if(_diplayTime < disableTime) {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if(_nextTutorial != null) _nextTutorial.SetActive(true);
    }
}
