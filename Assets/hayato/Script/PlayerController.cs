using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask platformLayer;
    private bool isGrounded = true;

    private bool isRotating = false;
    private float rotateAngle = 0;
    private float rotateTimer = 0;

    private bool isJumping = false;
    private bool isJumpingCheck = true;
    private float jumpTimeCounter;
    private float jumpTime = 0.35f;
    private float _jumpPower;

    private GameObject childNMagPole;
    private GameObject childSMagPole;
    private float childEnableCounter;
    private bool childEnabled = true;

    InputManager inputManager;
    PlayerManager playerManager;

    public GameObject eye;

    void Awake()
    {
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
        float step = playerManager.RotationSpeed * Time.deltaTime;
        // 指定したオブジェクトの座標を使って、地面と当たり判定をしている。
        Vector2 groundedStart = eye.transform.position;
        Vector2 groundedEnd = eye.transform.position - eye.transform.up * 0.38f;

        isGrounded = Physics2D.Linecast(groundedStart, groundedEnd, platformLayer);
        Debug.DrawLine(groundedStart, groundedEnd, Color.red);

        if (!isRotating) {      // 回転していないときは、キー入力を受け取る。
            if (inputManager.RotateLeftKey) {
                rotateAngle += 90f;
                isRotating = true;
                rotateTimer = playerManager.RotationSecond;

            } else if (inputManager.RotateRightKey) {
                rotateAngle -= 90f;
                isRotating = true;
                rotateTimer = playerManager.RotationSecond;
            }
        } else {        // 回転中の処理。
            rotateTimer -= Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, rotateAngle), step);
            if (rotateTimer <= 0) {
                isRotating = false;
            }
        }
    }

    void FixedUpdate()
    {
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
                magnet.effectorEnabledTime = magnet.effectorEnabledCounter;
                effector2D.enabled = false;
                effector2D.forceMagnitude = 0;
                childEnableCounter = playerManager.ChildReEnableCounter;
                childEnabled = false;
                childNMagPole.SetActive(false);
                childSMagPole.SetActive(false);
                jumpTimeCounter = jumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
            }
        }
    }


}