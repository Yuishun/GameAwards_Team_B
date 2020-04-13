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
        if (Input.GetKeyDown(KeyCode.LeftArrow))//回転
        {
            GravityController.LeftRoll = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))//回転
        {
            GravityController.RightRoll = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))//回転角度変更
        {
            GravityController.RollAngleChange(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))//回転角度変更
        {
            GravityController.RollAngleChange(false);
        }
        //===============================================
        //氷結・融解のカーソル(常に判定)
        //===============================================
        if (Input.GetKey(KeyCode.A))//左
        {
            FreezeCarsor.ControllerColliderHit(1);
        }
        if (Input.GetKey(KeyCode.D))//右
        {
            FreezeCarsor.ControllerColliderHit(2);
        }
        if (Input.GetKey(KeyCode.W))//上
        {
            FreezeCarsor.ControllerColliderHit(3);
        }
        if (Input.GetKey(KeyCode.S))//下
        {
            FreezeCarsor.ControllerColliderHit(4);
        }
        //===============================================
        //氷結・融解(押下のみ判定)
        //===============================================
        if (Input.GetKeyDown(KeyCode.R))//凍結
        {
            FreezeCarsor.FreezeImage();
        }
        if (Input.GetKeyDown(KeyCode.T))//溶解
        {
            FreezeCarsor.MeltIMage();
        }
    }
}
