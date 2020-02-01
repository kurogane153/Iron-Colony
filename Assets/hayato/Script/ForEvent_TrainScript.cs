using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEvent_TrainScript : MonoBehaviour {

    private PlayerController controller;
    private Rigidbody2D rb;

    private bool isMairoOnCollision;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        controller = GameObject.Find("Mairo").GetComponent<PlayerController>();
    }

    void Update () {
		
	}

    private void FixedUpdate()
    {
        if (isMairoOnCollision) {
            rb.velocity = new Vector2(rb.velocity.x + 0.05f, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "S_mag") {
            rb.bodyType = RigidbodyType2D.Dynamic;
            controller.enabled = false;
            StartCoroutine("TrainMoveStart");
            GameObject mairo = GameObject.Find("Mairo");
            mairo.transform.parent = transform;
            mairo.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            SoundManager.Instance.PlaySeByName("kachi2");
        }
    }

    private IEnumerator TrainMoveStart()
    {
        yield return new WaitForSeconds(1f);
        isMairoOnCollision = true;
        yield return new WaitForSeconds(3f);
        FadeManager.Instance.LoadScene("TrainScene", 1f);
    }
}
