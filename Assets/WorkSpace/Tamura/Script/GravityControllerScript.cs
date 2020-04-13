﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControllerScript : MonoBehaviour
{
    private Vector3 localGravity;
    Transform cam_root;
    private bool RollerFlag = false;
    enum RollAngle
    {
        Ten = 10,
        Fifteen = 15,
        Twenty = 20,
    }
    RollAngle rollAngle = RollAngle.Ten;
    enum RollWay
    {
        normal = 0,
        right = 1,
        left = 2
    }
    RollWay rollWay = RollWay.normal;
    float timer = 0;
    public bool RightRoll = false;
    public bool LeftRoll = false;
    void Start()
    {
        cam_root = Camera.main.transform.root;
        localGravity = Physics2D.gravity;
    }

    void Update()
    {
        if (RightRoll == LeftRoll)
        {
            RightRoll = LeftRoll = false;
        }
        else
            if (!RollerFlag)
        {
            if (LeftRoll) rollWay = RollWay.left;
            if (RightRoll) rollWay = RollWay.right;

            RollerFlag = true;
        }
    }
    void FixedUpdate()
    {
        if (RollerFlag)
            Roll();
    }
    void Roll()
    {
        timer += Time.deltaTime;
        if (timer <= 1)
        {
            if (rollWay == RollWay.right)
                cam_root.transform.Rotate(new Vector3(0, 0, (int)rollAngle) * Time.deltaTime);
            if (rollWay == RollWay.left)
                cam_root.transform.Rotate(new Vector3(0, 0, -(int)rollAngle) * Time.deltaTime);
            Physics2D.gravity = localGravity.y * cam_root.up;
        }
        if (timer >= 1)
        {
            timer = 0;
            RollerFlag = false;
            RightRoll = false;
            LeftRoll = false;
        }
    }

    public void RollAngleChange(bool Flag)
    {
        if (!RollerFlag)
            if (Flag)
            {
                switch (rollAngle)
                {
                    case RollAngle.Ten:
                        rollAngle = RollAngle.Fifteen;
                        break;
                    case RollAngle.Fifteen:
                        rollAngle = RollAngle.Twenty;
                        break;
                    case RollAngle.Twenty:
                        rollAngle = RollAngle.Ten;
                        break;
                }
            }
            else
            {
                switch (rollAngle)
                {
                    case RollAngle.Ten:
                        rollAngle = RollAngle.Twenty;
                        break;
                    case RollAngle.Fifteen:
                        rollAngle = RollAngle.Ten;
                        break;
                    case RollAngle.Twenty:
                        rollAngle = RollAngle.Fifteen;
                        break;
                }
            }
    }
}