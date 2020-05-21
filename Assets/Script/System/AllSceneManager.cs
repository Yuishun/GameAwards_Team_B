using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AllSceneManager : MonoBehaviour
{
    static AllSceneManager singleton;
    //次に読み込むシーンを選択
    static int gamedata1 = 0;
    //ステージクリア数保存
    static int gamedata2 = 0;
    static int[,] StageStatus = new int[7,2] { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 } };
    public static bool ProphecyCheck = true;

    [SerializeField]
    private Image Scenefader;
    [SerializeField]
    private Material m_mTransitionIn;
    [SerializeField]
    private Material m_mTransitionOut;
    [SerializeField]
    bool m_bFadeOut = true;
    private bool m_bFadeEnd = false;
    [SerializeField]
    public static bool m_bFadeInEnd = false;

    void Awake()
    {
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
        m_bFadeInEnd = false;
        Debug.Log(prevScene.name + "->" + nextScene.name);
        m_bFadeOut = false;
        StartCoroutine(BeginTransition());
    }
    //===================================================
    // 読み込みシーン選択＆フェード
    //===================================================
    public void OnPushChangeScene()
    {
        m_bFadeOut = true;
        StartCoroutine(BeginTransition());
        switch (gamedata1)
        {
            case 0://Titleからしか呼ばれない
                gamedata1 = 1;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                StartCoroutine(SceneChange("Test_MainScene"));
                break;
            case 1:
                StartCoroutine(SceneChange("Test_Stage1"));
                break;
            case 2:
                StartCoroutine(SceneChange("Test_Stage2"));
                break;
            case 3:
                StartCoroutine(SceneChange("Test_Stage3"));
                break;
            case 4:
                StartCoroutine(SceneChange("Test_Stage4"));
                break;
            case 5:
                StartCoroutine(SceneChange("Test_Stage5"));
                break;
            case 6:
                StartCoroutine(SceneChange("Test_Stage6"));
                break;
            case 7:
                StartCoroutine(SceneChange("Test_Stage7"));
                break;
            case 100:
                StartCoroutine(SceneChange("Test_GameClear"));
                break;
            case 1000:
                StartCoroutine(SceneChange("Test_MainScene"));
                break;
        }
    }

    //=============================================================
    // メニューボタン用
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
        while (!m_bFadeEnd || async.progress < 0.9f)
            yield return new WaitForEndOfFrame();
        m_bFadeEnd = false;
        async.allowSceneActivation = true;
    }

    //=============================================================
    // トランジション
    //=============================================================
    IEnumerator BeginTransition()
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
    //=============================================================
    // time秒かけてトランジションを行う
    //=============================================================
    IEnumerator Animate(Material material, float time)
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
    //=============================================================
    // シーン遷移時呼び出し処理
    //=============================================================
    public void Loadstagenum(int type)
    {
        gamedata1 = type;
        OnPushChangeScene();
    }
    //=============================================================
    // 各ステージクリア状態
    //=============================================================
    public int[,] GetClearData()
    {
        return StageStatus;
    }
}
