using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    enum RefractiveIndex
    {
        Air,
        Water,

    }

    Vector2 m_pos;
    Vector2 m_dirVec;
    public Vector2 dirVec
    {
        set { m_dirVec = value; }
    }
    //Rigidbody2D m_rb2d;

    LightStartPoint m_lightpoint;
    public LightStartPoint lightpoint
    {
        set { m_lightpoint = value; }
    }
    int m_linenum = 1;

    [SerializeField] LayerMask FrameLayer, WaterLayer;

    // Start is called before the first frame update
    void Start()
    {
        m_pos = transform.position;
        Debug.Log(0 + " " + m_dirVec);
        //m_rb2d = GetComponent<Rigidbody2D>();

        //InvokeRepeating("Refravtion", 0, 2);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Refraction();
        }
    }


    void Refraction()
    {
        RaycastHit2D ray;
        m_linenum = 1;
        m_dirVec = m_lightpoint.transform.right;
        m_pos = m_lightpoint.transform.position;
        int i = 0;

        bool  EnterWater = false;
        while (true)
        {
            if(m_dirVec==new Vector2(0, 0))
            {
                Debug.Log("Vector Error");
                break;
            }

            ray = Physics2D.Raycast(m_pos, m_dirVec, 50,
                LayerMask.GetMask("Default","PostProcessing"), 0, 2);
            // 枠に当たったら終了
            if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                transform.position = m_pos = ray.point;
                AddLineRenderer();
                break;
            }
            else  if(!EnterWater && ray.collider.gameObject.layer ==
                LayerMask.NameToLayer("PostProcessing"))  // 水だったら
            {
                //ray.transform.GetComponent<>().

                m_dirVec = Refractioning(GetRefractiveIndex(RefractiveIndex.Air),
                    GetRefractiveIndex(RefractiveIndex.Water),
                    m_dirVec, ray.normal);
                
                m_pos = ray.point;
                AddLineRenderer();
                EnterWater = true;
            }

            if (EnterWater)
            {
                Collider2D[] collider2D = new Collider2D[1];
                while (true)
                {
                    // まだ水の中にいるか
                    int hitnum = Physics2D.OverlapCircleNonAlloc(m_pos, 0.3f, 
                        collider2D,WaterLayer);
                    if (hitnum == 0)    // 
                    {
                        RaycastHit2D ray2 = Physics2D.Raycast(m_pos, -m_dirVec, 10,
                            WaterLayer, 0, 2);
                       // Debug.Log("Exit" + ray2.point);
                       // Debug.Log("ExitN" + ray2.normal);
                        m_pos = ray2.point + m_dirVec * 0.001f;
                        m_dirVec = Refractioning(GetRefractiveIndex(RefractiveIndex.Water),
                                GetRefractiveIndex(RefractiveIndex.Air),
                                m_dirVec, -ray2.normal);
                        AddLineRenderer();

                        EnterWater = false;
                        break;
                    }
                    // 先に不透過オブジェクトがあるかどうか
                    else if(0 < Physics2D.OverlapCircleNonAlloc(m_pos, 0.3f,
                        collider2D, FrameLayer))
                    {
                        RaycastHit2D ray2 = Physics2D.Raycast(m_pos, m_dirVec, 1,
                            FrameLayer, 0, 2);
                        m_pos = ray2.point;
                        AddLineRenderer();
                        EnterWater = false;
                        break;
                    }
                    m_pos += m_dirVec;

                }
            }
            i++;    // 無限ループ阻止
            if (i > 100)
            {
                Debug.Log("無限ループ脱出");
                return;
            }

        }

    }

    void AddLineRenderer()
    {
        m_lightpoint.line.positionCount = ++m_linenum;
        m_lightpoint.line.SetPosition(m_linenum - 1, m_pos);
        Debug.Log(m_linenum-1+" "+ m_dirVec);
    }

    float GetRefractiveIndex(RefractiveIndex index)
    {
        switch (index)
        {
            case RefractiveIndex.Air:
                return 1f;

            case RefractiveIndex.Water:
                return 1.333f;

            default:
                return -1f;
        }
      
    }

    Vector2 Refractioning(float n1,float n2,Vector2 v,Vector2 n)
    {
        if (n1 == -1 || n2 == -1)
        {
            return new Vector2(0, 0);
        }
       /* if (0 > Vector2.Dot(v, n))
        {
            n = -n;
        }*/

          float nr = n2 / n1;
        
       /*   float cos1 = Vector2.Dot(v, n);
          float cos2 = n1 / n2 * Mathf.Sqrt(nr * nr - (1 - cos1 * cos1));
          float omega = nr * cos2 - cos1;

          Vector2 f = n1 / n2 * (v - omega * n);
          return f.normalized;*/
          

        float C = -Vector2.Dot(v, n);
        float g = Mathf.Sqrt(nr * nr + C * C - 1);
        if (float.IsNaN(g))
        {
            //return v + 2 * C * n .normalized;
            nr = n1 / n2;
            C = -Vector2.Dot(v, n);
            g = Mathf.Sqrt(nr * nr + C * C - 1);

        }
        Vector2 T = 1 / nr * (v + (C - g) * n);
        return T.normalized;
    }
}
