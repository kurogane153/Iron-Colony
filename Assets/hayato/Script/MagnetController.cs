using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour {

    PointEffector2D effector2D;
    SpriteRenderer sprite;
    Color color;
    private float myForceMagunitude;
    public float effectorEnabledCounter = 1f;
    public float effectorEnabledTime;
    public bool isPoleEnter;
    private string enterPole;
    private int conflictAngle;
    private float distanceN;
    private float distanceS;

    GameObject player;
    GameObject playerMagN;
    GameObject playerMagS;

    PlayerController playerController;

    private enum MagPole
    {
        None,
        N_mag,
        S_mag
    }
    [SerializeField] MagPole Pole = MagPole.None;

    private void Awake()
    {
        effector2D = GetComponent<PointEffector2D>();
        
    }

    private void Start()
    {
        myForceMagunitude = effector2D.forceMagnitude;
        player = GameObject.Find("Eye").gameObject;
        playerMagN = GameObject.Find("North Magnetic Pole").gameObject;
        playerMagS = GameObject.Find("South Magnetic Pole").gameObject;
        playerController = GameObject.Find("Mairo").GetComponent<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();
        color = sprite.color;
    }

    // プレイヤーがジャンプしたときにPointEffector2Dを無効化されたときの処理。
    // 有効化までの時間はプレイヤー側が設定している。
    // こちらは、無効化されている間に時間を加算、設定された時間以上になったら
    // effectorを有効化させている。
    void Update ()
    {
        if (effectorEnabledTime > 0f) {
            effectorEnabledTime -= Time.deltaTime;
            var semitransparentColor = color;
            semitransparentColor.a = 0.5f;
            sprite.color = semitransparentColor;
            if (effectorEnabledTime <= 0f) {
                effector2D.enabled = true;
                sprite.color = color;
                effector2D.forceMagnitude = 0;
            }
        }
    }

    // プレイヤーのNかS極がトリガーに触れている間呼び出される
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // 自分の極とプレイヤーの極を比較して、吸引か反発か切り替えている。
        if ((collision.tag == "N_mag" || collision.tag == "S_mag") && !isPoleEnter) {
            distanceN = (transform.position - playerMagN.transform.position).sqrMagnitude;
            distanceS = (transform.position - playerMagS.transform.position).sqrMagnitude;
           
            //ここでなんと、プレイヤーと磁石のワールド座標での2点間の角度をとっている！！！！！！！！！！！！！！！！
            Vector3 diff = transform.position - player.transform.position;
            Vector3 axis = Vector3.Cross(player.transform.forward, diff);
            float angle = Vector3.Angle(player.transform.right*-1, diff) * (axis.x < 0 ? -1 : 1);
            Debug.Log(angle);

            if ( (-45.5f <= angle && angle < 0) || (0 <= angle && angle < 45.5f)) {
                conflictAngle = 0;
            } else if ( 45.5f <= angle && angle < 135.5f) {
                conflictAngle = 1;
            } else if ( 135.5f <= angle || angle < -135.5f) {
                conflictAngle = 2;
            } else {
                conflictAngle = 3;
            }
            switch (conflictAngle) {
                case 0:
                case 2:
                    if (playerController.angleNumber == 1 || playerController.angleNumber == 3) {
                        effector2D.enabled = false;
                        var semitransparentColor = color;
                        semitransparentColor.a = 0.5f;
                        sprite.color = semitransparentColor;
                    }
                    break;
                case 1:
                case 3:
                    if (playerController.angleNumber == 0 || playerController.angleNumber == 2) {
                        effector2D.enabled = false;
                        var semitransparentColor = color;
                        semitransparentColor.a = 0.5f;
                        sprite.color = semitransparentColor;
                    }
                    break;

            }
            Debug.Log(conflictAngle);
            if (Pole == MagPole.N_mag) {
                
                // 自分がN極で、プレイヤーのN極のほうが磁石と近かったら、反発モードにする。
                if (distanceN < distanceS) {
                    effector2D.forceMagnitude = -myForceMagunitude;
                    // S極のほうが近かったら吸引モード
                } else {
                    effector2D.forceMagnitude = myForceMagunitude;
                }
                
            } else if (Pole == MagPole.S_mag) {
                // 自分がN極で、プレイヤーのS極のほうが磁石と近かったら、反発モードにする。
                if (distanceN < distanceS) {
                    effector2D.forceMagnitude = myForceMagunitude;
                    // N極のほうが近かったら吸引モード
                } else {
                    effector2D.forceMagnitude = -myForceMagunitude;
                }
            }
            isPoleEnter = true;
            enterPole = collision.tag;
        }
    }

    // プレイヤーのNかS極がトリガーに触れている間呼び出される
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 自分の極とプレイヤーの極を比較して、吸引か反発か切り替えている。
        if (collision.tag == "N_mag" || collision.tag == "S_mag") {
            distanceN = (transform.position - playerMagN.transform.position).sqrMagnitude;
            distanceS = (transform.position - playerMagS.transform.position).sqrMagnitude;

            //ここでなんと、プレイヤーと磁石のワールド座標での2点間の角度をとっている！！！！！！！！！！！！！！！！
            Vector3 diff = transform.position - player.transform.position;
            Vector3 axis = Vector3.Cross(player.transform.forward, diff);
            float angle = Vector3.Angle(player.transform.right * -1, diff) * (axis.x < 0 ? -1 : 1);

            if ((-45.5f <= angle && angle < 0) || (0 <= angle && angle < 45.5f)) {
                conflictAngle = 0;
            } else if (45.5f <= angle && angle < 135.5f) {
                conflictAngle = 1;
            } else if (135.5f <= angle || angle < -135.5f) {
                conflictAngle = 2;
            } else {
                conflictAngle = 3;
            }
            switch (conflictAngle) {
                case 0:
                case 2:
                    if (playerController.angleNumber == 1 || playerController.angleNumber == 3) {
                        effector2D.enabled = false;
                        var semitransparentColor = color;
                        semitransparentColor.a = 0.5f;
                        sprite.color = semitransparentColor;
                        isPoleEnter = false;
                        effectorEnabledTime = effectorEnabledCounter/10;
                    }
                    break;
                case 1:
                case 3:
                    if (playerController.angleNumber == 0 || playerController.angleNumber == 2) {
                        effector2D.enabled = false;
                        var semitransparentColor = color;
                        semitransparentColor.a = 0.5f;
                        sprite.color = semitransparentColor;
                        isPoleEnter = false;
                        effectorEnabledTime = effectorEnabledCounter/10;
                    }
                    break;

            }
            if (Pole == MagPole.N_mag) {

                // 自分がN極で、プレイヤーのN極のほうが磁石と近かったら、反発モードにする。
                if (distanceN < distanceS) {
                    effector2D.forceMagnitude = -myForceMagunitude;
                    // S極のほうが近かったら吸引モード
                } else {
                    effector2D.forceMagnitude = myForceMagunitude;
                }

            } else if (Pole == MagPole.S_mag) {
                // 自分がN極で、プレイヤーのS極のほうが磁石と近かったら、反発モードにする。
                if (distanceN < distanceS) {
                    effector2D.forceMagnitude = myForceMagunitude;
                    // N極のほうが近かったら吸引モード
                } else {
                    effector2D.forceMagnitude = -myForceMagunitude;
                }
            }
            isPoleEnter = true;
            enterPole = collision.tag;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == enterPole && isPoleEnter) {
            //if (Pole == MagPole.N_mag) {
            //    effector2D.forceMagnitude = myForceMagunitude;
            //} else if (Pole == MagPole.S_mag) {
            //    effector2D.forceMagnitude = myForceMagunitude;
            //}
            
            isPoleEnter = false;
            effectorEnabledTime = effectorEnabledCounter;
        }
    }


}
