using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class test : MonoBehaviour
{
    //カメラオブジェクト
    public GameObject mainCamera;
    //プレイヤーオブジェクト
    private GameObject Player;
    //z軸を調整。正の数ならプレイヤーの前に、負の数ならプレイヤーの後ろに配置する
    public int zAdjust = 5;
    //X座調整
    public float X_camera = 0.0f;

    void Start()
    {
        Player= GameObject.Find("Mairo");
    }

    void Update()
    {
        if (Player.transform.position.x > 0)
        {
            //カメラはプレイヤーと同じ位置にする
            mainCamera.transform.position = new Vector3(Player.transform.position.x + X_camera, 0, Player.transform.position.z + zAdjust);
            //mainCamera.transform.Rotate(0.0f, 0.0f, 0.0f);
        }
    }

}