using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1)]
public class SceneManagerScript : MonoBehaviour
{
    static SceneManagerScript singleton;
    //次に読み込むシーンを選択
    static int gamedata1 = 0;
    //ステージクリア数保存
    static int gamedata2 = 0;
    static int[,] StageStatus = new int[7, 2] { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 } };
    public static bool ProphecyCheck = true;

    string prevSceneName;
    static bool OneLoad = true;
    [SerializeField]
    public Image Scenefader;
    [SerializeField]
    public Sprite ClearScenefader;
    [SerializeField]
    public Material m_mTransitionIn;
    [SerializeField]
    public Material m_mTransitionOut;
    [SerializeField]
    public Material m_mTransition_ClearIn;
    [SerializeField]
    public Material m_mTransition_ClearOut;
    [SerializeField]
    public bool m_bFadeOut = true;
    [SerializeField]
    public bool m_bFadeEnd = false;

    Color default_fadeColor = new Color32(112, 180, 255, 255);
    public static bool m_bFadeInEnd = false;
    public static bool m_bMenu_InStage = false;
    bool m_bClearFade = false;

    [SerializeField, Header("メニュースクリプト")]
    MenuScript menuScript;

    [SerializeField, Header("セレクトメニュー")]
    public GameObject SelectMenu;
    [SerializeField, Header("ステージメニュー")]
    public GameObject StageMenu;

    bool NowLoading = false;
    static float audioBGMVolume = 0;
    static float audioSEVolume = 0;
    [SerializeField,Header("ステージUI隠すまでの時間"),Range(1,10)]
    float hideUItime = 5f;
    [SerializeField]
    BGMAudioscript audioScript;
    void Awake()
    {
        SceneManager.activeSceneChanged += SceneChanged;
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;

            audioBGMVolume = PlayerPrefs.GetFloat("BGM", 1f);
            audioSEVolume = PlayerPrefs.GetFloat("SE", 1f);
            audioScript.SetVolume(audioBGMVolume);
        }
        else
            Destroy(gameObject);
    }
    //=============================================================
    // シーン遷移後処理
    //=============================================================
    void SceneChanged(Scene prevScene, Scene nextScene)
    {
        SceneManagerScript.m_bFadeInEnd = false;
        NowLoading = false;
        singleton.m_bFadeOut = false;
        //Debug.Log(prevSceneName + "->" + nextScene.name);

        for (int i = 0; i < StageClearLoad(); i++)
            StageStatus[i, 1] = 1;

        if (gamedata1 == 100)
            ClearSceneFadeOut();
        StartCoroutine(BeginTransition());
        prevSceneName = nextScene.name;
        if (1 <= gamedata1 && gamedata1 <= 7)
            SceneManagerScript.m_bMenu_InStage = true;
        else
            SceneManagerScript.m_bMenu_InStage = false;
        MenuEnd();
        audioScript.SetBGM(singleton, nextScene.name);
    }
    //===================================================
    // 読み込みシーン選択＆フェード
    //===================================================
    public void OnPushChangeScene()
    {
        if (gamedata1 == 100)
            ClearFadeSet();
        singleton.m_bFadeOut = true;
        StartCoroutine(BeginTransition());
        switch (gamedata1)
        {
            case 0://Titleからしか呼ばれない
                ProphecyCheck = true;
                StartCoroutine(SceneChange("StageSelect"));
                break;
            case 1:
                StartCoroutine(SceneChange("Stage1_Red"));
                break;
            case 2:
                StartCoroutine(SceneChange("Stage2_Orange"));
                break;
            case 3:
                StartCoroutine(SceneChange("Stage3_Yellow"));
                break;
            case 4:
                StartCoroutine(SceneChange("Stage4_Green"));
                break;
            case 5:
                StartCoroutine(SceneChange("Stage5_Blue"));
                break;
            case 6:
                StartCoroutine(SceneChange("Stage6_Indigo"));
                break;
            case 7:
                StartCoroutine(SceneChange("Stage7_Purple"));
                break;
            case 600:
                if (singleton)
                    singleton.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                StartCoroutine(SceneChange("Config"));
                break;
            case 100:
                if (singleton)
                    singleton.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                for (int i = 0; i < StageStatus.Length / 2; i++)
                        StageStatus[i, 1] = 0;
                StartCoroutine(SceneChange("GameClear"));
                break;
            case 500:
                StartCoroutine(SceneChange("StageSelect"));
                break;
            case 1000:
                if (singleton)
                    singleton.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                StartCoroutine(SceneChange("Start"));
                break;
        }
    }

    //=============================================================
    // メニューボタン用(ステージセレクト画面用)
    //=============================================================
    public static void SetButton_Active()
    {
        if (singleton) 
            singleton.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    //=============================================================
    // 非同期読み込み(フェード終了かつ読み込み後に遷移)
    //=============================================================
    IEnumerator SceneChange(string scene)
    {
        var async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        while (!singleton.m_bFadeEnd || async.progress < 0.9f)
            yield return new WaitForEndOfFrame();
        singleton.m_bFadeEnd = false;
        async.allowSceneActivation = true;
    }

    //=============================================================
    // トランジション
    //=============================================================
    IEnumerator BeginTransition()
    {
        if (gameObject != null)
        {
            if (m_bFadeOut)
            {
                if (m_bClearFade)
                {
                    yield return Animate(m_mTransition_ClearOut, 1);
                }
                else
                    yield return Animate(m_mTransitionOut, 1);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                if (m_bClearFade)
                    yield return Animate(m_mTransition_ClearIn, 1);
                else
                    yield return Animate(m_mTransitionIn, 1);
            }
        }
    }
    //=============================================================
    // time秒かけてトランジションを行う
    //=============================================================
    IEnumerator Animate(Material material, float time)
    {
        if (gameObject != null)
        {
            Scenefader.material = material;
            float current = 0;
            while (current < time)
            {
                material.SetFloat("_Alpha", current / time);
                yield return new WaitForEndOfFrame();
                current += Time.deltaTime / 2;
            }
            material.SetFloat("_Alpha", 1);
            if (m_bFadeOut)
            {
                m_bFadeEnd = true;
            }
            else
                m_bFadeInEnd = true;
        }
    }
    //=============================================================
    // シーン遷移時呼び出し処理
    //=============================================================
    public void Loadstagenum(int type)
    {
        if (!NowLoading)
        {
            NowLoading = !NowLoading;
            gamedata1 = type;
            OnPushChangeScene();
        }
    }
    //=============================================================
    // 各ステージクリア状態
    //=============================================================
    public int[,] GetClearData()
    {
        return StageStatus;
    }
    //=============================================================
    // 各ステージクリアしたとき
    //=============================================================
    public void OneStageClear(int val)
    {
        int count = 0;
        for (int i = 0; i < StageStatus.Length / 2; i++)
        {
            if (i + 1 == val)
            {
                count++;
                StageStatus[i, 1] = 1;
            }
        }
        StageClearSave(count);
        Loadstagenum(500);
    }
    //=============================================================
    // 各ステージクリア状態初期化
    //=============================================================
    public void DeleteClearData()
    {
        for (int i = 0; i < StageStatus.Length / 2; i++)
            StageStatus[i, 1] = 0;
        StageClearSave(0);
    }
    //=============================================================
    // Menu
    //=============================================================
    public void Menu()
    {
        StartCoroutine(MenuActivetion());
    }
    IEnumerator MenuActivetion()
    {
        if (SceneManagerScript.m_bFadeInEnd)
            yield return new WaitForEndOfFrame();
        if (SceneManagerScript.m_bMenu_InStage)
        {
            singleton.StageMenu.gameObject.SetActive(true);
            singleton.SelectMenu.gameObject.SetActive(false);
        }
        else
        {
            singleton.SelectMenu.gameObject.SetActive(true);
            singleton.StageMenu.gameObject.SetActive(false);
        }
        menuScript.enabled = true;
    }
    public void MenuEnd()
    {
        menuScript.enabled = false;
        singleton.StageMenu.gameObject.SetActive(false);
        singleton.SelectMenu.gameObject.SetActive(false);
    }
    //=============================================================
    // ステージ再読み込み
    //=============================================================
    public void ReStage()
    {
        Loadstagenum(gamedata1);
    }

    //=============================================================
    // ステージクリアフェードセット
    //=============================================================
    void ClearFadeSet()
    {
        m_bClearFade = true;
        m_mTransition_ClearOut.color = Color.white;
        Scenefader.GetComponent<Image>().sprite = ClearScenefader;
    }
    void ClearSceneFadeOut()
    {
        Vector3 scale = transform.localScale;
        scale.y = -1;
        Scenefader.transform.localScale = scale;
        Invoke("LateClearSceneFade", 1);
    }
    void LateClearSceneFade()
    {
        m_mTransition_ClearOut.color = default_fadeColor;
        m_bClearFade = false;
    }

    //===========================
    // 音量 BGM・SE
    //===========================
    public float GetBGMVolume()
    {        
        return audioBGMVolume;
    }
    public void SetBGMVolume(float val)
    {
        audioBGMVolume = val;
        PlayerPrefs.SetFloat("BGM", audioBGMVolume);
    }
    public float GetSEVolume()
    {
        return audioSEVolume;
    }
    public void SetSEVolume(float val)
    {
        audioSEVolume = val;
        PlayerPrefs.SetFloat("SE", audioSEVolume);
    }
    //===========================
    // UI隠し時間返し
    //===========================
    public float GethideUItime()
    {
        return hideUItime;
    }
    //===========================
    //クリア状態 セーブ・ロード
    //===========================
    void StageClearSave(int num)
    {
        PlayerPrefs.SetInt("ClearNum",num);
    }
    int StageClearLoad()
    {
        var add = PlayerPrefs.GetInt("ClearNum", 0);
        if (add > 7) add = 7;
        return add;
    }
}
