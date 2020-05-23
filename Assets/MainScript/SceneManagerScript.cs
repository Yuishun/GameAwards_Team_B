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
    [SerializeField, Header("デバッグ用クリアステージ数。不要になったら消去"),Range(0,7)]
    public int DebugStage = 0;
    static int[,] StageStatus = new int[7, 2] { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 } };
    public static bool ProphecyCheck = true;
    
    string prevSceneName;
    static bool OneLoad = true;
    [SerializeField]
    public Image Scenefader;
    
    [SerializeField]
    public Material m_mTransitionIn;
    [SerializeField]
    public Material m_mTransitionOut;
    [SerializeField]
    public bool m_bFadeOut = true;
    [SerializeField]
    public bool m_bFadeEnd = false;
    [SerializeField]
    public static bool m_bFadeInEnd = false;
    public static bool m_bMenu_InStage = false;
    
    [SerializeField, Header("メニュースクリプト")]
    MenuScript menuScript;
    
    [SerializeField, Header("セレクトメニュー")]
    public GameObject SelectMenu;
    [SerializeField, Header("ステージメニュー")]
    public GameObject StageMenu;
    
    bool NowLoading = false;
    
    void Awake()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Debug用
        for (int i = 0; i < StageStatus.Length / 2; i++) 
        {
            if (i < DebugStage)
                StageStatus[i, 1] = 1;
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        SceneManager.activeSceneChanged += SceneChanged;
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
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
        Debug.Log(prevSceneName + "->" + nextScene.name);

        StartCoroutine(BeginTransition());
        prevSceneName = nextScene.name;

        if (1 <= gamedata1 && gamedata1 <= 7)
            SceneManagerScript.m_bMenu_InStage = true;
        else
            SceneManagerScript.m_bMenu_InStage = false;
        MenuEnd();
    }
    //===================================================
    // 読み込みシーン選択＆フェード
    //===================================================
    public void OnPushChangeScene()
    {
        singleton.m_bFadeOut = true;
        StartCoroutine(BeginTransition());
        switch (gamedata1)
        {
            case 0://Titleからしか呼ばれない
                ProphecyCheck = true;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
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
            case 100:
                if (singleton)
                    singleton.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                StartCoroutine(SceneChange("GameClear"));
                break;
            case 500:
                StartCoroutine(SceneChange("StageSelect"));
                break;
            case 1000:
                if (singleton)
                    singleton.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
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
            singleton.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
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
        if (scene == "Start")
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
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
                yield return Animate(m_mTransitionOut, 1);
                yield return new WaitForEndOfFrame();
            }
            else
            {
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
        for (int i = 0; i < StageStatus.Length / 2; i++)
        {
            if (i == val)
                StageStatus[i, 1] = 1;
        }
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
}
