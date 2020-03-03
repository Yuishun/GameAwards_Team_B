using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    //transform
    Transform myTransform = null;

    //回転量
    float m_fRotateAmount = 1.0f;

    //=============================================================
    // コンストラクタ
    //=============================================================
    private void Start()
    {
        //transform取得
        myTransform = this.transform;
    }

    //=============================================================
    // 更新
    //=============================================================
    private void Update()
    {
        //回転
        Rotation();
    }

    //=============================================================
    // 回転
    //=============================================================
    private void Rotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            myTransform.Rotate(new Vector3(0, 0, m_fRotateAmount));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            myTransform.Rotate(new Vector3(0, 0, -m_fRotateAmount));
        }
    }
}
