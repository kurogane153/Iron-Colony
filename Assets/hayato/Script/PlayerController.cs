using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 localScale;
    private bool isGrounded = true;
    private bool isRotating = false;
    private int rotateDirection = 0;    //左回転 = 0　、　右回転 = 1。
    private float rotateAngle = 0;
    private float rotateTimer = 0;
    private bool isJumping = false;
    private bool isJumpingCheck = true;
    private float jumpTimeCounter;
    private float jumpTime = 0.35f;
    private float _jumpPower;
    public float speed = 120f;
    public float rotateSecond = 1.4f;
    [SerializeField] private LayerMask platformLayer;
    private GameObject childNMagPole;
    private GameObject childSMagPole;
    [SerializeField] private float childReEnableCounter = 0.5f;
    private float childEnableCounter;
    private bool childEnabled = true;

    InputManager inputManager;
    PlayerManager playerManager;

    public GameObject eye;


    void Awake()
    {
        localScale = transform.localScale;
        jumpTimeCounter = jumpTime;
    }

    void Start()
    {
        playerManager = PlayerManager.Instance;
        inputManager = InputManager.Instance;
        childNMagPole = transform.GetChild(0).gameObject;
        childSMagPole = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        // 指定したオブジェクトの座標を使って、地面と当たり判定をしている。
        Vector2 groundedStart = eye.transform.position;
        Vector2 groundedEnd = eye.transform.position - eye.transform.up * 0.38f;

        isGrounded = Physics2D.Linecast(groundedStart, groundedEnd, platformLayer);
        Debug.DrawLine(groundedStart, groundedEnd, Color.red);
    }

    

    void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        if (isGrounded) {
            rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.MoveSpeed - rb.velocity.x), rb.velocity.y));

            if (isJumpingCheck && inputManager.JumpKey != 0) {
                jumpTimeCounter = jumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
            }
        } else {
            if (inputManager.JumpKey == 0) {
                isJumping = false;
            }
            if (!isJumping) {
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), Physics.gravity.y * playerManager.GravityRate));
            }
        }

        if (!isRotating) {      // 回転していないときは、キー入力を受け取る。
            if (inputManager.RotateLeftKey) {
                rotateDirection = 0;
                rotateAngle += 90f;
                isRotating = true;
                rotateTimer = rotateSecond;
            } else if (inputManager.RotateRightKey) {
                rotateDirection = 1;
                rotateAngle -= 90f;
                isRotating = true;
                rotateTimer = rotateSecond;
            }
        } else {        // 回転中の処理。
            rotateTimer -= Time.deltaTime;
            if(rotateDirection == 0) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), step);
            } else if (rotateDirection == 1) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), step);
            }
            if (rotateTimer <= 0) {
                isRotating = false;
            }
        }

        if (isJumping) {
            
            jumpTimeCounter -= Time.deltaTime;

            if (inputManager.JumpKey == 2) {
                _jumpPower -= 0.1f;
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
            }
            if (jumpTimeCounter < 0) {
                isJumping = false;
            }
        }

        if (inputManager.JumpKey == 0) {
            isJumpingCheck = true;
        }

        if (!childEnabled) {
            childEnableCounter -= Time.deltaTime;
            if (childEnableCounter <= 0f) {
                childEnabled = true;
                childNMagPole.SetActive(true);
                childSMagPole.SetActive(true);
            }

        }
    }

    // Magnetに触れたときにも、地面に触れた、ということにして
    // ジャンプできるようにしている。
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magnet") {
            isGrounded = true;
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
            if (isJumpingCheck && inputManager.JumpKey != 0) {
                PointEffector2D effector2D = collision.gameObject.GetComponent<PointEffector2D>();
                MagnetController magnet = collision.gameObject.GetComponent<MagnetController>();
                magnet.effectorEnabledTime = 0;
                effector2D.enabled = false;
                childEnableCounter = childReEnableCounter;
                childEnabled = false;
                childNMagPole.SetActive(false);
                childSMagPole.SetActive(false);
                jumpTimeCounter = jumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
                Debug.Log("じゃんぷ");
            }
        }
    }


}