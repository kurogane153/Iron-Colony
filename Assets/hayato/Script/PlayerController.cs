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
    private float jumpTime = 0f;
    private float _jumpPower;
    private float speed = 120f;
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
        Vector2 groundedStart = transform.position - transform.up * 0.4f;
        Vector2 groundedEnd = transform.position - transform.up * 0.6f + transform.eulerAngles;

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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -90f), step);
            }
        } else {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, -90f), step);
            if (inputManager.JumpKey == 0) {
                isJumping = false;
            }
            if (!isJumping) {
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), Physics.gravity.y * playerManager.GravityRate));
            }
        }

        if (isJumping) {
            
            jumpTimeCounter += Time.deltaTime;

            if (inputManager.JumpKey == 2) {
                _jumpPower -= 0.1f;
                rb.AddForce(new Vector2(playerManager.MoveForceMultiplier * (inputManager.MoveKey * playerManager.JumpMoveSpeed - rb.velocity.x), 1 * _jumpPower));
            }
            if (jumpTimeCounter > 1) {
                isJumping = false;
            }
        }

        if (inputManager.JumpKey == 0) {
            isJumpingCheck = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}