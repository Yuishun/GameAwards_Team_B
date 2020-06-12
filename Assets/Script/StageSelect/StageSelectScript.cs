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
    GameObject BGSea;
    [SerializeField]
    Material Cloudmat, Seamat;
    ParticleSystem RainParticle;
    [SerializeField]
    private float ClearAnimationTime = 5;
    [SerializeField]
    private float WaveHeight = 0.7f;
    ParticleSystem[] rainbows = new ParticleSystem[] { };
    public static ParticleSystem.MinMaxGradient nowStageColor;
    SpriteRenderer[] islandstage;
    Color StageIconColor = new Color(180, 180, 180);
    [SerializeField]
    Color[] Stagecolors;
    void Start()
    {
        BGSea = GameObject.FindWithTag("BG").gameObject;
        islandstage = new SpriteRenderer[7]
{
        BGSea.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>(),
        BGSea.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>(),
        BGSea.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>(),
        BGSea.transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>(),
        BGSea.transform.GetChild(0).GetChild(4).GetComponent<SpriteRenderer>(),
        BGSea.transform.GetChild(0).GetChild(5).GetComponent<SpriteRenderer>(),
        BGSea.transform.GetChild(0).GetChild(6).GetComponent<SpriteRenderer>()
};
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
                StageClear(num);
                islandstage[num].color = Stagecolors[num];
            }
            else
            {
                islandstage[num].color = Color.white;
            }
            num++;
            if (num < ClearStageNum.Length / 2)
                islandstage[num].color = StageIconColor;
        }
        if (clearNum < 7)
            nowStageColor = colors[clearNum];
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
        for (int i = 0; i < rainbows.Length; i++)
            rainbows[i].gameObject.SetActive(true);
        
        Light directlight = GameObject.FindGameObjectWithTag("Light").transform.GetComponent<Light>();
        Camera cam = Camera.main.gameObject.GetComponent<Camera>();
        var nowcolor = directlight.color;
        var lightpos = directlight.transform.position;
        RainParticle = BGSea.transform.GetChild(1).GetComponent<ParticleSystem>();
        var emission = RainParticle.emission;
        Cloudmat.EnableKeyword("_cloudClear");
        Seamat.EnableKeyword("WaveHeight");
        yield return new WaitForSeconds(5f);
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
                for (int i = 0; i < rainbows.Length; i++)
                    rainbows[i].transform.localScale =
                        new Vector3(rainbows[i].transform.localScale.x,
                        Mathf.Lerp(rainbows[i].transform.localScale.y, 0.5f, count),
                        rainbows[i].transform.localScale.z);
            }

            yield return new WaitForEndOfFrame();
        }
        if (allScene)
            allScene.Loadstagenum(100);
    }
    //===================================================
    // クリアステージ処理
    //===================================================
    void StageClear(int val)
    {
        var target = BGSea.transform.GetChild(0).GetChild(val).transform;
        var pos = target.position;
        var obj = Instantiate(ClearEffectObj, pos, Quaternion.identity).gameObject.GetComponent<ParticleSystem>();
        var objmain = obj.main;
        var objchild = obj.transform.GetChild(0).GetComponent<ParticleSystem>();
        var objchildmain = objchild.main;
 
        switch (val)
        {
            case 0:
                objmain.startColor = objchildmain.startColor = colors[0];
                break;
            case 1:
                objmain.startColor = objchildmain.startColor = colors[1];
                break;
            case 2:
                objmain.startColor = objchildmain.startColor = colors[2];
                break;
            case 3:
                objmain.startColor = objchildmain.startColor = colors[3];
                break;
            case 4:
                objmain.startColor = objchildmain.startColor = colors[4];
                break;
            case 5:
                objmain.startColor = objchildmain.startColor = colors[5];
                break;
            case 6:
                objmain.startColor = objchildmain.startColor = colors[6];
                break;
        }
        var array = rainbows;
        rainbows = new ParticleSystem[array.Length + 1];
        System.Array.Copy(array, rainbows, array.Length);
        rainbows[array.Length] = objchild;
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

    public ParticleSystem.MinMaxGradient[] GetColor()
    {
        return colors;
    }
    public void MenuClose_ButtonControllOK()
    {
        m_bMenuOpen = false;
    }
}
