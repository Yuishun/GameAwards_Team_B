using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    LineRenderer line;
    public Material mat;
    List<Vector2> LinePointers = new List<Vector2>();
    Vector3 StartPos;
    Vector2 m_dirVec;
    [SerializeField]
    private float maxDistance = 5;
    [SerializeField]
    LayerMask FrameLayer,ContainerLayer, WaterLayer;
    Vector3 Hitway;
    float WaterRange;
    public bool MoveStage = false;
    private bool WaterRangeFlag = true;
    private bool Con_In_WaterFlag = false;
    private bool Free_In_WaterFlag = false;
    public bool numliset = false;
    private string Layer_Default = "Default";
    private string Layer_Water = "PostProcessing";
    private string Layer_Mirror = "Mirror";
    private string Layer_Container = "Container";
    enum Substance
    {
        Default = 0,
        Container= 1,
        Water = 2,
        Mirror = 3
    }
    Substance substance = Substance.Default;
    enum RefractiveIndex
    {
        Air,
        Water,
        Mirror,
    }

    void Start()
    {
        mat = Resources.Load<Material>("line");
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.sortingOrder = 10;

        var colorkeys = new[]
        {
            new GradientColorKey(Color.red,0),
            new GradientColorKey(Color.magenta,1),
            //new GradientColorKey(Color.black,1),
        };
        var alphaleys = new[]
        {
            new GradientAlphaKey(0.8f, 0),
            new GradientAlphaKey(0.8f, 0),
            //new GradientAlphaKey(0.0f, 2),
        };
        var gradient = new Gradient();
        gradient.SetKeys(colorkeys, alphaleys);
        line.colorGradient = gradient;
        line.material = mat;
        line.numCornerVertices = 3;
        transform.position = new Vector3(-2f,-0.8f,0);
        transform.rotation = Quaternion.Euler(0,0,20);
        StartPos = transform.position;
        m_dirVec = transform.right;
    }
    //=============================================================
    //メイン
    //=============================================================
    int num = 0;
    void Update()
    {
        StartPos = transform.position;
        m_dirVec = transform.right;
        Con_In_WaterFlag = false;
        MoveStage = true;//仮置き
        Debug.DrawLine(StartPos,StartPos +(Vector3)m_dirVec*maxDistance);//初期のRay *5倍
        if (MoveStage)
        {
            LinePointers.Add(transform.position);
            do
            {
                RaycastHit2D hir = LightIrradiation();
                if (substance == Substance.Mirror)
                {
                    Debug.Log("鏡");
                    MirrorLine(hir);
                }
                if (substance == Substance.Water)
                {
                    Debug.Log("水");
                    WaterLine(hir);
                }
            } while (substance != Substance.Default && num++ < 50);
            Debug.Log("Sub"+substance);

            if (num >= 50)
            {
                Debug.Log("繰り返し設定オーバー");
                //UnityEditor.EditorApplication.isPaused = true;
            }
                LightTracer();
        }
        if (numliset)
        {
            numliset = !numliset;
            num = 0;
            substance = Substance.Default;
            Con_In_WaterFlag = false;
        }
    }
    bool Onece = true;
    //=============================================================
    //Ray
    //=============================================================
    private RaycastHit2D LightIrradiation()
    {   
        LinePointers.Add(StartPos);
        Ray2D ray = new Ray2D(StartPos + (Vector3)m_dirVec, m_dirVec);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,
            maxDistance, LayerMask.GetMask(Layer_Default,Layer_Water,Layer_Mirror));
        Debug.DrawLine(StartPos, StartPos + (Vector3)m_dirVec,Color.black);//初期のRay　*1倍
        if (hit)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layer_Water))
            {
                substance = Substance.Water;
                var foward = hit.transform.forward;
                hit.transform.LookAt(StartPos);
                Hitway = hit.transform.forward;
                if (WaterRangeFlag)//1度だけ実行
                {
                    WaterRangeFlag = !WaterRangeFlag;
                    WaterRange = hit.transform.lossyScale.x * hit.transform.GetComponent<CircleCollider2D>().radius;
                }
                hit.transform.forward = foward;
                StartPos = hit.point + (Vector2)Hitway * WaterRange;
                LinePointers.Add(StartPos);
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layer_Mirror))
            {
                substance = Substance.Mirror;
                LinePointers.Add(hit.point);
                //debug用++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                Debug.DrawLine(hit.point, hit.point + (Vector2)hit.transform.right, Color.blue);//Mirror.right
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(Layer_Default))
            {
                substance = Substance.Default;
                LinePointers.Add(hit.point);
                //debug用++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                //if(Onece)
                //hit.transform.GetComponent<SpriteRenderer>().color = Color.red;
                //
                //if(!Onece)
                //    hit.transform.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                Onece = false;
            }
        }
        else
        {
            StartPos += (Vector3)m_dirVec;
            LightIrradiation();
        }
        return hit;
    }
    //=============================================================
    //水面角度判定/水判定の時実行される
    //=============================================================
    void WaterLine(RaycastHit2D hit)//容器を通らず入水した場合の水中処理と、容器のない場合の出水処理が足りない
    {
        //容器を通って入水したかどうか
        if (!Con_In_WaterFlag)
        {
            var refpos = hit.point + (Vector2)Hitway * WaterRange;
            //もし、水の前に容器があった場合
            Ray2D ray = new Ray2D(hit.point, -m_dirVec);
            RaycastHit2D wall = Physics2D.Raycast(ray.origin, ray.direction,
                WaterRange * 2, LayerMask.GetMask(Layer_Container));

//            Debug.DrawLine(hit.point, hit.point + WaterRange * 2 * ray.direction,Color.green);//GreenRay
            //容器を通った場合
            if (wall)
            {
                if (wall.collider.gameObject.layer == LayerMask.NameToLayer(Layer_Container))//もしものとき用
                {
                    //debug用++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    wall.transform.GetComponent<SpriteRenderer>().color = Color.gray;
                    Con_In_WaterFlag = !Con_In_WaterFlag;
                    WaterSurface_calculation(wall.transform.right, true);
                    {
                        /*
                        var n1 = GetRefractiveIndex(RefractiveIndex.Air);
                        var n2 = GetRefractiveIndex(RefractiveIndex.Water);

                        var Nr = n1 / n2;
                        var W = m_dirVec;
                        var no = (Vector2)wall.transform.right;
                        //ベクトルの内積
                        var a = W.x;
                        var b = W.y;
                        var c = no.x;
                        var d = no.y;
                        var inner = a * c + b * d;
                        //全反射
                        if ((Nr * Nr - 1 + (inner * inner)) < 0)
                        {
                            Debug.Log("反射");
                            Debug.DrawLine(refpos, refpos + m_dirVec * 100, Color.yellow);
                            var R = W - 2 * inner * no;
                            m_dirVec = R;
                            return;
                        }
                        //屈折
                        else
                        {
                            Debug.Log("屈折");
                            var Wr = Nr * (W - inner * no) - Mathf.Sqrt(1 - Nr * Nr * (1 - inner * inner)) * no;
                            m_dirVec = Wr;
                            var point = hit.point + (Vector2)Hitway * WaterRange;
                            Debug.DrawLine(point, point + m_dirVec * 100, Color.yellow);
                        }
                        */
                    }
                    return;
                }
            }
            Con_In_WaterFlag = false;
            //容器を通ってない場合の入水面
            if (!Free_In_WaterFlag)//容器を通らず入水面Flag
                WaterSurfar_In(hit, m_dirVec);
            else
            {
                //出水面処理TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
                //WaterSurfar_Out(Ray ray, Vector2 dir);
                Free_In_WaterFlag = false;
            }
        }
        //容器を通って入水中の計算
        if (Con_In_WaterFlag)
        {
            //水中である間繰り返し
            do
            {
                Ray2D ray = new Ray2D(hit.point, -m_dirVec);
                RaycastHit2D WaterDrop = Physics2D.Raycast(ray.origin, ray.direction,
                    WaterRange * 2, LayerMask.GetMask(Layer_Water));
                if (WaterDrop)
                {
                    StartPos += (Vector3)m_dirVec * WaterRange;
                    LinePointers.Add(StartPos);
                    Debug.Log("水出し");
                    Debug.DrawLine(StartPos, StartPos + new Vector3(0, -1, 0), Color.cyan);
                    hit = WaterDrop;
                }
                //容器水から出たとき
                else
                {
                    //出水面処理TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
                    //WaterSurfar_Out(Ray ray, Vector2 dir);
                    //                    RaycastHit2D wall = Physics2D.Raycast(ray.origin, ray.direction,
                    //                    WaterRange * 2, LayerMask.GetMask(Layer_Container));
                    //                    if (wall)//容器の壁が出水面にあるとき
                    //                    {
                    //                        if (wall.collider.gameObject.layer == LayerMask.NameToLayer(Layer_Container))
                    //                            WaterSurface_calculation(wall.transform.right, false);
                    //                    }
                    //                    else//容器の壁が出水面にない時
                    //                    {
                    //
                    //                    }

                    Con_In_WaterFlag = false;
                }
            } while (Con_In_WaterFlag);
            {
                /*
            Debug.Log("水の中");
            Ray2D ray = new Ray2D(hit.point, -m_dirVec);
            RaycastHit2D WaterDrop = Physics2D.Raycast(ray.origin, ray.direction,
                WaterRange * 2, LayerMask.GetMask(Layer_Water));
            if (WaterDrop)
            {
                StartPos += (Vector3)m_dirVec * WaterRange;
                LinePointers.Add(StartPos);
                Debug.Log("水出し");
                Debug.DrawLine(StartPos, StartPos + new Vector3(0, -1, 0),Color.cyan);
                UnityEditor.EditorApplication.isPaused = true;
            }//要修正********************************************************************************************
            */
            }
        }
    }
    //=============================================================
    //鏡角度判定/鏡判定の時実行される
    //=============================================================
    void MirrorLine(RaycastHit2D hit)
    {
        StartPos = hit.point;
        //LinePointers.Add(StartPos);
        var W = m_dirVec;
        var no = (Vector2)hit.transform.right;
        //ベクトルの内積
        var a = W.x;
        var b = W.y;
        var c = no.x;
        var d = no.y;
        var inner = a * c + b * d;
        var R = W - 2 * inner * no;
        m_dirVec = R;
    }
    //=============================================================//StartPosを書き直すこと！
    void WaterSurfar_In(RaycastHit2D hit, Vector2 dir)
    {
        //カプセル中心点//カプセルの範囲Vector3(x,y)//カプセル丸みの着く方向()か０か。//角度
        Collider2D[] hit_Around = Physics2D.OverlapCapsuleAll
            (hit.point + m_dirVec*WaterRange * 0.2f,//中心
            new Vector2(hit.transform.lossyScale.x, hit.transform.lossyScale.y),//サイズ(x,y)
            CapsuleDirection2D.Vertical,//丸のつく方向
            transform.rotation.z,//角度
            LayerMask.GetMask(Layer_Water));//レイヤー

        bool Checker = true;
        if (hit_Around.Length == 1)
        {
            Checker = false;
            hit_Around = Physics2D.OverlapCapsuleAll
            (hit.point + m_dirVec * WaterRange * 0.2f,
            new Vector2(hit.transform.lossyScale.x*1.7f, hit.transform.lossyScale.y),
            CapsuleDirection2D.Horizontal,
            transform.rotation.z,
            LayerMask.GetMask(Layer_Water));
        }
        if (!Checker)
            Debug.Log("凸");
        else
            Debug.Log("凹");
        //水滴が1個以上の時
        if (hit_Around.Length > 1)
        {
            float min_Firsttarget_distance = float.MaxValue;
            float min_Secondtarget_distance = float.MaxValue;
            GameObject Trytarget1 = null;
            GameObject Trytarget2 = null;

            float Side1 = float.MinValue;
            float Side2 = float.MinValue;
            foreach (var Around_one in hit_Around)
            {
                //前回のが次高点の場合の一時置き場
                float onetime_distance = min_Firsttarget_distance;
                float onetime_Side = Side1;
                GameObject onetime_obj = Trytarget1;
                bool CheckFlag = false;
                //まずは普通に取得
                float target_distance = Vector2.Distance(transform.position, Around_one.transform.position);

                if (target_distance < min_Firsttarget_distance)
                {
                    min_Firsttarget_distance = target_distance;
                    Trytarget1 = Around_one.gameObject;
                    //左右どちらにあるか判別
                    Side1 = SideDiscrimination(transform.position, hit.transform.position, Trytarget1.transform.position);
                    CheckFlag = true;
                }
                //Firstで別のものが入ったときかつ、前回と方向が違う場合
                if (onetime_Side != Side1)
                {
                    if (onetime_distance < min_Secondtarget_distance)
                    {
                        min_Secondtarget_distance = onetime_distance;
                        Trytarget2 = onetime_obj;
                    }
                }
                else
                //判別したものとは違う方向の最高点を取得
                //Firstの前回と方向が同じのものが入った場合あるいは前回のFirstより長い場合
                if (target_distance < min_Secondtarget_distance)
                {
                    if (!CheckFlag)
                        //左右どちらにあるか判別
                        Side2 = SideDiscrimination(transform.position, hit.transform.position, Around_one.transform.position);
                    else
                        //チェック済みの時
                        Side2 = Side1;
                    if (Side1 != Side2)
                    {
                        min_Secondtarget_distance = target_distance;
                        Trytarget2 = Around_one.gameObject;
                    }
                }
                if (Trytarget2 == null)
                    Trytarget2 = hit.collider.gameObject;
            }
            //LinePointers.Add();
            Vector2 vec = (Trytarget1.transform.position - Trytarget2.transform.position).normalized;
            Debug.DrawLine(Trytarget1.transform.position, Trytarget2.transform.position, Color.blue);
            Trytarget1.transform.GetComponent<SpriteRenderer>().color = Color.red;
            Trytarget2.transform.GetComponent<SpriteRenderer>().color = Color.red;
            
            var vertical1 = new Vector2(vec.y, -vec.x);
            var vertical2 = new Vector2(-vec.y, vec.x);
            var right = vertical2 - vertical1;
            WaterSurface_calculation(right, true);
        }
        //水滴が1個だけの時
    }
    //=============================================================
    //出水面計算
    //=============================================================
    void WaterSurfar_Out(Ray ray, Vector2 dir)
    {
        //                    RaycastHit2D wall = Physics2D.Raycast(ray.origin, ray.direction,
        //                    WaterRange * 2, LayerMask.GetMask(Layer_Container));
        //                    if (wall)//容器の壁が出水面にあるとき
        //                    {
        //                        if (wall.collider.gameObject.layer == LayerMask.NameToLayer(Layer_Container))
        //                            WaterSurface_calculation(wall.transform.right, false);
        //                    }
        //                    else//容器の壁が出水面にない時
        //                    {
        //
        //                    }

    }
    //=============================================================
    //反射屈折計算 入水・出水(容器の中)
    //=============================================================
    void WaterSurface_calculation(Vector2 right, bool IntheWater)//vec.right
    {
        var n1 = 0.00f;
        var n2 = 0.00f;
        if (IntheWater)
        {
            n1 = GetRefractiveIndex(RefractiveIndex.Air);
            n2 = GetRefractiveIndex(RefractiveIndex.Water);
        }
        else
        {
            n1 = GetRefractiveIndex(RefractiveIndex.Water);
            n2 = GetRefractiveIndex(RefractiveIndex.Air);
        }
        var Nr = n1 / n2;
        var W = m_dirVec;
        var no = right;
        //ベクトルの内積
        var a = W.x;
        var b = W.y;
        var c = no.x;
        var d = no.y;
        var inner = a * c + b * d;
        //全反射
        if ((Nr * Nr - 1 + (inner * inner)) < 0)
        {
            Debug.Log("反射");
            var R = W - 2 * inner * no;
            m_dirVec = R;
            return;
        }
        //屈折
        else
        {
            Debug.Log("屈折");
            var Wr = Nr * (W - inner * no) - Mathf.Sqrt(1 - Nr * Nr * (1 - inner * inner)) * no;
            m_dirVec = Wr;
            return;
        }
    }
    //=============================================================
    //外積計算後・左右判定
    //=============================================================
    float SideDiscrimination(Vector2 vec1, Vector2 vec2, Vector2 target)
    {
        var origin_vec2_Unit_vector_X = float.MinValue;
        var origin_vec2_Unit_vector_Y = float.MinValue;
        var origin_target_Unit_vector_X = float.MinValue;
        var origin_target_Unit_vector_Y = float.MinValue;
        {
            double dx, dy;
            double x1, x2, y1, y2;
            x1 = vec1.x;
            y1 = vec1.y;
            x2 = vec2.x;
            y2 = vec2.x;

            dx = x2 - x1;
            dy = y2 - y1;

            double L = Mathf.Sqrt((float)((dx * dx) + (dy * dy)));

            origin_vec2_Unit_vector_X = (float)((dx / L) + x1);
            origin_vec2_Unit_vector_Y = (float)((dy / L) + y1);

            x2 = target.x;
            y2 = target.x;

            dx = x2 - x1;
            dy = y2 - y1;

            L = Mathf.Sqrt((float)((dx * dx) + (dy * dy)));

            origin_target_Unit_vector_X = (float)((dx / L) + x1);
            origin_target_Unit_vector_Y = (float)((dy / L) + y1);
        }

        float S = (origin_vec2_Unit_vector_X * origin_target_Unit_vector_Y) - (origin_vec2_Unit_vector_Y * origin_target_Unit_vector_X);
        return S;
    }
    //=============================================================
    //登録された地点を辿り、線を引く
    //=============================================================
    void LightTracer()
    {
        float linedistance = 0;
        Vector2 pastpoint = transform.position;
        foreach (Vector2 point in LinePointers)
        {
            linedistance += Vector2.Distance(pastpoint, point);
            pastpoint = point;
        }
        //Debug.Log(linedistance);//2.20
        //長さに対して水の位置を判定。水中判定のときのみ、Lineの色を変化させる予定

        int linecount = 0;
        foreach (Vector2 point in LinePointers)
        {
            linecount++;
            line.positionCount = linecount;
            line.SetPosition(linecount - 1, point);
        }
     //   Debug.Log(linecount);
        //リストの全削除
        LinePointers.Clear();
    }
    //=============================================================
    //屈折率置き場
    //=============================================================
    float GetRefractiveIndex(RefractiveIndex index)
    {
        switch (index)
        {
            case RefractiveIndex.Air:
                return 1f;

            case RefractiveIndex.Water:
                return 1.333f;
            case RefractiveIndex.Mirror:
                return 2f;

            default:
                return -1f;
        }
    }
}
