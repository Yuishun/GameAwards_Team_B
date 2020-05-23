using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScript : MonoBehaviour
{
    [SerializeField]
    public SceneManagerScript allScene;
    [SerializeField]
    public GameObject prophecy;
    int[,] ClearStageNum;
    bool ButtonFlag = false;
    bool CheckFind_SceneManager = false;
    bool m_bMenuOpen = false;
    [SerializeField, Header("クリア時生成エフェクト")]
    GameObject ClearEffectObj;
    [SerializeField, Header("クリア時生成エフェクトColor")]
    ParticleSystem.MinMaxGradient[] colors;
    static Transform[] StageIcon;
    GameObject BGSea;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("AllScene"))
        {
            allScene = GameObject.FindGameObjectWithTag("AllScene").transform.GetComponent<SceneManagerScript>();
            ClearStageNum = allScene.GetClearData();
            BGSea = GameObject.FindWithTag("BG").gameObject;
            ClearStageCheck();
            CheckFind_SceneManager = true;
        }
        if (!CheckFind_SceneManager)
            Debug.Log("シーンマネージャーがないよ！");
    }
    //===================================================
    // ステージクリア状態チェック
    //===================================================
    void ClearStageCheck()
    {
        int clearNum = 0;
        int num = 0;
        while (num < ClearStageNum.Length / 2)
        {
            switch (ClearStageNum[num, 1])
            {
                case 0:
                    break;
                case 1:
                    clearNum++;
                    StartCoroutine(StageClear(num));
                    break;
            }
            num++;
        }
        if (clearNum <= 7)
            ButtonFlag = false;
    }
    //===================================================
    // Update
    //===================================================
    void Update()
    {
        //Menu
        if (Input.GetButtonDown("Button_START"))
        {
            if (!m_bMenuOpen)
            {
                if (ButtonFlag)
                {
                    allScene.Menu();
                    m_bMenuOpen = !m_bMenuOpen;
                }
            }
            else
            {
                m_bMenuOpen = !m_bMenuOpen;
            }
        }
        //StageNext
        else if (Input.GetButtonDown("Button_A"))
        {
            if (!m_bMenuOpen)
                if (ButtonFlag)
                {
                    if (CheckFind_SceneManager)
                    {
                        int clearnum = 0;
                        int num = 0;
                        while (num < ClearStageNum.Length / 2)
                        {
                            switch (ClearStageNum[num, 1])
                            {
                                case 0:
                                    break;
                                case 1:
                                    clearnum++;
                                    break;
                            }
                            num++;
                        }
                        StageSelect(clearnum + 1);
                    }
                    else
                        Debug.Log("シーンマネージャーがないよ！");
                }
        }
        if (!prophecy.activeSelf)
            ButtonFlag = true;
        //==*************************************************************
        // Debug用ステージセレクト
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StageSelect(1);
        else
        if (Input.GetKeyDown(KeyCode.Alpha2))
            StageSelect(2);
        else
        if (Input.GetKeyDown(KeyCode.Alpha3))
            StageSelect(3);
        else
        if (Input.GetKeyDown(KeyCode.Alpha4))
            StageSelect(4);
        else
        if (Input.GetKeyDown(KeyCode.Alpha5))
            StageSelect(5);
        else
        if (Input.GetKeyDown(KeyCode.Alpha6))
            StageSelect(6);
        else
        if (Input.GetKeyDown(KeyCode.Alpha7))
            StageSelect(7);
        //==*************************************************************
    }
    public void SelectButtonActivateion()
    {
        if (SceneManagerScript.m_bFadeInEnd)
        {
            ButtonFlag = true;
        }
    }
    //===================================================
    // ステージ選択用
    //===================================================
    public void StageSelect(int Stagenam)
    {
        if (Stagenam == 8)
            Stagenam = 100;

        if (allScene)
            allScene.Loadstagenum(Stagenam);
    }

    //===================================================
    // ステージクリア直後処理
    //===================================================
    IEnumerator OneStageClear()
    {
        yield return new WaitForEndOfFrame();
    }
    //===================================================
    // クリアステージ処理
    //===================================================
    IEnumerator StageClear(int val)
    {
        var pos = BGSea.transform.GetChild(0).GetChild(val).transform.position;
        var obj = Instantiate(ClearEffectObj, pos,Quaternion.identity).gameObject.GetComponent<ParticleSystem>().main;
        switch (val)
        {
            case 0:
                obj.startColor = colors[0];
                break;
            case 1:
                obj.startColor = colors[1];
                break;
            case 2:
                obj.startColor = colors[2];
                break;
            case 3:
                obj.startColor = colors[3];
                break;
            case 4:
                obj.startColor = colors[4];
                break;
            case 5:
                obj.startColor = colors[5];
                break;
            case 6:
                obj.startColor = colors[6];
                break;
        }
        yield return new WaitForEndOfFrame();
    }


    public int BackClearStageNum()
    {

        int clearNum = 0;
        int num = 0;
        if (CheckFind_SceneManager)
        {
            while (num < ClearStageNum.Length / 2)
            {
                if (ClearStageNum[num, 1] == 1)
                    clearNum++;
                num++;
            }
        }
        return clearNum;
    }
}
