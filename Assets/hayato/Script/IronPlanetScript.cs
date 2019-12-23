using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPlanetScript : MonoBehaviour {

    private bool isRotating = false;
    public bool GetIsRotating() { return isRotating; }
    public float rotateAngle = 0;
    private float rotateTimer = 0;
    public int angleNumber;

    InputManager inputManager;
    PlayerManager playerManager;
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        inputManager = InputManager.Instance;
        playerManager = PlayerManager.Instance;
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isRotating) {      // 回転していないときは、キー入力を受け取る。
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
}
