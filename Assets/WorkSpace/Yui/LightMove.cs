using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    Vector2 m_pos;
    Vector2 m_dirVec;
    public Vector2 dirVec
    {
        set { m_dirVec = value; }
    }
    Rigidbody2D m_rb2d;

    LightStartPoint m_lightpoint;
    public LightStartPoint lightpoint
    {
        set { m_lightpoint = value; }
    }
    int m_linenum = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_pos = transform.position;
        m_rb2d = GetComponent<Rigidbody2D>();


        //InvokeRepeating("Refravtion", 0, 2);
        Refraction();
    }


    void Refraction()
    {
        RaycastHit2D ray;
        m_linenum = 1;

        bool  EnterWater = false;
        while (true)
        {
            ray = Physics2D.Raycast(m_pos, m_dirVec, 50,
                LayerMask.GetMask("Default","PostProcessing"), 0, 2);
            // 枠に当たったら終了
            if (ray.collider.gameObject.layer == ~LayerMask.NameToLayer("Default"))
            {
                m_pos = ray.point;
                AddLineRenderer();
                break;
            }
            else  if(!EnterWater && 
                ray.collider.gameObject.layer == LayerMask.NameToLayer("PostProcessing"))  // 水だったら
            {
                //ray.transform.GetComponent<>().

                m_pos = ray.point;
                AddLineRenderer();
                EnterWater = true;
            }

            if (EnterWater)
            {
                while (true)
                {
                    Collider2D[] collider2D 
                        = Physics2D.OverlapCircleAll(m_pos, 0.3f, LayerMask.GetMask("PostProcessing"));
                    if (collider2D == null)
                    {
                        m_pos -= m_dirVec * 0.3f;
                        AddLineRenderer();
                        EnterWater = false;
                        break;
                    }
                    m_pos += m_dirVec;
                }
            }
        }

        void AddLineRenderer()
        {
            m_lightpoint.line.positionCount = ++m_linenum;
            m_lightpoint.line.SetPosition(m_linenum - 1, m_pos);
        }
    }
}
