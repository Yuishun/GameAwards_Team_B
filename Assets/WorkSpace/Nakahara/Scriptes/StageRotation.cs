using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotation : MonoBehaviour
{
    //transform
    Transform myTransform = null;

    //回転量
    float[] m_fRotateAmount = { 10.0f, 30.0f };
    int m_iRotateAmountIndex = 0;
    //回転量変更
    bool m_bChangeAngle = false;

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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            myTransform.Rotate(new Vector3(0, m_fRotateAmount[m_iRotateAmountIndex], 0));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            myTransform.Rotate(new Vector3(0, -m_fRotateAmount[m_iRotateAmountIndex], 0));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!m_bChangeAngle)
            {
                m_iRotateAmountIndex = 1;
                m_bChangeAngle = true;
            }
            else
            {
                m_iRotateAmountIndex = 0;
                m_bChangeAngle = false;
            }
        }
    }
}
