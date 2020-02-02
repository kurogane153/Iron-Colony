using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {

    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _chapterSelect;


    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Back_fromStartMenu();
        }
    }


    public void Back_fromStartMenu()
    {
        gameObject.SetActive(false);
        _mainMenuPanel.SetActive(true);
        SoundManager.Instance.PlaySeByName("NO");
    }

    public void Open_ChapterSelect()
    {
        gameObject.SetActive(false);
        _chapterSelect.SetActive(true);
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void NewGame()
    {
        gameObject.SetActive(false);
        FadeManager.Instance.LoadScene("EventScene", 2f);
        SoundManager.Instance.PlaySeByName("newgame");
    }
}
