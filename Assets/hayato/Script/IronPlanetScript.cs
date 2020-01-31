using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IronPlanetScript : MonoBehaviour {

    private bool isRotating = false;
    public bool GetIsRotating() { return isRotating; }
    public float rotateAngle = 0;
    private float rotateTimer = 0;
    public int angleNumber;
    private bool invincible_flag;
    private bool isFinalShotHit;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private GameObject _finalShotInstance;

    private bool isTeslaShotNG;    // テスラキャノンを撃ってもいいかのフラグ
    private bool isMoveOK;          // プレイヤーが動いてもOKかのフラグ
    private bool isMaxShot;         // 最大チャージで撃ったか
    private float teslaChargeTime;  // テスラキャノンチャージ時間
    private float teslaCapacity;    // テスラキャノンのチャージ容量（可変）
    private int powerDustGetCount;  // パワーダストを取った回数
    private float playerHP;         // プレイヤーHP 
    [SerializeField] private float _HPDamage = 20f;  //  HPダメージ量
    [SerializeField] private float _teslaMaxCharge = 5f;   // テスラキャノン最大チャージ（何秒チャージしたら撃てるかの定数）
    [SerializeField] private float _playerStartHP = 100;  // プレイヤー初期HP

    [SerializeField] private GameObject _teslaChargeEffect; // テスラキャノンをチャージ中に出てくるエフェクト
    [SerializeField] private GameObject _teslaShotEffect; // テスラキャノンショット時に出てくるエフェクト
    [SerializeField] private Vector3 _chargeEffectoffset;
    [SerializeField] private Vector3 _shotEffectoffset;
    [SerializeField] private GameObject _teslaEnergySoul;   // テスラキャノン発射時に生成される弾オブジェクト
    [SerializeField] private GameObject _damagedExplosioneffect;    // 被弾したときの爆発エフェクト
    [SerializeField] private GameObject _deathExplosioneffect;  // 死亡時爆発エフェクト
    private Vector3 teslaCannonPosition;  // テスラキャノンの座標
    private GameObject instantEffect;   // Instantiateされたエフェクトを覚えておく
    private GameObject instantTeslaShotEffect;  // Instantiateされたテスラキャノンショットエフェクト
    GameObject instantTeslaEnergySoul;

    public Slider HPslider;
    public Image HPsliderFill;
    public Slider TeslaMaxChargeSlider;
    public Slider TeslaSlider;
    public Image TeslaSliderFill;
    [SerializeField] private Color _fineColor;
    [SerializeField] private Color _coutionColor;
    [SerializeField] private Color _dangerColor;
    [SerializeField] private Color _teslaMaxChargeColor;
    [SerializeField] private Color _teslaUltraChargeColor;
    private Color teslaNormalColor;

    LastBossScript lastBossScript;
    InputManager inputManager;
    PlayerManager playerManager;
    Rigidbody2D rb;

    void Start () {
        inputManager = InputManager.Instance;
        playerManager = PlayerManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        lastBossScript = GameObject.Find("LastBoss").GetComponent<LastBossScript>();

        playerHP = _playerStartHP;
        teslaCapacity = 33f;

        HPslider.value = 1;
        HPsliderFill.color = _fineColor;
        TeslaSlider.value = 0;
        TeslaMaxChargeSlider.value = teslaCapacity / 100;
        teslaNormalColor = TeslaSliderFill.color;

        SoundManager.Instance.PlayBgmByName("game_maoudamashii_2_lastboss03");
    }
	
	void Update () {
        if (!isRotating && inputManager.JumpKey == 0 && isMoveOK) {
            // 回転していない＆テスラキャノンチャージしていない＆テスラキャノン最大ショット後の硬直でないときに回転できる
            if (inputManager.RotateLeftKey) {
                RotatingNow(180);
                if (instantTeslaShotEffect != null) {
                    // ショットエフェクトがあったら削除しておく。
                    Destroy(instantTeslaShotEffect);
                }

            } else if (inputManager.RotateRightKey) {
                RotatingNow(-180);
                if (instantTeslaShotEffect != null) {
                    // ショットエフェクトがあったら削除しておく。
                    Destroy(instantTeslaShotEffect);
                }
            }
            
        } else {        // 回転中の処理。回転できるようになるまでの時間を減らしてる
            rotateTimer -= Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), step);
            if (rotateTimer <= 0) {
                isRotating = false;
            }
        }
    }

    private void FixedUpdate()
    {
        
        if (isMoveOK) {
            if(66f < teslaChargeTime) {
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.MoveSpeed / 4 - rb.velocity.x), playerManager.MoveForceMultiplier * (inputManager.UpMoveKey * playerManager.MoveSpeed / 4 - rb.velocity.y)));
            } else {
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.MoveSpeed - rb.velocity.x), playerManager.MoveForceMultiplier * (inputManager.UpMoveKey * playerManager.MoveSpeed - rb.velocity.y)));
            }
        } else {
            rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.MoveSpeed / 10 - rb.velocity.x), playerManager.MoveForceMultiplier * (inputManager.UpMoveKey * playerManager.MoveSpeed / 10 - rb.velocity.y)));
        }

        if (inputManager.JumpKey == 2 && angleNumber == 0 && !isTeslaShotNG) {
            // ジャンプボタン押し続け＆砲口を敵に向けている＆最大ショット後の硬直でない
            
            if (instantEffect == null) {
                SoundManager.Instance.PlaySeByName("beam-gun-charge1");
                //チャージエフェクトが生成されていないとき、エフェクトを生成する。
                teslaCannonPosition = GameObject.Find("IronPlanet").transform.position + _chargeEffectoffset;
                instantEffect = GameObject.Instantiate(_teslaChargeEffect, teslaCannonPosition, Quaternion.identity) as GameObject;
            } else {
                //エフェクトが生成されていたら、座標をプレイヤーに合わせる
                instantEffect.transform.position = GameObject.Find("IronPlanet").transform.position + _chargeEffectoffset;
            }

            if (instantTeslaShotEffect != null) {
                //ショットエフェクトがまだ残っていたら座標をプレイヤーに合わせる
                instantTeslaShotEffect.transform.position = GameObject.Find("IronPlanet").transform.position + _shotEffectoffset;
            }

            if (teslaChargeTime <= teslaCapacity) {
                //チャージ中！
                teslaChargeTime += 100 / _teslaMaxCharge * Time.deltaTime;

                if(powerDustGetCount == 2 && 66f <= teslaChargeTime) {
                    TeslaSliderFill.color = _teslaMaxChargeColor;
                }

                if (teslaCapacity < teslaChargeTime) {
                    //満タンになったらそれ以上たまらないようにする
                    teslaChargeTime = teslaCapacity;
                    if (powerDustGetCount == 2) {
                        TeslaSliderFill.color = _teslaUltraChargeColor;
                    } else {
                        TeslaSliderFill.color = _teslaMaxChargeColor;
                    }
                }
            }
            TeslaSlider.value = teslaChargeTime / 100;

        } else if (inputManager.JumpKey == 0 || isTeslaShotNG) {
            // テスラキャノンチャージ中エフェクトがあったら削除して、ヒットエフェクトとショットエフェクト表示
            if (instantEffect != null) {

                SoundManager.Instance.StopSe();
                if (teslaChargeTime >= teslaCapacity) {
                    SoundManager.Instance.PlaySeByName("beamgun1");
                    
                    // チャージ量によって与えるダメージが変動する。
                    switch (powerDustGetCount) {
                        case 0:
                            instantTeslaEnergySoul = Instantiate(_teslaEnergySoul, transform.position, Quaternion.identity) as GameObject;
                            instantTeslaEnergySoul.GetComponent<EnergySoulScript>().SetParameter(false, _teslaMaxChargeColor, teslaChargeTime / 4.5f);
                            break;
                        case 1:
                            instantTeslaEnergySoul = Instantiate(_teslaEnergySoul, transform.position, Quaternion.identity) as GameObject;
                            instantTeslaEnergySoul.GetComponent<EnergySoulScript>().SetParameter(false, _teslaMaxChargeColor, teslaChargeTime / 2);
                            break;
                        case 2:
                            Vector2 castStart = transform.position;
                            Vector2 castEnd = transform.position + transform.right * 30f;
                            isFinalShotHit = Physics2D.Linecast(castStart, castEnd, platformLayer);
                            Debug.Log(isFinalShotHit);
                            Debug.DrawLine(castStart, castEnd, Color.red);
                            if (isFinalShotHit && lastBossScript.GetbossHp() - 100 <= 0) {
                                FinalShot();
                            } else {
                                instantTeslaEnergySoul = Instantiate(_teslaEnergySoul, transform.position, Quaternion.identity) as GameObject;
                                instantTeslaEnergySoul.GetComponent<EnergySoulScript>().SetParameter(true, _teslaUltraChargeColor, 100);
                            }
                            
                            break;
                    }
                    // テスラキャノンが最大チャージで放たれたときの処理（最大チャージ量は可変。)
                    if (instantTeslaShotEffect != null) {
                        // ショットエフェクトがあったら削除しておく。
                        Destroy(instantTeslaShotEffect);
                    }
                    instantTeslaShotEffect = GameObject.Instantiate(_teslaShotEffect, teslaCannonPosition + _shotEffectoffset, Quaternion.identity) as GameObject;
                    Destroy(instantTeslaShotEffect, 3f);
                    
                    isMoveOK = false;
                    isMaxShot = true;
                    // ここまで最大チャージで撃ったときの処理。
                }

                Destroy(instantEffect);
                TeslaSliderFill.color = teslaNormalColor;
                isTeslaShotNG = true;
            }
            // ショットエフェクトが生成されていたら座標をプレイヤーと合わせる。3秒後に削除
            if (instantTeslaShotEffect != null) {
                instantTeslaShotEffect.transform.position = GameObject.Find("IronPlanet").transform.position + _shotEffectoffset;
            }
            // テスラキャノンゲージ減る
            if (0 < teslaChargeTime) {
                teslaChargeTime -= 100 / _teslaMaxCharge * Time.deltaTime * (powerDustGetCount + 1 * 2.25f);
                TeslaSlider.value = teslaChargeTime / 100;
            } else {
                isTeslaShotNG = false;
                isMoveOK = true;
                if (isMaxShot) {
                    powerDustGetCount = 0;
                    teslaCapacity = 33f;
                    TeslaMaxChargeSlider.value = teslaCapacity / 100;
                    isMaxShot = false;
                }
            }
        }
    }

    // 回転ボタン押されたときの処理
    private void RotatingNow(float Rotate)
    {
        rotateAngle += Rotate;

        if (rotateAngle > 360) {
            rotateAngle = 180;
        }

        if (rotateAngle < -180) {
            rotateAngle = 0;
        }

        isRotating = true;
        rotateTimer = playerManager.RotationSecond;
        iTween.RotateTo(gameObject, iTween.Hash("z", rotateAngle, "time", playerManager.RotationSecond));

        if (1 < ++angleNumber) {
            angleNumber = 0;
        }
        
        SoundManager.Instance.PlaySeByName("punch-swing1");
    }

    // ダメージを受ける
    private void Damage()
    {
        if (!invincible_flag) {
            SoundManager.Instance.PlaySeByName("bomb1");
            playerHP -= _HPDamage;
            Debug.Log("現在のHP " + playerHP + " / " + _playerStartHP);


            // HPバー関係
            HPslider.value = playerHP / _playerStartHP;
            if (playerHP <= _playerStartHP / 3.5) {
                // HPが28%くらいでデンジャーカラー
                HPsliderFill.color = _dangerColor;
            } else if (playerHP <= _playerStartHP / 2) {
                // HPが半分以下でコーションカラー
                HPsliderFill.color = _coutionColor;
            }

            if (playerHP <= 0) {
                Instantiate(_deathExplosioneffect, transform.position, Quaternion.identity);
                transform.position = new Vector3(0, 0, -11);
                // 現在のScene名を取得する
                Scene loadScene = SceneManager.GetActiveScene();
                // Sceneの読み直し
                FadeManager.Instance.LoadScene("LastBossTestScene", 1f);
            } else {
                Instantiate(_damagedExplosioneffect, transform.position, Quaternion.identity);
            }
        }
        
    }

    // パワーダストを取ったとき、テスラキャノンのチャージ上限を解放する。最大で2まで増える。
    private void PowerCharge()
    {
        if(powerDustGetCount < 2) {
            powerDustGetCount++;
            switch (powerDustGetCount) {
                case 1:
                    SoundManager.Instance.PlaySeByName("power-up1");
                    teslaCapacity = 66f;
                    break;
                case 2:
                    SoundManager.Instance.PlaySeByName("power-up1");
                    teslaCapacity = 100f;
                    break;
            }
            TeslaMaxChargeSlider.value = teslaCapacity / 100;
        }
        Debug.Log("現在のチャージ上限 " + teslaCapacity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "StarDust") {
            Damage();
            Debug.Log("ダストが当たってプレイヤーにダメージ！");
        } else if (collision.gameObject.tag == "PowerDust") {
            Damage();
            Debug.Log("ダストが当たってプレイヤーにダメージ！");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "StarDust") {
            Damage();
            Debug.Log("ダストが当たってプレイヤーにダメージ！");
        } else if (collision.tag == "PowerDust") {
            PowerCharge();
            Debug.Log("パワーダストを吸収してチャージ容量が増えた！");
        }
    }

    private void FinalShot()
    {
        lastBossScript.SetDeathConfirmFlag();
        invincible_flag = true;
        Instantiate(_finalShotInstance, transform.position, Quaternion.identity);
        Pauser.Pause();
    }

}
