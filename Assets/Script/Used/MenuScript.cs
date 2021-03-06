﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class MenuScript : MonoBehaviour
{
    [SerializeField]
    SceneManagerScript sceneManagerScript;
    [SerializeField]
    public static bool m_bMenuOpen = false;
    bool InStage = false;
    int value = 0;
    enum MenuState
    {
        Controll = 0,
        Config = 1,
        Title = 2,
        Close = 3,
        Reset = 4,
        ReSelect = 5
    }
    MenuState State = MenuState.Controll;
    [SerializeField]
    Transform TargetMenu;
    [SerializeField]
    Transform ControllerCanvas;
    [SerializeField]
    Image image1, image2, image3, image4;
    Color color1, color2, color3, color4;
    Color color1b, color2b, color3b, color4b;
    float beforeAxis = 0;
    InputController inputter;
    bool ControllerMenuFlag = false, MovieFlag = false;
    AudioSource audio;
    AudioClip SoundBA, SoundBB;
    StageSelectScript stageselect;
    VideoPlayer PVplayer;
    void Awake()
    {
        sceneManagerScript = transform.GetComponent<SceneManagerScript>();
        audio = transform.GetChild(0).gameObject.AddComponent<AudioSource>();
        audio.loop = false;
        audio.playOnAwake = false;
        audio.volume = sceneManagerScript.GetSEVolume();
        SoundBA = Resources.Load<AudioClip>("Sound\\SE\\decision29");
        SoundBB = Resources.Load<AudioClip>("Sound\\SE\\cancel2");
        PVplayer = ControllerCanvas.GetChild(3).GetComponent<VideoPlayer>();
    }
    void OnEnable()
    {
        m_bMenuOpen = true;
        if (SceneManagerScript.m_bMenu_InStage)
        {
            InStage = true;
            State = MenuState.Reset;
            TargetMenu = sceneManagerScript.StageMenu.transform;
            inputter = GameObject.FindGameObjectWithTag("GameController").GetComponent<InputController>();
        }
        else
        {
            stageselect = GameObject.Find("StageSlect").GetComponent<StageSelectScript>();
            InStage = false;
            State = MenuState.Controll;
            TargetMenu = sceneManagerScript.SelectMenu.transform;
        }
        if (InStage)
            inputter.menucontroll(false);

        image1 = TargetMenu.GetChild(1).GetComponent<Image>();
        image2 = TargetMenu.GetChild(2).GetComponent<Image>();
        image3 = TargetMenu.GetChild(3).GetComponent<Image>();
        image4 = TargetMenu.GetChild(4).GetComponent<Image>();
        color1 = image1.color;
        color2 = image2.color;
        color3 = image3.color;
        color4 = image4.color;
        color1b = image1.color - new Color32(70, 70, 70, 0);
        color2b = image2.color - new Color32(70, 70, 70, 0);
        color3b = image3.color - new Color32(70, 70, 70, 0);
        color4b = image4.color - new Color32(70, 70, 70, 0);

        value = 0;
        audio.PlayOneShot(SoundBA);
    }
    void OnDisable()
    {
        if (InStage)
            inputter.menucontroll(true);
        m_bMenuOpen = false;
        image1.color = color1;
        image2.color = color2;
        image3.color = color3;
        image4.color = color4;
    }
    void Update()
    {
        if (!ControllerMenuFlag)
        {
            float Lstick = Input.GetAxis("L_Vertical");
            if (beforeAxis == 0 && 0 != Lstick)
            {
                if (Lstick > 0)
                {
                    if (--value < 0) value = 3;
                }
                else if (Lstick < 0)
                {
                    if (++value > 3) value = 0;
                }
            }
            beforeAxis = Lstick;
            SelectLighting();


            if (InStage)
            {
                switch (value)
                {
                    case 0:
                        State = MenuState.Reset;
                        break;
                    case 1:
                        State = MenuState.ReSelect;
                        break;
                    case 2:
                        State = MenuState.Controll;
                        break;
                    case 3:
                        State = MenuState.Close;
                        break;
                }
            }
            else
            {
                switch (value)
                {
                    case 0:
                        State = MenuState.Controll;
                        break;
                    case 1:
                        State = MenuState.Config;
                        break;
                    case 2:
                        State = MenuState.Title;
                        break;
                    case 3:
                        State = MenuState.Close;
                        break;
                }
            }
            if (Input.GetButtonDown("Button_START"))
                sceneManagerScript.MenuEnd();
            else if (Input.GetButtonDown("Button_A"))
            {
                switch (State)
                {
                    case MenuState.Controll:
                        audio.PlayOneShot(SoundBA);
                        ControllerCanvas.gameObject.SetActive(true);
                        ControllerCanvas.GetChild(2).gameObject.SetActive(true);
                        if (!ControllerMenuFlag) ControllerMenuFlag = true;
                        break;
                    case MenuState.Config:
                        audio.PlayOneShot(SoundBA);
                        sceneManagerScript.Loadstagenum(600);
                        break;
                    case MenuState.Title:
                        audio.PlayOneShot(SoundBA);
                        sceneManagerScript.Loadstagenum(1000);
                        break;
                    case MenuState.Close:
                        if (!InStage)
                            stageselect.MenuClose_ButtonControllOK();
                        audio.PlayOneShot(SoundBB);
                        sceneManagerScript.MenuEnd();
                        break;
                    case MenuState.Reset:
                        audio.PlayOneShot(SoundBA);
                        sceneManagerScript.ReStage();
                        break;
                    case MenuState.ReSelect:
                        audio.PlayOneShot(SoundBA);
                        sceneManagerScript.Loadstagenum(500);
                        break;
                }
                if (!ControllerMenuFlag)
                    sceneManagerScript.MenuEnd();
            }
        }
        else if (MovieFlag)
        {
            if (Input.GetButtonDown("Button_A") || Input.GetButtonDown("Button_B")
                || Input.GetButtonDown("Button_X") || Input.GetButtonDown("Button_Y"))
            {
                audio.PlayOneShot(SoundBB);
                PVplayer.Stop();
                PVplayer.gameObject.SetActive(false);
                GetComponent<AudioSource>().Play();
                MovieFlag = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Button_A"))
            {
                audio.PlayOneShot(SoundBB);
                ControllerCanvas.GetChild(2).gameObject.SetActive(false);
                ControllerCanvas.gameObject.SetActive(false);
                if (ControllerMenuFlag) ControllerMenuFlag = false;
            }
            else if (Input.GetButtonDown("Button_X"))
            {
                audio.PlayOneShot(SoundBA);
                PVplayer.gameObject.SetActive(true);
                GetComponent<AudioSource>().Pause();
                if (!MovieFlag)
                    MovieFlag = true;
            }
        }
    }

    void SelectLighting()
    {
        if (value == 0)
        {
            image1.color = color1;
            image2.color = color2b;
            image3.color = color3b;
            image4.color = color4b;
        }
        else
            if (value == 1)
        {
            image1.color = color1b;
            image2.color = color2;
            image3.color = color3b;
            image4.color = color4b;
        }
        else
            if (value == 2)
        {
            image1.color = color1b;
            image2.color = color2b;
            image3.color = color3;
            image4.color = color4b;
        }
        else
            if (value == 3)
        {
            image1.color = color1b;
            image2.color = color2b;
            image3.color = color3b;
            image4.color = color4;
        }
    }
}