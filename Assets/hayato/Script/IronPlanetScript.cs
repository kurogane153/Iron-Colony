using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IronPlanetScript : MonoBehaviour {

    private bool isRotating = false;
    public bool GetIsRotating() { return isRotating; }
    public float rotateAngle = 0;
    private float rotateTimer = 0;
    public int angleNumber;

    private float teslaChargeTime;  // テスラキャノンチャージ時間
    private float teslaCapacity;    // テスラキャノンのチャージ容量（可変）
    private int powerDustGetCount;  // パワーダストを取った回数
    private float playerHP;         // プレイヤーHP 
    [SerializeField] private float HPDamage = 20f;  //  HPダメージ量
    [SerializeField] private float teslaMaxCharge = 5f;   // テスラキャノン最大チャージ（何秒チャージしたら撃てるかの定数）
    [SerializeField] private float _playerStartHP = 100;  // プレイヤー初期HP

    InputManager inputManager;
    PlayerManager playerManager;
    Rigidbody2D rb;

    void Start () {
        inputManager = InputManager.Instance;
        playerManager = PlayerManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        playerHP = _playerStartHP;
        teslaCapacity = 33f;
    }
	
	void Update () {
        if (!isRotating && inputManager.JumpKey == 0) {      // 回転していないときは、キー入力を受け取る。
            if (inputManager.RotateLeftKey) {
                RotatingNow(180);

            } else if (inputManager.RotateRightKey) {
                RotatingNow(-180);
            }
        } else {        // 回転中の処理。回転できるようになるまでの時間を減らしてる
            rotateTimer -= Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), step);
            if (rotateTimer <= 0) {
                isRotating = false;
            }
        }

        if(inputManager.JumpKey == 2) {
            if(teslaChargeTime <= teslaCapacity) {
                teslaChargeTime += 100 / teslaMaxCharge * Time.deltaTime;
                if(teslaCapacity < teslaChargeTime) {
                    teslaChargeTime = teslaCapacity;
                }
            }
            Debug.Log("テスラキャノン量      " + teslaChargeTime);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.MoveSpeed - rb.velocity.x), playerManager.MoveForceMultiplier * (inputManager.UpMoveKey * playerManager.MoveSpeed - rb.velocity.y)));
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
        playerHP -= HPDamage;
        Debug.Log("現在のHP " + playerHP + " / " + _playerStartHP);
        if(playerHP <= 0) {
            // 現在のScene名を取得する
            Scene loadScene = SceneManager.GetActiveScene();
            // Sceneの読み直し
            SceneManager.LoadScene(loadScene.name);
        }
    }

    // パワーダストを取ったとき、テスラキャノンのチャージ上限を解放する。最大で2まで増える。
    private void PowerCharge()
    {
        if(powerDustGetCount < 2) {
            powerDustGetCount++;
            switch (powerDustGetCount) {
                case 1:
                    teslaCapacity = 66f;
                    break;
                case 2:
                    teslaCapacity = 100f;
                    break;
            }
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
}
