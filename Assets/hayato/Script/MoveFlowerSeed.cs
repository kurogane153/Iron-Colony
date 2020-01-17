using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFlowerSeed : MonoBehaviour {

    private bool isSetFlowerStand;
    [SerializeField] private GameObject _flowerInstance;
    [SerializeField] private GameObject _collisionFlower;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody2;
    private MovableMagnetContoroller magnetContoroller;
    private Vector3 startPosition;

	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2 = GetComponent<Rigidbody2D>();
        magnetContoroller = GetComponent<MovableMagnetContoroller>();
        startPosition = transform.position;
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FlowerStand" && !isSetFlowerStand) {
            Instantiate(_flowerInstance, transform.position - new Vector3(0, 0.3f, 0), Quaternion.identity);
            Instantiate(_collisionFlower, transform.position + new Vector3(0, 1.9f, 0), Quaternion.identity);
            isSetFlowerStand = true;
            gameObject.tag = "Ground";
            gameObject.layer = LayerMask.NameToLayer("Default");
            rigidbody2.bodyType = RigidbodyType2D.Static;
            magnetContoroller.enabled = false;
        } else if(collision.tag == "KillZone") {
            transform.position = startPosition;
            rigidbody2.velocity = Vector2.zero;
        }

        
    }
}
