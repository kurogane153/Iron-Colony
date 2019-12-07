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

    public GameObject RepulsionParticle;
    public Sprite StickImage;
    private Sprite NormalImage;

    PlayerController playerController;

    private enum MagPole
    {
        None,
        N_mag,
        S_mag
    }
    [SerializeField] private MagPole Pole = MagPole.None;

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
        NormalImage = sprite.sprite;
    }

    // プレイヤーがジャンプしたときにPointEffector2Dを無効化されたときの処理。
    // 有効化までの時間はプレイヤー側が設定している。
    // こちらは、無効化されている間に時間を加算、設定された時間以上になったら
    // effectorを有効化させている。
    void Update ()
    {
        if (effectorEnabledTime > 0f) {
            effectorEnabledTime -= Time.deltaTime;
            Transparentize_half();
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
            conflictAngle = Find_hit_angle();
            switch (conflictAngle) {
                case 0:
                case 2:
                    if (playerController.angleNumber == 1 || playerController.angleNumber == 3) {
                        effector2D.enabled = false;
                        Transparentize_half();
                    } else {
                        Decide_force_magnitude(collision.transform.position, collision.transform.rotation);
                    }
                    break;
                case 1:
                case 3:
                    if (playerController.angleNumber == 0 || playerController.angleNumber == 2) {
                        effector2D.enabled = false;
                        Transparentize_half();
                    } else {
                        Decide_force_magnitude(collision.transform.position, collision.transform.rotation);
                    }
                    break;

            }
            isPoleEnter = true;
            enterPole = collision.tag;
        }
    }

    // プレイヤーのNかS極がトリガーに触れている間呼び出される
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 自分の極とプレイヤーの極を比較して、吸引か反発か切り替えている。
        if (collision.tag == "N_mag" || collision.tag == "S_mag" && !playerController.GetIsRotating()) {
            conflictAngle = Find_hit_angle();
            switch (conflictAngle) {
                case 0:
                case 2:
                    if (playerController.angleNumber == 1 || playerController.angleNumber == 3) {
                        Invalid_temporarily();
                    } else {
                        Decide_force_magnitude(collision.transform.position, collision.transform.rotation);
                    }
                    break;
                case 1:
                case 3:
                    if (playerController.angleNumber == 0 || playerController.angleNumber == 2) {
                        Invalid_temporarily();
                    } else {
                        Decide_force_magnitude(collision.transform.position, collision.transform.rotation);
                    }
                    break;

            }
            enterPole = collision.tag;
            isPoleEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == enterPole && isPoleEnter == true) {
            isPoleEnter = false;
            effectorEnabledTime = effectorEnabledCounter;
            playerController.Change_Normal_Sprite();
            Change_MyImage_Normal();
            playerController.isWallStick = false;
        }
    }

    //半透明にする関数
    private void Transparentize_half()
    {
        Color semitransparentColor = color;
        semitransparentColor.a = 0.5f;
        sprite.color = semitransparentColor;
        playerController.Change_Normal_Sprite();
        Change_MyImage_Normal();
    }

    //プレイヤーから磁界のない向きで近づかれたときに磁石の磁力を一時的に無効化させるふるまい
    //Stayのときに呼ばれるもので、プレイヤーが磁界から離れたら通常より早く復帰する。
    //中で半透明にする処理も読んでいる
    private void Invalid_temporarily()
    {
        effector2D.enabled = false;
        Transparentize_half();
        isPoleEnter = false;
        effectorEnabledTime = effectorEnabledCounter / 10;
    }

    //conflict_Angleに衝突してきた位置の情報を入れている
    // 0 : 右
    // 1 : 上
    // 2 : 左
    // 3 : 右
    private int Find_hit_angle()
    {
        distanceN = (transform.position - playerMagN.transform.position).sqrMagnitude;
        distanceS = (transform.position - playerMagS.transform.position).sqrMagnitude;

        //ここでなんと、プレイヤーと磁石のワールド座標での2点間の角度をとっている！！！！！！！！！！！！！！！！
        Vector3 diff = transform.position - player.transform.position;
        Vector3 axis = Vector3.Cross(player.transform.forward, diff);
        float angle = Vector3.Angle(player.transform.right * -1, diff) * (axis.x < 0 ? -1 : 1);

        if ((-45.5f <= angle && angle < 0) || (0 <= angle && angle < 45.5f)) {
            return 0;
        } else if (45.5f <= angle && angle < 135.5f) {
            return 1;
        } else if (135.5f <= angle || angle < -135.5f) {
            return 2;
        } else {
            return 3;
        }
    }

    //磁石の極とプレイヤーが近づけさせてきた極を参照して、吸引するか反発するか切り替えている。
    private void Decide_force_magnitude(Vector3 magTransForm, Quaternion magRotation)
    {
        if (Pole == MagPole.N_mag) {

            // 自分がN極で、プレイヤーのN極のほうが磁石と近かったら、反発モードにする。
            if (distanceN < distanceS) {
                effector2D.forceMagnitude = -myForceMagunitude;
                playerController.Change_Effectively_S_Pole();
                if (!playerController.GetIsRotating() && !isPoleEnter)
                {
                    Instantiate(RepulsionParticle, magTransForm, magRotation);

                }
                // S極のほうが近かったら吸引モード
            } else {
                effector2D.forceMagnitude = myForceMagunitude;
                playerController.Change_Effectively_N_Pole();
                playerController.isWallStick = true;
                Change_MyImage_Stick();
                
                
            }

        } else if (Pole == MagPole.S_mag) {
            // 自分がS極で、プレイヤーのN極のほうが磁石と近かったら、吸引モードにする。
            if (distanceN < distanceS) {
                effector2D.forceMagnitude = myForceMagunitude;
                playerController.Change_Effectively_N_Pole();
                playerController.isWallStick = true;
                Change_MyImage_Stick();
                
               
                // S極のほうが近かったら反発モード
            } else {
                effector2D.forceMagnitude = -myForceMagunitude;
                playerController.Change_Effectively_S_Pole();
                if (!playerController.GetIsRotating() && !isPoleEnter)
                {
                    Instantiate(RepulsionParticle, magTransForm, magRotation);

                }
            }
        }
    }

    private void Change_MyImage_Normal()
    {
        sprite.sprite = NormalImage;
    }

    private void Change_MyImage_Stick()
    {
        sprite.sprite = StickImage;
    }
    
    public bool IsMagPole_N()
    {
        if (Pole == MagPole.N_mag) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsMagPole_S()
    {
        if (Pole == MagPole.S_mag) {
            return true;
        } else {
            return false;
        }
    }
}
