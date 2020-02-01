using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevtor : MonoBehaviour {

    private PlayerController controller;
    private Rigidbody2D rb;

    private bool isMairoOnCollision;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GameObject.Find("Mairo").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (isMairoOnCollision) {
            transform.Translate(new Vector3(0, 0.05f, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "S_mag") {
            controller.enabled = false;
            StartCoroutine("TrainMoveStart");
            GameObject mairo = GameObject.Find("Mairo");
            mairo.transform.parent = transform;
            Rigidbody2D rbm = mairo.GetComponent<Rigidbody2D>();
            rbm.velocity = Vector2.zero;
            rbm.isKinematic = true;
        }
    }

    private IEnumerator TrainMoveStart()
    {
        yield return new WaitForSeconds(1f);
        isMairoOnCollision = true;
        yield return new WaitForSeconds(2f);
        //FadeManager.Instance.LoadScene("Chapter2", 1f);
        PlayerPrefs.SetInt("Chapter", 1);
    }
}
