using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//[DefaultExecutionOrder(-2)]
public class CoroutineScript : MonoBehaviour
{
    SceneManagerScript SceneManagerScript;
    private Image Scenefader;
    [SerializeField]
    private Material m_mTransitionIn;
    [SerializeField]
    private Material m_mTransitionOut;
    static bool onece = true;
    void Awake()
    {
        if (onece)
        {
            onece = !onece;
            DontDestroyOnLoad(gameObject);
            SceneManagerScript = GameObject.FindGameObjectWithTag("AllScene").GetComponent<SceneManagerScript>();
            Scenefader = SceneManagerScript.Scenefader;
            m_mTransitionIn = SceneManagerScript.m_mTransitionIn;
            m_mTransitionOut = SceneManagerScript.m_mTransitionOut;
        }
    }
    //=============================================================
    // トランジション
    //=============================================================
    public IEnumerator BeginTransition()
    {
        if (gameObject != null)
        {
            if (SceneManagerScript.m_bFadeOut)
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
    public IEnumerator Animate(Material material, float time)
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
            if (SceneManagerScript.m_bFadeOut)
            {
                SceneManagerScript.m_bFadeEnd = true;
            }
            else
                SceneManagerScript.m_bFadeInEnd = true;
        }
    }

}

/*
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
    
    void Start()
    {

    }
     */
