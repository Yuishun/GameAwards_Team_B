using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    //衝突タイマー
    int m_iCollisionTimer = 0;
    //衝突時間（3秒後ステージ遷移）
    int m_iLimitCollisionTime = 180;
    //衝突フラグ
    bool m_bCollisiontFlag = false;

    private void Update()
    {
        //衝突時間
        CollisionTime();
    }

    //=============================================================
    // 衝突間（物理衝突なし）
    //=============================================================
    void OnTriggerStay(Collider collider)
    {
        //光オブジェクトとの衝突
        if (collider.gameObject.name == "N_Light")
        {
            //色
            GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
            //衝突フラグ
            m_bCollisiontFlag = true;
            //ステージ遷移
            SceneTransition();
        }
    }

    //=============================================================
    // 衝突後（物理衝突なし）
    //=============================================================
    private void OnTriggerExit(Collider collider)
    {
        //光オブジェクトとの衝突後
        if (collider.gameObject.name == "N_Light")
        {
            //色
            GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
            //衝突フラグ
            m_bCollisiontFlag = false;
        }
    }

    //=============================================================
    // 衝突時間（3秒後ステージ遷移）
    //=============================================================
    private void CollisionTime()
    {
        if (m_bCollisiontFlag)
            m_iCollisionTimer++;
        else
            m_iCollisionTimer = 0;
    }

    //=============================================================
    // ステージ遷移
    //=============================================================
    private void SceneTransition()
    {
        if (m_iCollisionTimer > m_iLimitCollisionTime)
        {
            SceneManager.LoadScene("Nakahara02");
            m_iCollisionTimer = 0;
        }
    }
}
