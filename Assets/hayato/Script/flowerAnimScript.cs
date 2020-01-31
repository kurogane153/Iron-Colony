using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerAnimScript : MonoBehaviour {

    void Start () {
        GetComponent<Animator>().SetTrigger("OnInstantiate");
    }

}
