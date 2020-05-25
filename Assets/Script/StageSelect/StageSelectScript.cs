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
    [SerializeField]
    Material Cloudmat, Seamat;
    ParticleSystem RainParticle;
    [SerializeField]
    private float ClearAnimationTime = 5;
    [SerializeField]
    private float WaveHeight = 0.7f;
    
    void Start()
    {
        BGSea = GameObject.FindWithTag("BG").gameObject;
        if (GameObject.FindGameObjectWithTag("AllScene"))
        {
            allScene = GameObject.FindGameObjectWithTag("AllScene").transform.GetComponent<SceneManagerScript>();
            ClearStageNum = allScene.GetClearData();
            ClearStageCheck();
            CheckFind_SceneManager = true;
        }
        if (!CheckFind_SceneManager)
            Debug.Log("シーンマネージャーがないよ！");
        if (Cloudmat)
        {
            Cloudmat.EnableKeyword("cloudClear");
            Cloudmat.SetFloat("_cloudClear", 0);
        }
        if (Seamat)
        {
            Seamat.EnableKeyword("WaveHeight");
            Seamat.SetFloat("_WaveHeight", WaveHeight);
        }
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
            if (ClearStageNum[num, 1] == 1)
            {
                clearNum++;
                StartCoroutine(StageClear(num));
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
                m_bMenuOpen = !m_bMenuOpen;
        }
        //StageNext
        else if (Input.GetButtonDown("Button_A"))
        {
            if (!m_bMenuOpen)
                if (ButtonFlag)
                {
                    if (CheckFind_SceneManager)
                    {
                        StageSelect(BackClearStageNum() + 1);
                    }
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
        
        if (Input.GetKeyDown(KeyCode.Alpha7))
            StageSelect(8);
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
    // ステージ選択用(シーンロード指定)
    //===================================================
    public void StageSelect(int Stagenam)
    {
        if (Stagenam == 8)
            StartCoroutine("AllStageClear");
        else
        if (allScene)
            allScene.Loadstagenum(Stagenam);
    }

    //===================================================
    // 全ステージクリア直後処理
    //===================================================
    IEnumerator AllStageClear()
    {
        Light directlight = GameObject.FindGameObjectWithTag("Light").transform.GetComponent<Light>();
        Camera cam = Camera.main.gameObject.GetComponent<Camera>();
        var nowcolor = directlight.color;
        var lightpos = directlight.transform.position;
        RainParticle = BGSea.transform.GetChild(1).GetComponent<ParticleSystem>();
        var emission = RainParticle.emission;
        Cloudmat.EnableKeyword("_cloudClear");
        Seamat.EnableKeyword("WaveHeight");
        var val = 0f;
        while (val <= ClearAnimationTime) 
        {
            val += Time.deltaTime;
            var count = val / ClearAnimationTime;
            if (count < 1)
            {
                directlight.transform.position = Vector3.Lerp(lightpos, new Vector3(0, 50, -10), count);
                directlight.color = Color.Lerp(nowcolor, Color.white, count);
                cam.backgroundColor = directlight.color;
                emission.rateOverTime = 300 - 300 * ((val + 1.5f)/ ClearAnimationTime);
                Cloudmat.SetFloat("_cloudClear", count);
                Seamat.SetFloat("_WaveHeight", WaveHeight + 0.1f - WaveHeight * count);
            }
            yield return new WaitForEndOfFrame();
        }
        if (allScene)
            allScene.Loadstagenum(100);
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
