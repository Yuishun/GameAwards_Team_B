using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField]
    AudioClip[] BGMgroup;
    static AudioClip playmusic;
    AudioSource BGMaudio;
    void Awake()
    {
        BGMaudio = GetComponent<AudioSource>();
        BGMaudio.loop = true;
        BGMaudio.volume = PlayerPrefs.GetFloat("BGM");
    }

    
    void Update()
    {
        
    }

    public void SetBGM(string Scenename)
    {
        switch (Scenename)
        {
            case "Title":
                playmusic = BGMgroup[0];
                break;
            case "StageSelect":
                playmusic = BGMgroup[1];
                break;
            case "Start":
                playmusic = BGMgroup[2];
                break;
            case "Stage1_Red":
            case "Stage2_Orange":
            case "Stage3_Yellow":
            case "Stage4_Green":
            case "Stage5_Blue":
            case "Stage6_Indigo":
            case "Stage7_Purple":
                playmusic = BGMgroup[3];
                break;
            case "GameClear":
                    playmusic = BGMgroup[4];
                break;
            case "Config":
                playmusic = BGMgroup[5];
                break;
        }
        BGMaudio.clip = playmusic;
        BGMaudio.Play();
    }
}
