using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOnSelf : MonoBehaviour
{
    Selectable sel;

    void Start()
    {
        // 自分を選択状態にする
        sel = GetComponent<Selectable>();
        sel.Select();
    }

    private void OnEnable()
    {
        sel.Select();
    }
}