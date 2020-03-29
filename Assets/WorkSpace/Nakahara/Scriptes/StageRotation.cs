using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageRotation : MonoBehaviour
{
    //transform
    Transform myTransform;

    //回転状態
    private enum ANGLESTATE
    {
        DEGREE_NONE,
        DEGREE_TEN=1,
        DEGREE_THIRTY,
        DEGREE_FREE,
        DEGREE_MAX
    }
    private ANGLESTATE m_eAngleState;

    //回転量
    private float m_fRotateAmount;

    //回転量変更方向
    bool m_bUpChangeAngle;
    bool m_bDownChangeAngle;

    //デバック変数
    [SerializeField]
    private GameObject m_gAngleState;
    private Text m_tAngleStateText;

    //=============================================================
    // セッター
    //=============================================================

    //回転量
    private void SetRotateAmount(ANGLESTATE state)
    {
        switch (state)
        {
            case ANGLESTATE.DEGREE_NONE:
                m_fRotateAmount = 0;
                break;

            case ANGLESTATE.DEGREE_TEN:
                m_fRotateAmount = 10;
                break;

            case ANGLESTATE.DEGREE_THIRTY:
                m_fRotateAmount = 30;
                break;

            case ANGLESTATE.DEGREE_FREE:
                m_fRotateAmount = 1;
                break;
        }
    }

    //=============================================================
    // コンストラクタ
    //=============================================================
    private void Start()
    {
        //transform取得
        myTransform = this.transform;

        //回転状態初期化
        m_eAngleState = ANGLESTATE.DEGREE_FREE;

        //回転量初期化
        SetRotateAmount(m_eAngleState);

        //回転量変更方向初期化
        m_bUpChangeAngle = false;
        m_bDownChangeAngle = false;

        //デバック変数
        m_tAngleStateText = m_gAngleState.GetComponent<Text>();
    }

    //=============================================================
    // 更新
    //=============================================================
    private void Update()
    {
        //回転
        MoveRotate();

        //デバック
        DebugAngleState(m_eAngleState);
    }

    //=============================================================
    // 入力
    //=============================================================
    private void CheckUserInput()
    {
        switch (m_eAngleState)
        {
            case ANGLESTATE.DEGREE_FREE:
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    myTransform.Rotate(new Vector3(0, m_fRotateAmount, 0));
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    myTransform.Rotate(new Vector3(0, -m_fRotateAmount, 0));
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_bUpChangeAngle = true;
                    ChangeAngle();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_bDownChangeAngle = true;
                    ChangeAngle();
                }
                break;

            default:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    myTransform.Rotate(new Vector3(0, m_fRotateAmount, 0));
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    myTransform.Rotate(new Vector3(0, -m_fRotateAmount, 0));
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_bUpChangeAngle = true;
                    ChangeAngle();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_bDownChangeAngle = true;
                    ChangeAngle();
                }
                break;
        }
    }

    //=============================================================
    // 回転
    //=============================================================
    private void MoveRotate()
    {
        switch (m_eAngleState)
        {
            case ANGLESTATE.DEGREE_TEN:
                CheckUserInput();
                break;

            case ANGLESTATE.DEGREE_THIRTY:
                CheckUserInput();
                break;

            case ANGLESTATE.DEGREE_FREE:
                CheckUserInput();
                break;
        }
        
    }

    //=============================================================
    // 回転量変更
    //=============================================================
    private void ChangeAngle()
    {
        switch (m_eAngleState)
        {
            case ANGLESTATE.DEGREE_TEN:
                if (m_bUpChangeAngle)
                {
                    m_eAngleState = ANGLESTATE.DEGREE_THIRTY;
                    SetRotateAmount(m_eAngleState);
                    m_bUpChangeAngle = false;
                }

                if (m_bDownChangeAngle)
                {
                    m_eAngleState = ANGLESTATE.DEGREE_FREE;
                    SetRotateAmount(m_eAngleState);
                    m_bDownChangeAngle = false;
                }
                break;

            case ANGLESTATE.DEGREE_THIRTY:
                if (m_bUpChangeAngle)
                {
                    m_eAngleState = ANGLESTATE.DEGREE_FREE;
                    SetRotateAmount(m_eAngleState);
                    m_bUpChangeAngle = false;
                }

                if (m_bDownChangeAngle)
                {
                    m_eAngleState = ANGLESTATE.DEGREE_TEN;
                    SetRotateAmount(m_eAngleState);
                    m_bDownChangeAngle = false;
                }
                break;

            case ANGLESTATE.DEGREE_FREE:
                if (m_bUpChangeAngle)
                {
                    m_eAngleState = ANGLESTATE.DEGREE_TEN;
                    SetRotateAmount(m_eAngleState);
                    m_bUpChangeAngle = false;
                }

                if (m_bDownChangeAngle)
                {
                    m_eAngleState = ANGLESTATE.DEGREE_THIRTY;
                    SetRotateAmount(m_eAngleState);
                    m_bDownChangeAngle = false;
                }
                break;
        }
    }

    //=============================================================
    // デバック
    //=============================================================
    private void DebugAngleState(ANGLESTATE state)
    {
        switch (state)
        {
            case ANGLESTATE.DEGREE_NONE:
                m_tAngleStateText.text = "0";
                break;

            case ANGLESTATE.DEGREE_TEN:
                m_tAngleStateText.text = "10";
                break;

            case ANGLESTATE.DEGREE_THIRTY:
                m_tAngleStateText.text = "30";
                break;

            case ANGLESTATE.DEGREE_FREE:
                m_tAngleStateText.text = "FREE";
                break;
        }
    }
}
