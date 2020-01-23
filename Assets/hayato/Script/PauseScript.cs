using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{

    //　ポーズした時に表示するUI
    [SerializeField]
    private GameObject pauseUI;

    [SerializeField]
    private GameObject restartCheckPanel;

    [SerializeField]
    private GameObject gotoTitleCheckPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //　ポーズUIが表示されてる時は停止
            if (pauseUI.activeSelf) {
                pauseUI.SetActive(false);
                Time.timeScale = 1f;
                SoundManager.Instance.UnPauseBGM();
                //　ポーズUIが表示されてなければ通常通り進行
            } else if (restartCheckPanel.activeSelf ) {
                Back_fromRestart();
            } else if (gotoTitleCheckPanel.activeSelf) {
                Back_fromTitle();
            } else {
                pauseUI.SetActive(true);
                Time.timeScale = 0f;
                SoundManager.Instance.PauseBGM();
            }
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        SoundManager.Instance.UnPauseBGM();
    }

    public void Restart()
    {
        pauseUI.SetActive(false);
        restartCheckPanel.SetActive(false);
        Time.timeScale = 1f;
        // Sceneの読み直し
        FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 2.5f);
    }

    public void GotoTitle()
    {
        pauseUI.SetActive(false);
        gotoTitleCheckPanel.SetActive(false);
        Time.timeScale = 1f;
        // Title画面へ
        FadeManager.Instance.LoadScene("TitleScene", 2.5f);
    }

    public void Open_Restart()
    {
        pauseUI.SetActive(false);
        restartCheckPanel.SetActive(true);
    }

    public void Open_GotoTitle()
    {
        pauseUI.SetActive(false);
        gotoTitleCheckPanel.SetActive(true);
    }

    public void Back_fromRestart()
    {
        pauseUI.SetActive(true);
        restartCheckPanel.SetActive(false);
    }

    public void Back_fromTitle()
    {
        pauseUI.SetActive(true);
        gotoTitleCheckPanel.SetActive(false);
    }
}
