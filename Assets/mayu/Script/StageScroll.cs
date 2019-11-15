using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class StageScroll : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    public float scrollSpeedX = 0.1f;
    [SerializeField]
    private float scrollSpeedY = 0.1f;

    void Start()
    {
        player = GameObject.Find("mairo");
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", Vector2.zero);
    }

    void Update()
    {
        var x = Mathf.Repeat(player.transform.position.x * scrollSpeedX, 1);
        var y = Mathf.Repeat(player.transform.position.y * scrollSpeedY, 1);
        var offset = new Vector2(y,x);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}