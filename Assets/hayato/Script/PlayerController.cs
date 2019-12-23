using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer Renderer;
    public Sprite NormalSprite;
    public Sprite Active_N_Pole_Sprite;
    public Sprite Active_S_Pole_Sprite;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask platformLayer;
    private bool isGrounded = true;

    private bool isRotating = false;
    public bool GetIsRotating() { return isRotating; }
    private float rotateAngle = 0;
    private float rotateTimer = 0;
    public int angleNumber;

    public bool isWallStick = false;
    private bool isMagJamp = false;
    private bool isJumping = false;
    private bool isJumpingCheck = true;
    private float jumpTimeCounter;
    private float _jumpPower;

    private GameObject childNMagPole;
    private GameObject childSMagPole;
    private float childEnableCounter;
    private bool childEnabled = true;

    InputManager inputManager;
    PlayerManager playerManager;

    public GameObject eye;

    NorthMagPoleScript northMagController;
    SouthMagPoleScript southmagController;

    private GameObject movableMag;
    MovableMagnetContoroller moveMagController;
    private bool isMovableMagStick;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        inputManager = InputManager.Instance;
        jumpTimeCounter = playerManager.JumpTime;
        childNMagPole = transform.GetChild(0).gameObject;
        childSMagPole = transform.GetChild(1).gameObject;
        northMagController = childNMagPole.GetComponent<NorthMagPoleScript>();
        southmagController = childSMagPole.GetComponent<SouthMagPoleScript>();
        Renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 指定したオブジェクトの座標を使って、地面と当たり判定をしている。
        Vector2 groundedStart = eye.transform.position - eye.transform.up * 0.34f - eye.transform.right * 0.3f;
        Vector2 groundedEnd = eye.transform.position - eye.transform.up * 0.34f + eye.transform.right * 0.3f;

        isGrounded = Physics2D.Linecast(groundedStart, groundedEnd, platformLayer);
        Debug.DrawLine(groundedStart, groundedEnd, Color.red);

        if (!isRotating) {      // 回転していないときは、キー入力を受け取る。
            if (inputManager.RotateLeftKey) {
                RotatingNow(90);
                
            } else if (inputManager.RotateRightKey) {
                RotatingNow(-90);
            }
        } else {        // 回転中の処理。回転できるようになるまでの時間を減らしてる
            rotateTimer -= Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), step);
            if (rotateTimer <= 0) {
                isRotating = false;
            }
        }
    }

    void FixedUpdate()
    {
        // 地面にいるとき
        if (isGrounded) {
            rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.MoveSpeed - rb.velocity.x), rb.velocity.y));

            if (isJumpingCheck && inputManager.JumpKey != 0) {
                SoundManager.Instance.PlaySeByName("Jump_2");
                jumpTimeCounter = playerManager.JumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
                isWallStick = false;
            }
        // 空中にいるとき
        } else {
            // ジャンプキーが話されたらジャンプ中でないことにする
            if (inputManager.JumpKey == 0) {
                isJumping = false;
            }
            // ジャンプしてないかつ磁石の側面にくっついていないとき
            if (!isJumping && !isWallStick) {
                // 落ちる力が一定を超えるとそれ以上落下速度が上がらないようにする
                if(rb.velocity.y <= -10) {
                    rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 0));
                } else if(rb.velocity.y <= 0) {
                    // 落ち始めると重力を使って落ちるようにする
                    rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), Physics.gravity.y * playerManager.GravityRate));
                } else {
                    // ジャンプパワーがまだあるときは小ジャンプ実現のためにジャンプパワー軽減率を使う
                    if (0 <= _jumpPower) {
                        _jumpPower -= playerManager.JumpPowerAttenuation * 2;
                        rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
                    // ジャンプパワーがないときは重力を使って落とす
                    } else {
                        rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), Physics.gravity.y * playerManager.GravityRate));
                    }
                }
            }
        }

        // ジャンプ中
        if (isJumping) {
            
            // ジャンプキーを押し続けていられる時間をへらす
            jumpTimeCounter -= Time.deltaTime;

            // ジャンプキーを押し続けている間は通常のジャンプパワー軽減率がはたらく
            if (inputManager.JumpKey == 2) {
                _jumpPower -= playerManager.JumpPowerAttenuation;
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
            } else if(inputManager.JumpKey == 0) {
                _jumpPower -= playerManager.JumpPowerAttenuation;

            }
            // ジャンプキーを押し続けていられる時間がくると、ジャンプ中を解除する
            if (jumpTimeCounter < 0) {
                isJumping = false;
            }
            // 下に落ちているときはジャンプ中を解除
            if (rb.velocity.y <= -1) {
                isJumping = false;
            }
        }

        if (inputManager.JumpKey == 0) {
            isJumpingCheck = true;
        }

        // 子要素の磁力が非アクティブ時、復活までの時間を減らし、その時間になると磁力を復活させる
        if (!childEnabled) {
            childEnableCounter -= Time.deltaTime;
            if (childEnableCounter <= 0f) {
                childEnabled = true;
                childNMagPole.GetComponent<BoxCollider2D>().enabled = true;
                childSMagPole.GetComponent<BoxCollider2D>().enabled = true;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magnet") {
            isWallStick = true;
            isMagJamp = false;

        }
    }

    // プレイヤーのコライダーに触れたやつのタグがMagnetだったときにそこからジャンプしたとき！
    // そいつのPointEffector2Dコンポーネントと磁石用スクリプトを取得
    // effector2dを無効化させて、再び有効にするためのカウンターをセット
    // そして自分は、N極とS極を設定した時間無効化する。
    // effector2dを無効化された相手は、自分のスクリプトの中でカウントダウンができる。
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magnet") {
            
            if (isJumpingCheck && inputManager.JumpKey == 1 && isWallStick && !isMagJamp) {
                PointEffector2D effector2D = collision.gameObject.GetComponent<PointEffector2D>();
                MagnetController magnet = collision.gameObject.GetComponent<MagnetController>();
                magnet.effectorEnabledTime = magnet.effectorEnabledCounter;
                magnet.isPoleEnter = false;
                effector2D.enabled = false;
                effector2D.forceMagnitude = 0;
                childEnableCounter = playerManager.ChildReEnableCounter;
                childEnabled = false;
                childNMagPole.GetComponent<BoxCollider2D>().enabled = false;
                childSMagPole.GetComponent<BoxCollider2D>().enabled = false;
                jumpTimeCounter = playerManager.JumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
                isWallStick = false;
                SoundManager.Instance.PlaySeByName("Jump_2");
                isMagJamp = true;
            } else if (isJumpingCheck && inputManager.JumpKey == 1 && !isMagJamp) {
                SoundManager.Instance.PlaySeByName("Jump_2");
                jumpTimeCounter = playerManager.JumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
                isWallStick = false;
                isMagJamp = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magnet") {
            isWallStick = false;
            isMagJamp = false;
        }
    }

    // 回転ボタン押されたときの処理
    private void RotatingNow(float Rotate)
    {
        rotateAngle += Rotate;
        if (rotateAngle > 360) {
            rotateAngle = 90;
        }
        if (rotateAngle < 0) {
            rotateAngle = 270;
        }
        isRotating = true;
        rotateTimer = playerManager.RotationSecond;
        iTween.RotateTo(gameObject, iTween.Hash("z", rotateAngle, "time", playerManager.RotationSecond));
        if (3 < ++angleNumber) {
            angleNumber = 0;
        }
        if (isMovableMagStick) {
            OnRotateOffMagStick();
        }
        SoundManager.Instance.PlaySeByName("punch-swing1");
    }

    // 動く磁石をくっつかせたときの処理
    // 動く磁石を取得してそれのコンストレイントポジションをアクティブにし、
    // 子要素の磁力を切る
    public void SetMovableMagStickFlg(Collision2D collision)
    {
        movableMag = collision.gameObject;
        moveMagController = movableMag.GetComponent<MovableMagnetContoroller>();
        moveMagController.SetPosConstraintEnable();
        isMovableMagStick = true;
        northMagController.DisablePointEffector();
        southmagController.DisablePointEffector();
    }

    // 回転したときにくっついている磁石を離すふるまいをしていただく
    private void OnRotateOffMagStick()
    {
        moveMagController.SetPosConstraintDisable();
        isMovableMagStick = false;
        northMagController.EnablePointEffector();
        southmagController.EnablePointEffector();
    }

    //　他オブジェクトから使う用。動く磁石をくっつかせているかわかる
    public bool GetisMovableMagStck() { return isMovableMagStick; }

    // N極がアクティブ中は画像を差し替える
    public void Change_Effectively_N_Pole()
    {
        Renderer.sprite = Active_N_Pole_Sprite;
    }

    // S極がアクティブ中は画像を差し替える
    public void Change_Effectively_S_Pole()
    {
        Renderer.sprite = Active_S_Pole_Sprite;
    }

    // どちらもアクティブでない、または磁界から離れると元の画像に戻す
    public void Change_Normal_Sprite()
    {
        Renderer.sprite = NormalSprite;
    }

    public void MovableMagSetPalent(Collision2D collision)
    {
        transform.SetParent(collision.transform);
    }

    private void RemovePalent()
    {
        transform.SetParent(null);
    }

}