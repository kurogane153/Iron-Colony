using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouthMagPoleScript : MonoBehaviour {

    public GameObject particle;
    public GameObject RepulsionParticle;
    public GameObject Mairo;
    PlayerController playerController;
    PointEffector2D pointEffector;
    private float MyForceMagnitude;
    [SerializeField] private float movableMagImpactPower = 2f;

    void Start()
    {
        pointEffector = GetComponent<PointEffector2D>();
        MyForceMagnitude = pointEffector.forceMagnitude;
        playerController = Mairo.GetComponent<PlayerController>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet S") {
            pointEffector.forceMagnitude = -MyForceMagnitude;
            Instantiate(RepulsionParticle, transform.position, transform.rotation);
        } else if (collision.gameObject.tag == "Movable Magnet N" && (playerController.angleNumber == 0 || playerController.angleNumber == 2)) {
            pointEffector.forceMagnitude = MyForceMagnitude * movableMagImpactPower;
        } else if ((collision.gameObject.tag == "Movable Magnet S" || collision.gameObject.tag == "Movable Magnet N") && (playerController.angleNumber == 1 || playerController.angleNumber == 3)) {
            DisablePointEffector();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet N") {
            DisablePointEffector();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet N" && !playerController.GetisMovableMagStck() && (playerController.angleNumber == 0 || playerController.angleNumber == 2) && !playerController.GetIsRotating()) {
            playerController.SetMovableMagStickFlg(collision);
        } else if (collision.gameObject.tag == "Magnet" && !playerController.GetIsRotating() && collision.gameObject.GetComponent<MagnetController>().isPoleEnter && collision.gameObject.GetComponent<MagnetController>().IsMagPole_N()) {
            StickPerticleEnable();
            SoundManager.Instance.PlaySeByName("kati_1");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movable Magnet S") {
            pointEffector.forceMagnitude = MyForceMagnitude * movableMagImpactPower;
        } else if(collision.gameObject.tag == "Movable Magnet N") {
            EnablePointEffector();
        }
    }

    public void EnablePointEffector()
    {
        pointEffector.enabled = true;
        pointEffector.forceMagnitude = MyForceMagnitude;
    }

    public void DisablePointEffector()
    {
        pointEffector.enabled = false;
    }

    public void StickPerticleEnable()
    {
        Instantiate(particle, transform.position, transform.rotation);
    }
}
