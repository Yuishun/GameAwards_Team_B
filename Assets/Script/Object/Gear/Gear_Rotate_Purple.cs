﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_Rotate_Purple : MonoBehaviour
{
    [SerializeField]
    Transform[] Rotate;
    [SerializeField]
    float[] RotSpeed;
    //Vector2 Angle = Vector2.right;
    //float a = 0.01f;
    //
    //float cos, sin;
    float PrevAngle;

    // Start is called before the first frame update
    void Start()
    {
        //cos = Mathf.Cos(a);
        //sin = Mathf.Sin(a);
        PrevAngle = 0;
        if (Rotate.Length != RotSpeed.Length)
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(PrevAngle - 2 > transform.localEulerAngles.z
            || PrevAngle + 2 < transform.localEulerAngles.z)
        {
            //float x1 = Circle.position.x, y1 = Circle.position.y;
            //float x2 = x1 * cos - y1 * sin;
            //float y2 = x1 * sin + y1 * cos;
            //Circle.MovePosition(new Vector2(x2, y2));
            for(int i = 0; i < Rotate.Length; i++)
                Rotate[i].RotateAround(Vector3.zero, Vector3.forward, RotSpeed[i]);

            PrevAngle = transform.localEulerAngles.z;
        }
    }
}
