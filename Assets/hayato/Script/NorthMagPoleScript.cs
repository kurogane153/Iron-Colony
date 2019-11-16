using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthMagPoleScript : MonoBehaviour {

    public GameObject Mairo;
    PointEffector2D pointEffector;
    private float MyForceMagnitude;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet N" || collision.gameObject.tag == "Movable Magnet S") {
            
        }
    }

    public void EnablePointEffector ()
    {
        pointEffector.enabled = true;
    }

    public void DisablePointEffector ()
    {
        pointEffector.enabled = false;
    }

}
