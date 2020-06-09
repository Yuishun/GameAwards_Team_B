using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    GravityControllerScript GravityController;
    FreezeCarsorScript FreezeCarsor;
    SceneManagerScript sceneManagerScript;
    UIController uIController;
    bool m_bControll_ok = true;
    float NotOperation_Time = 0;
    float SetHidetime = 1;
    void Start()
    {
        gameObject.tag = "GameController";
        GravityController = transform.GetChild(1).transform.GetComponent<GravityControllerScript>();
        FreezeCarsor = transform.GetChild(2).transform.GetComponent<FreezeCarsorScript>();
        uIController = FreezeCarsor.transform.root.GetChild(0).GetComponent<UIController>();
        if (GameObject.FindWithTag("AllScene"))
        {
            sceneManagerScript = GameObject.FindWithTag("AllScene").GetComponent<SceneManagerScript>();
            SetHidetime = sceneManagerScript.GethideUItime();
        }
    }
    void Update()
    {
        if (m_bControll_ok)
        {
            //コントロールしていない時間を計測
            if (!MenuScript.m_bMenuOpen)
                NotOperation_Time += Time.deltaTime;
            //===============================================
            //ステージ回転(押下のみ判定)
            //===============================================
            // Rスティックかキーボード左右矢印
            float RstickH = Input.GetAxisRaw("R_Horizontal");
            float Trigger = Input.GetAxisRaw("LR_Trigger");
            if (RstickH < 0 || Trigger < 0 ||
                Input.GetButtonDown("Button_L"))//回転
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    GravityController.LeftRoll = true;
                }
            }
            else if (RstickH > 0 || Trigger > 0 ||
                Input.GetButtonDown("Button_R"))//回転
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    GravityController.RightRoll = true;
                }
            }
            else if (Input.GetButtonDown("Button_Y"))
            //回転角度変更
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    GravityController.RollAngleChange(true);
                }
            }
            else if (Input.GetButtonDown("Button_X"))
            //回転角度変更
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    GravityController.RollAngleChange(false);
                }
            }


            //===============================================
            //氷結・融解のカーソル(常に判定)
            //===============================================
            float LstickH = Input.GetAxis("L_Horizontal");
            float LstickV = Input.GetAxis("L_Vertical");
            float DpadH = Input.GetAxis("DPad_Horizontal");
            float DpadV = Input.GetAxis("DPad_Vertical");
            if (LstickH < 0 || DpadH < 0)//左
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    FreezeCarsor.ControllerColliderHit(1);
                    uIController.CarsorWay(1);
                }
            }
            else if (LstickH > 0 || DpadH > 0)//右
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    FreezeCarsor.ControllerColliderHit(2);
                    uIController.CarsorWay(2);
                }
            }
            if (LstickV > 0 || DpadV > 0)//上
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    FreezeCarsor.ControllerColliderHit(3);
                    uIController.CarsorWay(3);
                }
            }
            else if (LstickV < 0 || DpadV < 0)//下
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    FreezeCarsor.ControllerColliderHit(4);
                    uIController.CarsorWay(4);
                }
            }
            if (LstickH == 0 && DpadH == 0 && DpadV == 0 && LstickV == 0) 
            {
                if (!MenuScript.m_bMenuOpen)
                    uIController.CarsorWay(0);
            }
            //===============================================
            //氷結・融解(押下のみ判定)
            //===============================================
            if (Input.GetButtonDown("Button_A"))//凍結
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    FreezeCarsor.FreezeImage();
                }
            }
            if (Input.GetButtonDown("Button_B"))//溶解
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    FreezeCarsor.MeltIMage();
                }
            }

            FreezeCarsor.SetRot();

            //================================================
            // メニュー
            //================================================
            if (Input.GetButtonDown("Button_START"))
            {
                if (!MenuScript.m_bMenuOpen)
                {
                    NotOperation_Time = 0;
                    sceneManagerScript.Menu();
                }
            }
            if (NotOperation_Time > SetHidetime)
            {
                uIController.HideUI();
            }
            if (NotOperation_Time == 0)
            {
                uIController.ShowUI();
            }
        }
    }
    public void menucontroll(bool flag)
    {
        if (flag)
            Invoke("menuend", 0.3f);
        else
            m_bControll_ok = false;
    }
    void menuend()
    {
        m_bControll_ok = true;
    }
    public void NoControll()
    {
        m_bControll_ok = false;
        uIController.HideUI();
    }
}
