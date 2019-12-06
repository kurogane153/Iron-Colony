using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magunetEffect : MonoBehaviour {
    public GameObject particle;

    void OnCollisionEnter(Collision other)
    {
        //衝突したオブジェクトがSphereだったらParticleを発生させる
        if (other.gameObject.name == "Magnet")
        {
            Instantiate(particle, transform.position, transform.rotation);
        }
    }
}
