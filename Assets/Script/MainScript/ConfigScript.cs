﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConfigScript : MonoBehaviour
{
    [SerializeField]
    AudioSource audio;
    SceneManagerScript managerScript;
    float BGMVolume;
    float SEVolume;
    bool ArrowFlag = false;
    bool ButtonFlag = false;
    int[] ButtonState = { 0, 0, 0, 0};
    [SerializeField]
    RectTransform arrow, BGMslider, SEslider;
    Slider BGMValue, SEValue;
    [SerializeField]
    Text numtext;
    Vector3 IconDistance;
    float timer = 0;
    [SerializeField,Range(0.15f,0.3f)]
    float interval_time;
    Text text;
    [SerializeField]
    Image img;
    [SerializeField]
    Sprite spriteA,spriteB;
    string Atex = "決定",Btex = "戻る";
    GameObject mute1, mute2;
    enum SelectState
    {
        BGMSlide,
        SESlide,
        Release,
        Return
    }
    SelectState Config_State = SelectState.BGMSlide;
    
    AudioClip buttonA, buttonB,warning;
    AudioSource audioButton;
    bool onebutton = false;
    void Start()
    {
        Config_State = SelectState.BGMSlide;
        BGMValue = BGMslider.GetComponent<Slider>();
        SEValue = SEslider.GetComponent<Slider>();

        var manage = GameObject.FindGameObjectWithTag("AllScene");
        if (manage)
        {
            managerScript = manage.GetComponent<SceneManagerScript>();
            var data = managerScript.GetClearData();
            var count = 0;
                for (int i = 0; i < data.Length / 2; i++)
            {
                if (data[i, 1] == 1)
                    count++;
            }
            numtext.text = count + "/7";
            
            BGMValue.value = managerScript.GetBGMVolume();
            SEValue.value = managerScript.GetSEVolume();
            var val = PlayerPrefs.GetFloat("BGM");
            if (BGMValue.value != val) {
                BGMValue.value = PlayerPrefs.GetFloat("BGM",1);
                SEValue.value = PlayerPrefs.GetFloat("SE", 1);
            }
            audio = manage.GetComponent<AudioSource>();
        }
        else
        {
            BGMValue.value = PlayerPrefs.GetFloat("BGM", 1);
            SEValue.value = PlayerPrefs.GetFloat("SE", 1);
        }
        IconDistance = BGMValue.transform.localPosition-SEValue.transform.localPosition;
        text = img.transform.GetChild(0).GetComponent<Text>();
        img.sprite = spriteA;
        mute1 = transform.GetChild(2).GetChild(5).gameObject;
        mute2 = transform.GetChild(3).GetChild(5).gameObject;
        if (BGMValue.value == 0)
            mute1.SetActive(true);
        else
            mute1.SetActive(false);
        if (SEValue.value == 0)
            mute2.SetActive(true);
        else
            mute2.SetActive(false);
        buttonA = Resources.Load<AudioClip>("Sound\\SE\\decision29");
        buttonB = Resources.Load<AudioClip>("Sound\\SE\\cancel2");
        warning = Resources.Load<AudioClip>("Sound\\SE\\warning2");
        audioButton = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Button_A"))
        {
            ArrowFlag = true;
            ButtonFlag = true;
        }
        if (Input.GetButtonDown("Button_B"))
        {
            if (onebutton)
            {
                onebutton = false;
                audioButton.PlayOneShot(buttonB);
            }
            ArrowFlag = false;
            ButtonFlag = false;
        }
        //===============================================
        //　Lst/Rst 判定
        //===============================================
        float LstickH = Input.GetAxis("L_Horizontal");
        float DpadH = Input.GetAxis("DPad_Horizontal");
        float LstickV = Input.GetAxis("L_Vertical");
        float DpadV = Input.GetAxis("DPad_Vertical");
        timer += Time.deltaTime;
        if (timer > interval_time)
        {
            timer = 0;

            if (!ArrowFlag)
            {
                if (!ButtonFlag)
                {
                    if (LstickV > 0 || DpadV > 0)//上
                    {
                        ButtonState[2] = 1;
                        ButtonState[3] = 0;
                        ArrowFlag = true;
                    }
                    else if (LstickV < 0 || DpadV < 0)//下
                    {
                        ButtonState[2] = 0;
                        ButtonState[3] = 1;
                        ArrowFlag = true;
                    }
                }
                else
                {
                    if (LstickH < 0 || DpadH < 0)//左
                    {
                        ButtonState[0] = 1;
                        ButtonState[1] = 0;
                        ArrowFlag = true;
                    }
                    else if (LstickH > 0 || DpadH > 0)//右
                    {
                        ButtonState[0] = 0;
                        ButtonState[1] = 1;
                        ArrowFlag = true;
                    }
                }
                switch (Config_State)
                {
                    case SelectState.BGMSlide:
                        {
                            if (ButtonFlag)
                            {
                                if (!onebutton)
                                {
                                    onebutton = true;
                                    audioButton.PlayOneShot(buttonA);
                                }
                                if (ButtonState[0] == 1)
                                    BGMChange(-0.1f);
                                if (ButtonState[1] == 1)
                                    BGMChange(0.1f);
                            }
                            else
                            if (ButtonState[3] == 1)
                            {
                                Config_State = SelectState.SESlide;
                                arrow.transform.localPosition -= IconDistance;
                            }
                            if (BGMValue.value == 0)
                                mute1.SetActive(true);
                            else
                                mute1.SetActive(false);
                        }
                        break;
                    case SelectState.SESlide:
                        {
                            if (ButtonFlag)
                            {
                                if (!onebutton)
                                {
                                    onebutton = true;
                                    audioButton.PlayOneShot(buttonA);
                                }
                                if (ButtonState[0] == 1)
                                    SEChange(-0.1f);
                                if (ButtonState[1] == 1)
                                    SEChange(0.1f);
                            }
                            else
                            if (ButtonState[2] == 1)
                            {
                                Config_State = SelectState.BGMSlide;
                                arrow.transform.localPosition += IconDistance;
                            }
                            else
                            if (ButtonState[3] == 1)
                            {
                                Config_State = SelectState.Release;
                                arrow.transform.localPosition -= IconDistance;
                            }
                            if (SEValue.value == 0)
                                mute2.SetActive(true);
                            else
                                mute2.SetActive(false);
                        }
                        break;
                    case SelectState.Release:
                        {
                            if (ButtonFlag)
                            {
                                audioButton.PlayOneShot(warning);
                                StageCountClear();
                                ButtonFlag = false;
                            }
                            else
                            if (ButtonState[2] == 1)
                            {
                                Config_State = SelectState.SESlide;
                                arrow.transform.localPosition += IconDistance;
                            }
                            else
                            if (ButtonState[3] == 1)
                            {
                                Config_State = SelectState.Return;
                                arrow.transform.localPosition -= IconDistance;
                            }
                        }
                        break;
                    case SelectState.Return:
                        {
                            if (ButtonFlag)
                            {
                                audioButton.PlayOneShot(buttonA);
                                ReturnSelect();
                                ButtonFlag = false;
                            }
                            else
                            if (ButtonState[2] == 1)
                            {
                                Config_State = SelectState.Release;
                                arrow.transform.localPosition += IconDistance;
                            }
                        }
                        break;
                }
                if (!ButtonFlag)
                {
                    img.sprite = spriteA;
                    text.text = Atex;
                }
                else
                {
                    img.sprite = spriteB;
                    text.text = Btex;
                }
            }
            else
            {
                if (DpadV == 0 && LstickV == 0 || LstickH == 0 && DpadH == 0)
                    ArrowFlag = false;
            }
        }
        if (LstickH == 0 && DpadH == 0)
        {
            ButtonState[0] = 0;
            ButtonState[1] = 0;
        }
        if (DpadV == 0 && LstickV == 0)
        {
            ButtonState[2] = 0;
            ButtonState[3] = 0;
        }
    }

    public void BGMChange(float val)
    {
        BGMVolume = BGMValue.value;
        if (0f <= BGMVolume && BGMVolume <= 1f)
        {
            if (BGMVolume == 0f)
            {
                if (val > 0)
                    BGMValue.value += val;
            }
            else
            if (BGMVolume == 1f)
            {
                if (val < 0)
                    BGMValue.value += val;
            }
            else
                BGMValue.value += val;
        }
        if (managerScript)
        {
            audio.volume = BGMValue.value;
            managerScript.SetSEVolume(BGMValue.value);
        }
    }
    public void SEChange(float val)
    {
        SEVolume = SEValue.value;
        if (0f <= SEVolume && SEVolume <= 1f)
        {
            if (SEVolume == 0f)
            {
                if (val > 0)
                    SEValue.value += val;
            }
            else
            if (SEVolume == 1f)
            {
                if (val < 0)
                    SEValue.value += val;
            }
            else
                SEValue.value += val;
        }
        if (managerScript)
            managerScript.SetBGMVolume(SEValue.value);
    }
    public void StageCountClear()
    {
        numtext.text = "0/7";
        if (managerScript)
            managerScript.DeleteClearData();
    }

    public void ReturnSelect()
    {
        if (managerScript)
        {
            managerScript.SetBGMVolume(BGMValue.value);
            managerScript.SetSEVolume(SEValue.value);
            PlayerPrefs.Save();
            managerScript.Loadstagenum(500);
        }
        else
        {
            PlayerPrefs.SetFloat("BGM", BGMValue.value);
            PlayerPrefs.SetFloat("SE", SEValue.value);
        }
    }
}
