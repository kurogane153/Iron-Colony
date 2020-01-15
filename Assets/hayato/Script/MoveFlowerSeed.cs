using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFlowerSeed : MonoBehaviour {

    private bool isSetFlowerStand;
    [SerializeField] private GameObject _flowerInstance;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody2;
    private MovableMagnetContoroller magnetContoroller;

	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody2 = GetComponent<Rigidbody2D>();
        magnetContoroller = GetComponent<MovableMagnetContoroller>();
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FlowerStand" && !isSetFlowerStand) {
            Instantiate(_flowerInstance, transform.position - new Vector3(0, 1.3f, 0), Quaternion.identity);
            isSetFlowerStand = true;
            gameObject.tag = "Ground";
            gameObject.layer = LayerMask.NameToLayer("Default");
            rigidbody2.bodyType = RigidbodyType2D.Static;
            magnetContoroller.enabled = false;
        }
    }
}
