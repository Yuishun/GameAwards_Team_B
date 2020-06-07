using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional_LightScript : MonoBehaviour
{
    Camera camera;
    Light light;
    SpriteRenderer Island;
    Color32[] Colors = new Color32[] {
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),
            new Color32( 50,   0,   0, 255),
            new Color32(125,   0,  50, 255),
            new Color32(150,  50,   0, 255),
            new Color32(125,  90,   0, 255),
            new Color32(100, 100,  80, 255),
            new Color32(100,  90, 100, 255),
            new Color32( 90,  90,  90, 255),
            new Color32( 80,  80,  80, 255),
            new Color32( 90,  90,  90, 255),
            new Color32(100, 100, 100, 255),
            new Color32(150, 100, 100, 255),
            new Color32(155,  90, 100, 255),
            new Color32(125,  55,  50, 255),
            new Color32(100,   0,  90, 255),
            new Color32( 50,   0,  50, 255),
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),
            new Color32(  0,   0,   0, 255),};
    void Start()
    {
        Island = GameObject.FindGameObjectWithTag("BG").transform.GetChild(0).GetComponent<SpriteRenderer>();
        camera = Camera.main.GetComponent<Camera>();
        var high = transform.position.y;
        DateTime NowTime = DateTime.Now;
        
        var angle = 360 / 24;
        angle *= (NowTime.Hour + 12);
        transform.RotateAround(Vector3.zero, Vector3.forward, angle);
        transform.LookAt(new Vector3(0, 0, high * 0.8f));
        light = transform.GetComponent<Light>();

        light.color = Colors[NowTime.Hour];
        camera.backgroundColor = light.color;
        Island.color = Color.Lerp(Color.white, new Color(255, 200, 200), NowTime.Hour / 24);
        
    }
    /*
    void Update()
    {
        var min = DateTime.Now;

        var sti = min.Millisecond * 0.001f;

        var prev = (min.Second) % 24;
        var next = (min.Second + 1) % 24;
        
        light.color = Color32.Lerp(Colors[prev], Colors[next], sti);
        camera.backgroundColor = light.color;
        Island.color = Color.Lerp(Color.white, new Color(255, 200, 200), (prev) / 24);
    }
    */
}
