﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthMagPoleScript : MonoBehaviour {

    public GameObject Mairo;
    PlayerController playerController;
    PointEffector2D pointEffector;
    private float MyForceMagnitude;
    [SerializeField] private float movableMagImpactPower = 2f;

    // Use this for initialization
    void Start () {
        pointEffector = GetComponent<PointEffector2D>();
        MyForceMagnitude = pointEffector.forceMagnitude;
        playerController = Mairo.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet N") {
            pointEffector.forceMagnitude = -MyForceMagnitude;
        } else if (collision.gameObject.tag == "Movable Magnet S" && (playerController.angleNumber == 0 || playerController.angleNumber == 2)) {
            pointEffector.forceMagnitude = MyForceMagnitude * movableMagImpactPower;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet S" && !playerController.GetisMovableMagStck() && (playerController.angleNumber == 0 || playerController.angleNumber == 2)) {
            playerController.SetMovableMagStickFlg(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Movable Magnet N") {
            pointEffector.forceMagnitude = MyForceMagnitude * movableMagImpactPower;
        }
    }

    public void EnablePointEffector ()
    {
        pointEffector.enabled = true;
        pointEffector.forceMagnitude = MyForceMagnitude;
    }

    public void DisablePointEffector ()
    {
        pointEffector.enabled = false;
    }

}
