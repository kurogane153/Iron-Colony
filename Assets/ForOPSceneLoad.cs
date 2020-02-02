using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForOPSceneLoad : MonoBehaviour {

    public void Chapter1Load()
    {
        FadeManager.Instance.LoadScene("mayu Main", 1f);
    }
    
}
