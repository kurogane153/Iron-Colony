using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastBossScript : MonoBehaviour {

    private float bossHp;
    public float GetbossHp() { return bossHp; }
    private int dustThrowCount;
    private bool isCrazyMode;
    private float dustThrowTimer;
    private bool deathConfirmFlag;
    public void SetDeathConfirmFlag() { deathConfirmFlag = true; }

    [SerializeField] private Slider slider;
    [SerializeField] private GameObject DustThrowPoint;

    [SerializeField] private GameObject StarDust;
    [SerializeField] private GameObject PowerDust;

    [SerializeField] private float _bossStartHP = 300f;

    [SerializeField] private float _dustThrowRate_Normal = 8f;
    [SerializeField] private float _dustThrowRate_Crazy = 5f;

    [SerializeField] private int _powerDustThrowTime_Normal = 3;
    [SerializeField] private int _powerDustThrowTime_Crazy = 5;

    [SerializeField] private GameObject _effectPoint1, _effectPoint2, _effectPoint3, _smallEffect, _bigEffect;

    void Start () {
        bossHp = _bossStartHP;
        slider.value = bossHp / _bossStartHP;
    }

    private void FixedUpdate()
    {
        if (0 < bossHp && !deathConfirmFlag) {
            dustThrowTimer += Time.deltaTime;
            if (isCrazyMode) {
                if (_dustThrowRate_Crazy <= dustThrowTimer) {
                    ++dustThrowCount;
                    Debug.Log("ダスト投てき " + dustThrowCount + "回目");
                    Instantiate(InstantDust(), DustThrowPoint.transform.position, Quaternion.identity);
                    dustThrowTimer = 0;
                }
            } else {
                if (_dustThrowRate_Normal <= dustThrowTimer) {
                    ++dustThrowCount;
                    Debug.Log("ダスト投てき " + dustThrowCount + "回目");
                    Instantiate(InstantDust(), DustThrowPoint.transform.position, Quaternion.identity);
                    dustThrowTimer = 0;
                }
            }
        }
    }

    private GameObject InstantDust()
    {
        SoundManager.Instance.PlaySeByName("cannon2");
        if (isCrazyMode) {
            if(dustThrowCount == _powerDustThrowTime_Crazy) {
                Debug.Log("発狂モードのラスボスがパワーダストを投げた！");
                dustThrowCount = 0;
                return PowerDust;
            } else {
                Debug.Log("発狂モードのラスボスがふつうのダストを投げた！");
                return StarDust;
            }
        } else {
            if(dustThrowCount == _powerDustThrowTime_Normal) {
                Debug.Log("通常モードのラスボスがパワーダストを投げた！");
                dustThrowCount = 0;
                return PowerDust;
            } else {
                Debug.Log("通常モードのラスボスがふつうのダストを投げた！");
                return StarDust;
            }
        }
    }

    private IEnumerator HPCheck()
    {
        if(bossHp <= _bossStartHP / 3 && !isCrazyMode) {
            //ボスHPが最大値の3割かつ発狂モードでないとき
            yield return new WaitForSeconds(3.5f);
            isCrazyMode = true;
            GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.1f, 0.1f);
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlayBgmByName("game_maoudamashii_2_lastboss04");
            Debug.Log("ラスボスは発狂モードになった！！");
        } else if(bossHp <= 0 && isCrazyMode) {
            //ボスがHP0以下かつ発狂モードであるとき
            yield return new WaitForSeconds(3.5f);
            SoundManager.Instance.StopBgm();
            yield return new WaitForSeconds(0.5f);
            Instantiate(_smallEffect, _effectPoint1.transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySeByName("bomb1");
            yield return new WaitForSeconds(0.5f);
            Instantiate(_smallEffect, _effectPoint2.transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySeByName("bomb1");
            yield return new WaitForSeconds(0.5f);
            Instantiate(_smallEffect, _effectPoint3.transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySeByName("bomb1");
            yield return new WaitForSeconds(1f);
            Instantiate(_bigEffect, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySeByName("bomb1");
            SoundManager.Instance.PlaySeByName("bomb1");
            GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
            GetComponent<CircleCollider2D>().enabled = false;
            transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3f);
            FadeManager.Instance.LoadScene("EndingScene", 3.5f);
        }
    }

    public void BossHPDamage(float damage)
    {
        bossHp -= damage;
        slider.value = bossHp / _bossStartHP;
        //コルーチンを使って、時間差で発狂モードになる。
        StartCoroutine("HPCheck");
    }
}
