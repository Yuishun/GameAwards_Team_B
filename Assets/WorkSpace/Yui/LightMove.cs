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

    [SerializeField] LayerMask FrameLayer, WaterLayer;
    [SerializeField] Surface surface;
    Color m_color;

    RaycastHit2D ray;

    // Start is called before the first frame update
    void Start()
    {
        m_pos = transform.position;
        Debug.Log(0 + " " + m_dirVec);
        //m_rb2d = GetComponent<Rigidbody2D>();

        //InvokeRepeating("Refravtion", 0, 2);
        
    }

    void FixedUpdate()
    {
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            //StartCoroutine(Refraction());
            Refraction();
        }*/
        StartCoroutine("Refraction");
    }


    IEnumerator Refraction()
    {
        // FixedUpdate終わりまで待つ
        yield return new WaitForFixedUpdate();

        m_lightpoint.LineReset();
        // 変数宣言
        //RaycastHit2D ray;
        m_dirVec = m_lightpoint.transform.right;    // 初期方向
        m_pos = m_lightpoint.transform.position;    // 初期位置
        m_color = Color.yellow;
        int i = 0;

        bool  EnterWater = false;   // 水に当たっているか
        while (true)
        {
            // エラー時の処理
            if(m_dirVec==new Vector2(0, 0))
            {
                Debug.Log("Vector Error");
                break;
            }

            // 水か光を通さないものに当たっているか
            ray = Physics2D.Raycast(m_pos, m_dirVec, 50,
                LayerMask.GetMask("Default","PostProcessing"), 0, 2);
            // 枠に当たったら位置をLineRendererに伝えて終了
            if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                transform.position = m_pos = ray.point;
                m_dirVec = new Vector2(ray.normal.y, ray.normal.x);
                AddLineRenderer();
                m_lightpoint.DrawLine();
                break;
            }
            else  if(!EnterWater && ray.collider.gameObject.layer ==
                LayerMask.NameToLayer("PostProcessing"))  // 水だったら
            {
                //ray.transform.GetComponent<>().
                //ray.transform.GetComponent<WaterSurface>().Rerurn_dirVec(m_pos, ray, m_dirVec);
                var watersurface = surface.ReInVector2(m_pos, ray, m_dirVec);
                // 空気から水への屈折したベクトルを取得
                m_dirVec = Refractioning(GetRefractiveIndex(RefractiveIndex.Air),
                    GetRefractiveIndex(RefractiveIndex.Water),
                    m_dirVec, ray.normal);
                
                m_pos = ray.point;

                AddLineRenderer();
                EnterWater = true;
            }

            // 水の中にいるとき
            if (EnterWater)
            {
                Collider2D[] collider2D = new Collider2D[1];
                while (true)
                {
                    // まだ水の中にいるか
                    int hitnum = Physics2D.OverlapCircleNonAlloc(m_pos, 0.3f, 
                        collider2D,WaterLayer);
                    if (hitnum == 0)    // 水から抜けた時
                    {
                        ray = Physics2D.Raycast(m_pos, -m_dirVec, 10,
                            WaterLayer, 0, 2);
                       // Debug.Log("Exit" + ray2.point);
                       // Debug.Log("ExitN" + ray2.normal);
                        m_pos = ray.point + m_dirVec * 0.001f; // 位置調整
                        m_dirVec = Refractioning(GetRefractiveIndex(RefractiveIndex.Water),
                                GetRefractiveIndex(RefractiveIndex.Air),
                                m_dirVec, -ray.normal);
                        m_color = Color.green;
                        AddLineRenderer();

                        EnterWater = false;
                        break;
                    }
                    // 水の中に不透過オブジェクトがあるかどうか
                    else if(0 < Physics2D.OverlapCircleNonAlloc(m_pos, 0.3f,
                        collider2D, FrameLayer))
                    {
                        ray = Physics2D.Raycast(m_pos, m_dirVec, 1,
                            FrameLayer, 0, 2);
                        m_pos = ray.point;
                        AddLineRenderer();
                        EnterWater = false;
                        break;
                    }

                    // 位置を方向に進める
                    m_pos += m_dirVec;

                }
            }
            i++;    // 無限ループ阻止
            if (i > 100)
            {
                Debug.Log("無限ループ脱出");
                break;
            }

        }

    }

    // LineRendererに屈折位置・終点を伝える
    void AddLineRenderer()
    {
        m_lightpoint.AddLineRenderer(m_pos, m_dirVec,m_color);
        //Debug.Log("Pos "+m_pos+"Vec "+ m_dirVec+"Color "+m_color);
        //Debug.Log("Normal" + ray.normal);
    }


    //! 以下から屈折に関するもの
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
            //return (v + 2 * C * n).normalized;
            nr = n1 / n2;
            C = -Vector2.Dot(v, n);
            g = Mathf.Sqrt(nr * nr + C * C - 1);

        }
        Vector2 T = 1 / nr * (v + (C - g) * n);
        return T.normalized;
    }
}
