using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouthMagPoleScript : MonoBehaviour {

    public GameObject Mairo;
    PointEffector2D pointEffector;
    private float MyForceMagnitude;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnablePointEffector()
    {
        pointEffector.enabled = true;
    }

    public void DisablePointEffector()
    {
        pointEffector.enabled = false;
    }
}
