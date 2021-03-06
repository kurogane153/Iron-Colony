﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthMagPoleScript : MonoBehaviour {

    public GameObject particle;
    public GameObject RepulsionParticle;
    public GameObject Mairo;
    PlayerController playerController;
    PointEffector2D pointEffector;
    private float MyForceMagnitude;
    [SerializeField] private float movableMagImpactPower = 2f;

    void Start () {
        pointEffector = GetComponent<PointEffector2D>();
        MyForceMagnitude = pointEffector.forceMagnitude;
        playerController = Mairo.GetComponent<PlayerController>();
    }
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet N" && (playerController.angleNumber == 0 || playerController.angleNumber == 2)) {
            pointEffector.forceMagnitude = -MyForceMagnitude;
            Vector3 center = (collision.transform.position + transform.position) * 0.5f;
            Instantiate(RepulsionParticle, center, transform.rotation);
            SoundManager.Instance.PlaySeByName("light_saber1");
        } else if (collision.gameObject.tag == "Movable Magnet S") {
            pointEffector.forceMagnitude = MyForceMagnitude * movableMagImpactPower;
        } else if ((collision.gameObject.tag == "Movable Magnet S" || collision.gameObject.tag == "Movable Magnet N") && (playerController.angleNumber == 1 || playerController.angleNumber == 3)) {
            DisablePointEffector();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet S") {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet S" && !playerController.GetisMovableMagStck() && (playerController.angleNumber == 0 || playerController.angleNumber == 2)) {
            playerController.SetMovableMagStickFlg(collision);
            DisablePointEffector();
        } else if (collision.gameObject.tag == "Magnet" && !playerController.GetIsRotating() && collision.gameObject.GetComponent<MagnetController>().isPoleEnter && collision.gameObject.GetComponent<MagnetController>().IsMagPole_S()) {
            StickPerticleEnable();
            SoundManager.Instance.PlaySeByName("kachi2");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Movable Magnet N" && (playerController.angleNumber == 0 || playerController.angleNumber == 2)) {
            pointEffector.forceMagnitude = MyForceMagnitude;
            Vector3 center = (collision.transform.position + transform.position) * 0.5f;
            Instantiate(RepulsionParticle, center, transform.rotation);
        } else if (collision.gameObject.tag == "Movable Magnet S") {
            EnablePointEffector();
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

    public void StickPerticleEnable()
    {
        Instantiate(particle, transform.position, transform.rotation);
    }

}
