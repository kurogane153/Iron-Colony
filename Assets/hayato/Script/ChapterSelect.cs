using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterSelect : MonoBehaviour {

    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _Button1;
    [SerializeField] private GameObject _Button2;
    [SerializeField] private GameObject _Button3;
    [SerializeField] private GameObject _Button4;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Back_fromChapterSelect();
        }
    }

    private void OnEnable()
    {
        int save = PlayerPrefs.GetInt("Chapter", 0);

        switch (save) {
            case 0:
                _Button1.SetActive(false);
                _Button2.SetActive(false);
                _Button3.SetActive(false);
                _Button4.SetActive(false);
                break;
            case 1:
                _Button2.SetActive(false);
                _Button3.SetActive(false);
                _Button4.SetActive(false);
                break;
            case 2:
                _Button3.SetActive(false);
                _Button4.SetActive(false);
                break;
            case 3:
                _Button4.SetActive(false);
                break;
        }
    }

    public void Back_fromChapterSelect()
    {
        gameObject.SetActive(false);
        _startMenu.SetActive(true);
        SoundManager.Instance.PlaySeByName("NO");
    }

    public void Load_Scene1()
    {
        gameObject.SetActive(false);
        FadeManager.Instance.LoadScene("mayu Main", 3f);
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void Load_Scene2()
    {
        gameObject.SetActive(false);
        FadeManager.Instance.LoadScene("MasterScene", 3f);
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void Load_Scene3()
    {
        gameObject.SetActive(false);
        FadeManager.Instance.LoadScene("TrainScene", 3f);
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void Load_Scene4()
    {
        gameObject.SetActive(false);
        FadeManager.Instance.LoadScene("CommandRoom", 3f);
        SoundManager.Instance.PlaySeByName("OK");
    }
}
