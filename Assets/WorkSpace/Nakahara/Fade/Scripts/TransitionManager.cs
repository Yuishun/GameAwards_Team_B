using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField]
    private Material m_mTransitionIn;

    [SerializeField]
    private Material m_mTransitionOut;

    [SerializeField]
    bool m_bFadeOut = true;

    //=============================================================
    // コンストラクタ
    //=============================================================
    private void Start()
    {
        StartCoroutine(BeginTransition());
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
        GetComponent<Image>().material = material;
        float current = 0;
        while (current < time)
        {
            material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime / 2;
        }
        material.SetFloat("_Alpha", 1);
    }
}
