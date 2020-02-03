using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainScript : MonoBehaviour {

    [SerializeField, Range(0f, 10f)] private float _startSpeed = 3f;
    [SerializeField, Range(0f, 30f)] private float _maxSpeed = 16f;
    [SerializeField, Range(0f, 3f)] private float _accel = 0.05f;
    [SerializeField, Range(0f, 10f)] private float _whatLowSpeed = 6f;
    [SerializeField, Range(0f, 3f)] private float _onLowSpeedAccel = 0.075f;
    [SerializeField, Range(0f, 300f)] private float _switchSpeed = 75f;
    [SerializeField, Range(0f, 300f)] private float _nockBackPower = 75f;
    [SerializeField, Range(0f, 5f)] private float _onGetBoosterAddMaxSpeed = 1.5f;

    private Sprite NPoleImage;  // N極時の画像。最初はこれ
    public Sprite SPoleImage;   // S極時の画像。

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool NSFlag;    // 自分自身が何極かのフラグ。TRUEがN極。FALSEがS極。
    private bool isConflictRail;    // レールと触れているかのフラグ

    InputManager inputManager;

    void Start () {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        isConflictRail = true;
        NSFlag = true;
        rb.velocity = new Vector2(_startSpeed, 0);
        sr = GetComponent<SpriteRenderer>();
        NPoleImage = sr.sprite;
        SoundManager.Instance.PlayBgmByName("TABGM4");
        SoundManager.Instance.PlaySeByName("bullet-train-driving1");
        if (PlayerPrefs.GetInt("Chapter") < 3) {
            PlayerPrefs.SetInt("Chapter", 3);
        }
    }
	
	void Update () {
        if (inputManager.JumpKey == 1 && isConflictRail) {
            if (NSFlag) {
                // 自分の極をS極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, _switchSpeed), ForceMode2D.Impulse);
                NSFlag = false;
                sr.sprite = SPoleImage;
            } else {
                // 自分の極をN極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, -_switchSpeed), ForceMode2D.Impulse);
                NSFlag = true;
                sr.sprite = NPoleImage;
            }
        } else if (!isConflictRail) {
            if (NSFlag) {
                // 自分の極をS極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, -_switchSpeed), ForceMode2D.Force);
                
            } else {
                // 自分の極をN極に変える。
                rb.AddForce(new Vector2(rb.velocity.x, _switchSpeed), ForceMode2D.Force);
                
            }
        }
    }

    private void FixedUpdate()
    {
        if(rb.velocity.x < _maxSpeed) {
            rb.velocity = new Vector2(rb.velocity.x + _accel, rb.velocity.y);
        }
        if (rb.velocity.x < _whatLowSpeed) {
            rb.velocity = new Vector2(rb.velocity.x + _onLowSpeedAccel, rb.velocity.y);
        }
        if(_maxSpeed < rb.velocity.x) {
            rb.velocity = new Vector2(_maxSpeed, rb.velocity.y);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "RailObject") {
            SoundManager.Instance.PlaySeByName("trainExplosion");
            if (isConflictRail) {
                rb.AddForce(new Vector2(-_nockBackPower, rb.velocity.y), ForceMode2D.Impulse);
            } else if (!isConflictRail) {
                if (NSFlag) {
                    // 自分の極をS極に変える。
                    rb.AddForce(new Vector2(-_nockBackPower, 100.0f), ForceMode2D.Impulse);
                    NSFlag = false;
                    sr.sprite = SPoleImage;
                } else {
                    // 自分の極をN極に変える。
                    rb.AddForce(new Vector2(-_nockBackPower, -100.0f), ForceMode2D.Impulse);
                    NSFlag = true;
                    sr.sprite = NPoleImage;
                }
            }
        } else if(collision.gameObject.tag == "Rail") {
            isConflictRail = true;
            SoundManager.Instance.PlaySeByName("kachi2");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rail") {
            isConflictRail = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rail") {
            isConflictRail = false;
            SoundManager.Instance.PlaySeByName("light_saber1");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Booster") {
            _maxSpeed += _onGetBoosterAddMaxSpeed;
            Destroy(collision.gameObject);
            SoundManager.Instance.PlaySeByName("power-up1");
        } else if(collision.tag == "TrainMonster") {
            // 現在のScene名を取得する
            Scene loadScene = SceneManager.GetActiveScene();
            // Sceneの読み直し
            SceneManager.LoadScene(loadScene.name);
        }
    }
}
