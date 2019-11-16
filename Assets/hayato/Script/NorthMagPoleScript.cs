using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthMagPoleScript : MonoBehaviour {

    public GameObject Mairo;
    PlayerController playerController;
    PointEffector2D pointEffector;
    private float MyForceMagnitude;

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
        } else if (collision.gameObject.tag == "Movable Magnet S") {
            pointEffector.forceMagnitude = MyForceMagnitude;
            playerController.SetMovableMagStickFlg();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Movable Magnet N") {
            pointEffector.forceMagnitude = MyForceMagnitude;
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
