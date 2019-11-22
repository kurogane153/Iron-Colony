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
    private float rotateAngle = 0;
    private float rotateTimer = 0;
    public int angleNumber;

    public bool isWallStick = false;
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

    void Awake()
    {
       
    }

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
                if( 3 < ++angleNumber) {
                    angleNumber = 0;
                }
                if (isMovableMagStick) {
                    OnRotateOffMagStick();
                }
                
            } else if (inputManager.RotateRightKey) {
                rotateAngle -= 90f;
                isRotating = true;
                rotateTimer = playerManager.RotationSecond;
                if ( --angleNumber < 0) {
                    angleNumber = 3;
                }
                if (isMovableMagStick) {
                    OnRotateOffMagStick();
                }
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
                jumpTimeCounter = playerManager.JumpTime;
                isJumpingCheck = false;
                isJumping = true;
                _jumpPower = playerManager.JumpPower;
                isWallStick = false;
            }
        } else {
            if (inputManager.JumpKey == 0) {
                isJumping = false;
            }
            if (!isJumping && !isWallStick) {
                if(rb.velocity.y <= -10) {
                    rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 0));
                } else if(rb.velocity.y <= 0) {
                    rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), Physics.gravity.y * playerManager.GravityRate));
                } else {
                    _jumpPower -= playerManager.JumpPowerAttenuation * 2;
                    rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
                }
            }
        }

        if (isJumping) {
            
            jumpTimeCounter -= Time.deltaTime;

            if (inputManager.JumpKey == 2) {
                _jumpPower -= playerManager.JumpPowerAttenuation;
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
            } else if(inputManager.JumpKey == 0) {
                _jumpPower -= playerManager.JumpPowerAttenuation;

            }
            if (jumpTimeCounter < 0) {
                isJumping = false;
            }
            if (rb.velocity.y <= -1) {
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
                childNMagPole.GetComponent<BoxCollider2D>().enabled = true;
                childSMagPole.GetComponent<BoxCollider2D>().enabled = true;
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
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Magnet") {
            isWallStick = false;
        }
    }


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

    public bool GetisMovableMagStck()
    {
        return isMovableMagStick;
    }

    public void Change_Effectively_N_Pole()
    {
        Renderer.sprite = Active_N_Pole_Sprite;
    }

    public void Change_Effectively_S_Pole()
    {
        Renderer.sprite = Active_S_Pole_Sprite;
    }

    public void Change_Normal_Sprite()
    {
        Renderer.sprite = NormalSprite;
    }

}