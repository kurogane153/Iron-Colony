using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForOPSceneLoad : MonoBehaviour {

    public void Chapter1Load()
    {
        FadeManager.Instance.LoadScene("mayu Main", 1f);
    }

    public void Chapter3Load()
    {
        FadeManager.Instance.LoadScene("TrainScene", 1f);
    }

    public void Chapter4Load()
    {
        FadeManager.Instance.LoadScene("LastBossTestScene", 1f);
    }

    public void TitleSceneLoad()
    {
        FadeManager.Instance.LoadScene("TitleScene", 1f);
    }
    
}
