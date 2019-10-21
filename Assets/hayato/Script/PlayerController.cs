using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector2 localScale;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isJumpingCheck = true;
    private float jumpTimeCounter;
    private float jumpTime = 0.35f;
    private float _jumpPower;
    [SerializeField] private LayerMask platformLayer;

    InputManager inputManager;
    PlayerManager playerManager;

    void Awake()
    {
        localScale = transform.localScale;
        jumpTimeCounter = jumpTime;
    }

    void Start()
    {
        playerManager = PlayerManager.Instance;
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        isGrounded = Physics2D.Linecast(transform.position - transform.up * 0.4f, transform.position - transform.up * 0.6f, platformLayer);
    }

    void FixedUpdate()
    {
        if (inputManager.MoveKey != 0) {
            // 向きを変える
            localScale.x = inputManager.MoveKey;
            transform.localScale = localScale;
        }

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
                _jumpPower -= 0.2f;
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
            }
            if (jumpTimeCounter < 0) {
                isJumping = false;
            }
        }

        if (inputManager.JumpKey == 0) {
            isJumpingCheck = true;
        }
    }
}