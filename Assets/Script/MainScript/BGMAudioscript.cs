using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudioscript : MonoBehaviour
{
    [SerializeField]
    AudioClip[] BGMgroup;
    AudioSource aud;
    //void Awake()
    //{
    //    aud = GetComponent<AudioSource>();
    //    aud.loop = true;
    //    aud.playOnAwake = false;
    //    aud.volume = PlayerPrefs.GetFloat("BGM");
    //}
    
    public void SetBGM(SceneManagerScript maf,string name)
    {
        maf.GetBGMVolume();
        switch (name)
        {
            case "Title":
                aud.clip = BGMgroup[0];
                break;
            case "StageSelect":
                aud.clip = BGMgroup[1];
                break;
            case "Stage1_Red":
            case "Stage2_Orange":
            case "Stage3_Yellow":
            case "Stage4_Green":
            case "Stage5_Blue":
            case "Stage6_Indigo":
            case "Stage7_Purple":
                aud.clip = BGMgroup[2];
                break;
            case "Start":
                aud.clip = BGMgroup[3];
                break;
            case "Config":
                aud.clip = BGMgroup[4];
                break;
            case "GameClear":
                aud.clip = BGMgroup[5];
                break;
        }
        aud.Play();
    }
    public void SetVolume(float val)
    {
        aud = GetComponent<AudioSource>();
        aud.loop = true;
        aud.playOnAwake = false;
        aud.volume = val;
        
    }
}
