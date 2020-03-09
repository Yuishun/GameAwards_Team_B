using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //カーソルスピード
    float m_fCursorSpeed = 0.5f;

    //衝突時の大きさ変動
    float m_fDeltaSize = 3.0f;
    //衝突中の色
    bool m_bSelecting = false;

    //=============================================================
    // 更新
    //=============================================================
    private void Update()
    {
        CheckUserInput();
    }

    //=============================================================
    // 入力
    //=============================================================
    private void CheckUserInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0f, m_fCursorSpeed, 0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0f, -m_fCursorSpeed, 0f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-m_fCursorSpeed, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(m_fCursorSpeed, 0f, 0f);
        }
    }

    //=============================================================
    // 衝突時（物理衝突なし）
    //=============================================================
    private void OnTriggerEnter(Collider collider)
    {
        //衝突時の大きさ
        Vector3 scale = collider.gameObject.transform.localScale;
        if (collider.gameObject.tag == "N_Obj")
        {
            collider.gameObject.transform.localScale = new Vector3(scale.x + m_fDeltaSize, scale.y + m_fDeltaSize, scale.z);
        }
    }

    //=============================================================
    // 衝突中（物理衝突なし）
    //=============================================================
    private void OnTriggerStay(Collider collider)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //色
            if (!m_bSelecting)
            {
                collider.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
                m_bSelecting = true;
            }
            else
            {
                collider.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f);
                m_bSelecting = false;
            }
        }
    }

    //=============================================================
    // 衝突後（物理衝突なし）
    //=============================================================
    private void OnTriggerExit(Collider collider)
    {
        //衝突時の大きさ
        Vector3 scale = collider.gameObject.transform.localScale;
        if (collider.gameObject.tag == "N_Obj")
        {
            collider.gameObject.transform.localScale = new Vector3(scale.x - m_fDeltaSize, scale.y - m_fDeltaSize, scale.z);
        }
    }
}
