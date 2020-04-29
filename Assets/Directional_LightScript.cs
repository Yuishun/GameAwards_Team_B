using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional_LightScript : MonoBehaviour
{
    void Start()
    {
        var high = transform.position.y;
        DateTime NowTime = DateTime.Now;
        var angle = 360 / 24;
        angle *= (NowTime.Hour + 12);
        transform.RotateAround(Vector3.zero, Vector3.forward, angle);
        transform.LookAt(new Vector3(0, 0, high * 0.8f));
        var light = transform.GetComponent<Light>();
        light.color = new Color32(125, 55, 50, 255);
        switch (NowTime.Hour)
        {
            case  0: light.color = new Color32(  0,   0,   0, 255); break;
            case  1: light.color = new Color32(  0,   0,   0, 255); break;
            case  2: light.color = new Color32(  0,   0,   0, 255); break;
            case  3: light.color = new Color32(  0,   0,   0, 255); break;
            case  4: light.color = new Color32(  0,   0,   0, 255); break;
            case  5: light.color = new Color32( 50,   0,   0, 255); break;
            case  6: light.color = new Color32(125,   0,  50, 255); break;
            case  7: light.color = new Color32(150,  50,   0, 255); break;
            case  8: light.color = new Color32(125,  90,   0, 255); break;
            case  9: light.color = new Color32(100, 100,  80, 255); break;
            case 10: light.color = new Color32(100,  90, 100, 255); break;
            case 11: light.color = new Color32( 90,  90,  90, 255); break;
            case 12: light.color = new Color32( 80,  80,  80, 255); break;
            case 13: light.color = new Color32( 90,  90,  90, 255); break;
            case 14: light.color = new Color32(100, 100, 100, 255); break;
            case 15: light.color = new Color32(150, 100, 100, 255); break;
            case 16: light.color = new Color32(155,  90, 100, 255); break;
            case 17: light.color = new Color32(125,  55,  50, 255); break;
            case 18: light.color = new Color32(100,   0,  90, 255); break;
            case 19: light.color = new Color32( 50,   0,  50, 255); break;
            case 20: light.color = new Color32(  0,   0,   0, 255); break;
            case 21: light.color = new Color32(  0,   0,   0, 255); break;
            case 22: light.color = new Color32(  0,   0,   0, 255); break;
            case 23: light.color = new Color32(  0,   0,   0, 255); break;
        }
    }
}
