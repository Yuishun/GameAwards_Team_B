using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    GravityControllerScript GravityController;
    FreezeCarsorScript FreezeCarsor;

    void Start()
    {
        GravityController = transform.GetChild(0).transform.GetComponent<GravityControllerScript>();
        FreezeCarsor = transform.GetChild(1).transform.GetComponent<FreezeCarsorScript>();
    }
    void Update()
    {
        //===============================================
        //ステージ回転(押下のみ判定)
        //===============================================
        // Rスティックかキーボード左右矢印
        float RstickH = Input.GetAxisRaw("R_Horizontal");
        float RstickV = Input.GetAxisRaw("R_Vertical");
        float Trigger = Input.GetAxisRaw("LR_Trigger");
        if (RstickH < 0 || Trigger < 0 ||
            Input.GetButtonDown("Button_L"))//回転
        {
            GravityController.LeftRoll = true;
        }
        else if (RstickH > 0 || Trigger > 0 ||
            Input.GetButtonDown("Button_R"))//回転
        {
            GravityController.RightRoll = true;
        }
        else if (Input.GetButtonDown("Button_Y") ||
            RstickV > 0)//回転角度変更
        {
            GravityController.RollAngleChange(true);
        }
        else if (Input.GetButtonDown("Button_X") ||
            RstickV < 0)//回転角度変更
        {
            GravityController.RollAngleChange(false);
        }


        //===============================================
        //氷結・融解のカーソル(常に判定)
        //===============================================
        float LstickH = Input.GetAxis("L_Horizontal");
        float LstickV = Input.GetAxis("L_Vertical");
        if (LstickH < 0)//左
        {
            FreezeCarsor.ControllerColliderHit(1);
        }
        else if (LstickH > 0)//右
        {
            FreezeCarsor.ControllerColliderHit(2);
        }
        if (LstickV > 0)//上
        {
            FreezeCarsor.ControllerColliderHit(3);
        }
        else if (LstickV < 0)//下
        {
            FreezeCarsor.ControllerColliderHit(4);
        }
        //===============================================
        //氷結・融解(押下のみ判定)
        //===============================================
        if (Input.GetButtonDown("Button_A"))//凍結
        {
            FreezeCarsor.FreezeImage();
        }
        if (Input.GetButtonDown("Button_B"))//溶解
        {
            FreezeCarsor.MeltIMage();
        }

        //================================================
        // メニュー
        //================================================
        if (Input.GetButtonDown("Button_START"))
        {

        }
    }
}
