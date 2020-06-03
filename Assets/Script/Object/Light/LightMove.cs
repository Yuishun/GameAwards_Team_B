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
    public Color m_color;

    RaycastHit2D ray;

    // Start is called before the first frame update
    void Start()
    {
        m_pos = transform.position;
        //Debug.Log(0 + " " + m_dirVec);
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
        m_color = m_lightpoint.vertColor;
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
            i++;    // 無限ループ阻止
            if (i > 100)
            {
                Debug.Log("無限ループ脱出");
                break;
            }

            // 水か光を通さないものに当たっているか
            ray = Physics2D.Raycast(m_pos, m_dirVec, 50,
                LayerMask.GetMask("Default","PostProcessing"), 0, 2);
            // 枠に当たったら位置をLineRendererに伝えて終了
            if (ray.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                if(FrameLayerProcessing(ray.transform.tag, false))
                    break;
                continue;
            }
            else  if(!EnterWater && ray.collider.gameObject.layer ==
                LayerMask.NameToLayer("PostProcessing"))  // 水だったら
            {
                // 水の粒が規定値よりあるか
                if (!ray.transform.GetComponent<MetaballParticleClass>().
                     CountWater())
                {
                    m_pos = ray.point + m_dirVec * Vector2.Distance(ray.point, ray.transform.position) * 2;
                    continue;
                }
                //ray.transform.GetComponent<>().
                //ray = ray.transform.GetComponent<MetaballParticleClass>().WaterNormalVec(ray);
                var watersurface = surface.ReInVector2(m_pos, ray, m_dirVec);
                // 空気から水への屈折したベクトルを取得
                m_dirVec = Refractioning(GetRefractiveIndex(RefractiveIndex.Air),
                    GetRefractiveIndex(RefractiveIndex.Water),
                    m_dirVec, ray.normal);
                
                m_pos = ray.point;

                AddLineRenderer();
                m_color = ray.transform.GetComponent<MetaballParticleClass>().
                    spRend.color;
                EnterWater = true;
            }

            // 水の中にいるとき
            if (EnterWater)
            {
                Collider2D[] collider2D = new Collider2D[1];
                Vector2 raypos = m_pos;
                int k = 0;
                while (EnterWater)
                {
                    if (++k > 100)
                        break;
                    // まだ水の中にいるか
                    int hitnum = Physics2D.OverlapCircleNonAlloc(m_pos, 0.2f, 
                        collider2D,WaterLayer);
                    if (hitnum == 0)    // 水から抜けた時
                    {
                        ray = Physics2D.Raycast(m_pos, -m_dirVec, 5,
                            WaterLayer, 0, 2);
                       // m_color = ray.transform.GetComponent<MetaballParticleClass>().
                       //     spRend.color;
                        // Debug.Log("Exit" + ray2.point);
                        //Debug.Log("ExitN" + ray.normal);
                        m_pos = ray.point + m_dirVec * 0.001f; // 位置調整
                        ray = ray.transform.GetComponent<MetaballParticleClass>().WaterNormalVec(ray);                        
                        //var watersurface = surface.ReInVector2(m_pos, ray, -m_dirVec);
                        m_dirVec = Refractioning(GetRefractiveIndex(RefractiveIndex.Water),
                                GetRefractiveIndex(RefractiveIndex.Air),
                                m_dirVec, -ray.normal);

                        AddLineRenderer();

                        EnterWater = false;                       
                    }
                    // 水の中に不透過オブジェクトがあるかどうか
                    else if (ray = Physics2D.Raycast(raypos,
                        m_dirVec, 0.3f, FrameLayer, 0, 2))
                    {                        
                        
                        //if(ray.collider)
                        EnterWater =
                            FrameLayerProcessing(ray.transform.tag, EnterWater);
                        if(!EnterWater)
                            yield break;
                    }
                    else
                    {
                        raypos = m_pos;
                        // 基準位置を方向に進める
                        m_pos += m_dirVec * 0.3f;
                    }
                }
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
    
    bool FrameLayerProcessing(string tag,bool flag)
    {
        switch (tag)
        {
            case "Frame":
            case "Flower":
                transform.position = m_pos = ray.point;
                m_dirVec = new Vector2(ray.normal.y, -ray.normal.x);
                AddLineRenderer();
                m_lightpoint.DrawLine();
                return !flag;                
            case "Mirror":
                m_pos = ray.point;
                float C = -Vector2.Dot(m_dirVec, ray.normal);
                m_dirVec = (m_dirVec + 2 * C * ray.normal).normalized;                
                AddLineRenderer();
                m_pos += m_dirVec * 0.01f;
                return flag;
            case "ColorWall":
                Color wallcolor = ray.transform.GetComponent<SpriteRenderer>().color;
                if (m_color == wallcolor)
                {
                    m_pos += m_dirVec * 0.3f;
                    return flag;
                }
                else
                {
                    m_pos = ray.point;
                    m_dirVec = new Vector2(ray.normal.y, -ray.normal.x);
                    AddLineRenderer();
                    m_lightpoint.DrawLine();
                    return !flag;
                }
                break;
        }
        Debug.Log("FramereturnError");
        return true;
    }
}
