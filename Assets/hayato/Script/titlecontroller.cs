using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlecontroller : MonoBehaviour {


    [SerializeField] Animator _TitleLogoAnimator;
    [SerializeField] Animator _PleaseAnyKeyAnimator;
    [SerializeField] Animator _IronPlanetAnimator;
    [SerializeField] Animator _MainMenuAnimator;

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _optionPanel;
    [SerializeField] private GameObject _quitPanel;

    [SerializeField] private UnityEngine.UI.Slider masterSlider;

    [SerializeField] private UnityEngine.UI.Slider BGMSlider;

    [SerializeField] private UnityEngine.UI.Slider SESlider;

    private bool isAnyKeyPress;

    void Update()
    {
        if (Input.anyKey && !isAnyKeyPress) {
            _TitleLogoAnimator.SetTrigger("New Trigger");
            _PleaseAnyKeyAnimator.SetTrigger("New Trigger");
            _IronPlanetAnimator.SetTrigger("New Trigger");
            _mainPanel.SetActive(true);
            //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
            isAnyKeyPress = true;
            SoundManager.Instance.PlaySeByName("OK2");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_mainPanel.activeSelf) {
                OpenQuitPanel();
            } else if (_startPanel.activeSelf) {
                Back_fromStartPanel();
            } else if (_optionPanel.activeSelf) {
                Back_fromOptionPanel();
            } else if (_quitPanel.activeSelf) {
                Back_fromQuitPanel();
            }
        }
    }



    public void OpenStartPanel()
    {
        _mainPanel.SetActive(false);
        _startPanel.SetActive(true);
        //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void OpenOptionPanel()
    {
        _mainPanel.SetActive(false);
        _optionPanel.SetActive(true);
        masterSlider.value = SoundManager.Instance.Volume;
        BGMSlider.value = SoundManager.Instance.BgmVolume;
        SESlider.value = SoundManager.Instance.SeVolume;
        //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void OpenQuitPanel()
    {
        _mainPanel.SetActive(false);
        _quitPanel.SetActive(true);
        //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
        SoundManager.Instance.PlaySeByName("OK");
    }

    public void Back_fromStartPanel()
    {
        _startPanel.SetActive(false);
        _mainPanel.SetActive(true);
        //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
        SoundManager.Instance.PlaySeByName("NO");
    }

    public void Back_fromOptionPanel()
    {
        _optionPanel.SetActive(false);
        _mainPanel.SetActive(true);
        PlayerPrefs.SetFloat("MasterVolume", SoundManager.Instance.Volume);
        PlayerPrefs.SetFloat("BGMVolume", SoundManager.Instance.BgmVolume);
        PlayerPrefs.SetFloat("SEVolume", SoundManager.Instance.SeVolume);
        //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
        SoundManager.Instance.PlaySeByName("NO");
    }

    public void Back_fromQuitPanel()
    {
        _quitPanel.SetActive(false);
        _mainPanel.SetActive(true);
        //_MainMenuAnimator.SetBool("Open", !_MainMenuAnimator.GetBool("Open"));
        SoundManager.Instance.PlaySeByName("NO");
    }

    public void ChangeMasterVolume(UnityEngine.UI.Slider slider)
    {
        SoundManager.Instance.Volume = slider.value;
    }

    public void ChangeBGMVolume(UnityEngine.UI.Slider slider)
    {
        SoundManager.Instance.BgmVolume = slider.value;
    }

    public void ChangeSEVolume(UnityEngine.UI.Slider slider)
    {
        SoundManager.Instance.SeVolume = slider.value;
    }
}
