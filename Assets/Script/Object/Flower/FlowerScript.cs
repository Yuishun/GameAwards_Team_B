﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    [SerializeField]
    Sprite seed;
    [SerializeField]
    Sprite sprout;
    [SerializeField]
    Sprite bloom;
    [SerializeField]
    Sprite flower;
    [SerializeField]
    Material material;
    SpriteRenderer SpriteRenderer;
    float Alpha = 1.0f;
    float swingtime = 0;
    bool swingway = true;

    //衝突タイマー
    int m_iCollisionTimer = 0;
    //衝突時間（2秒後ステージ遷移）
    int m_iLimitCollisionTime = 150;
    //衝突フラグ
    bool m_bCollisiontFlag = false;
    Collider2D[] col = new Collider2D[1];
    [SerializeField]
    Color ClearColor = Color.white;
    [SerializeField]
    int stagenum;

    void Start()
    {
        SpriteRenderer = transform.GetComponent<SpriteRenderer>();
        SpriteRenderer.material = material;
        SpriteRenderer.sprite = seed;
        StartCoroutine("Swing");
    }
    //他スクリプトからここを呼び出すことでクリア花開花画像切り替えが始まる
    public void GoalFlower()
    {
        StopCoroutine("Swing");
        StartCoroutine("Blooming");
    }

    public void SkipFlower()
    {
        StopCoroutine("Blooming");
    }

    IEnumerator Blooming()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        //FadeOut
        for (int i = 0; i < 8*2; i++)
        {
            Alpha -= 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return new WaitForSeconds(0.05f);
        }
        //画像切り替え：芽
        SpriteRenderer.sprite = sprout;
        //FadeIn
        for (int i = 0; i < 8*2; i++)
        {
            Alpha += 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        //FadeOut
        for (int i = 0; i < 8*2; i++)
        {
            Alpha -= 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return new WaitForSeconds(0.05f);
        }
        //画像切り替え：蕾
        SpriteRenderer.sprite = bloom;
        //FadeIn
        for (int i = 0; i < 8*2; i++)
        {
            Alpha += 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        //FadeOut
        for (int i = 0; i < 8*2; i++)
        {
            Alpha -= 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return new WaitForSeconds(0.05f);
        }
        //画像切り替え：花
        SpriteRenderer.sprite = flower;
        //FadeIn
        for (int i = 0; i < 8*2; i++)
        {
            Alpha += 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return null;
        }
        material.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("AllSceneManager").GetComponent<SceneManagerScript>().
            OneStageClear(stagenum);
    }
    IEnumerator Swing()
    {
        while (true)
        {
            var angle = 0f;
            swingtime += Time.deltaTime;
            if (swingtime >= 1)
            {
                swingtime = 0;
                swingway = !swingway;
                transform.up = transform.root.up;
            }
            if (swingway)
                angle = Mathf.LerpAngle(-10, 10, swingtime);
            else
                angle = Mathf.LerpAngle(10, -10, swingtime);
            transform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
    }

    private void Update()
    {
        col[0] = null;
            Physics2D.OverlapCircleNonAlloc(transform.position, 0.6f,
            col, LayerMask.GetMask("Light"));
        if (col[0])
        {
            if (col[0].GetComponent<LightMove>().m_color
                != ClearColor)
                return;

            m_iCollisionTimer++;
            if (m_iCollisionTimer > m_iLimitCollisionTime)
            {
                GoalFlower();
            }
        }
        else if (m_iCollisionTimer > 0)
        {
            m_iCollisionTimer = 0;
        }
    }
}
