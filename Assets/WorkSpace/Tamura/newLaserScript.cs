using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newLaserScript : MonoBehaviour
{

    LineRenderer line;
    Vector2 Point1, Point2;
    List<Vector2> LinePointers = new List<Vector2>();

    public Material mat;
    private float timer = 0;
    Vector2 hitWay;
    Vector3 hitrange;
    bool Container_IN = false;
    WaterSurface waterSurface;
    Vector2 m_dirVec;
    void Start()
    {
        mat = Resources.Load<Material>("line");
        line = this.gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.widthMultiplier = 1;
        line.endWidth = 0.1f;

        line.startColor = Color.blue;
        line.endColor = Color.cyan;
        line.material = mat;
        line.numCornerVertices = 3;

        waterSurface = gameObject.AddComponent<WaterSurface>();
        m_dirVec = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        Ray2D ray = new Ray2D(transform.position, m_dirVec);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin,ray.direction, 10, LayerMask.GetMask("Default", "PostProcessing"), 0, 2);
        LinePointers.Add(transform.position);
        WaterSurface(RayIrradiation());
        //線を引いている
        LineTracer();
        
    }

    //==========================================================================
    //Rayを飛ばしてオブジェクト判定
    //==========================================================================
    RaycastHit2D RayIrradiation()
    {
        int maxDistance = 10;
        Ray2D ray;

        //Rayを物体から見て下に照射
        ray = new Ray2D(transform.position, -transform.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, maxDistance, LayerMask.GetMask("Default", "PostProcessing"));
        if (hit)
        {
            hitWay = new Vector2(0, 0);
            //Rayの物体までの描画用
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PostProcessing"))
            {
                var upway = hit.transform.forward;
                hit.collider.transform.LookAt(transform.position);
                hitWay = hit.transform.forward * hit.collider.transform.lossyScale.x * 0.3f;
                hit.transform.forward = upway;
                LinePointers.Add(hit.point + hitWay);
                hitrange = hitWay + hitWay * Vector3.Distance(hit.point, hit.transform.position);
                float distance = Vector2.Distance(transform.position,hit.transform.position);
                RaycastHit2D container_box = Physics2D.Raycast(ray.origin, ray.direction, distance, LayerMask.GetMask("Container"));
                if(container_box)
                if (container_box.collider.gameObject.layer == LayerMask.NameToLayer("Container"))
                {
                    Container_IN = true;

                    Vector2.Distance(transform.position, container_box.transform.position);
                    Vector2.Distance(transform.position, hit.transform.position);
                }
            }
            LinePointers.Add(hit.point + hitWay);
        }
        return hit;
    }
    //==========================================================================
    //水面処理2D
    //==========================================================================
    public bool onetimecreate = false;
    void WaterSurface(RaycastHit2D hit)
    {
        if (hit) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PostProcessing"))
            {
                //カプセル中心点//カプセルの範囲Vector3(x,y)//カプセル丸みの着く方向()か０か。//角度
                Collider2D[] hit_Around = Physics2D.OverlapCapsuleAll
                    (hit.point + (Vector2)hitWay * 0.2f,
                    new Vector2(hit.transform.lossyScale.x , hit.transform.lossyScale.y),
                    CapsuleDirection2D.Vertical,
                    transform.rotation.z,
                    LayerMask.GetMask("PostProcessing"));


                bool Checker = true;
                if (onetimecreate && hit_Around.Length != 1)
                {
                    GameObject Sphere = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    Sphere.transform.position = hit.point + (Vector2)hitWay * 0.2f;
                    Sphere.transform.position += new Vector3(0, 0, 10);
                    Sphere.transform.localScale = hit.transform.lossyScale * 0.8f;
                    Sphere.transform.LookAt(transform.position, Vector3.forward);
                    Sphere.name = "凹";
                    onetimecreate = !onetimecreate;
                }
                else
                if (onetimecreate)
                {
                    GameObject Sphere = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    Sphere.transform.position = hit.point;
                    Sphere.transform.position += new Vector3(0, 0, 1);
                    Sphere.transform.LookAt(transform.position, Vector3.forward);
                    Sphere.transform.localScale = hit.transform.lossyScale;
                    onetimecreate = !onetimecreate;
                    Sphere.name = "凸";
                    Checker = false;
                }
                if (hit_Around.Length == 1)
                {
                    Checker = false;
                    hit_Around = Physics2D.OverlapCapsuleAll
                    (hit.transform.position,
                    new Vector2(hit.transform.lossyScale.x * 2, hit.transform.lossyScale.y * 2),
                    CapsuleDirection2D.Horizontal,
                    transform.rotation.z,
                    LayerMask.GetMask("PostProcessing"));
                }
                if (!Checker)
                    Debug.Log("凸");
                else
                    Debug.Log("凹");

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
                        //もし、hit位置よりも水玉コライダ1つ分以上離れていた時
                        //if (Vector2.Distance(hit.point, Around_one.transform.position) > hit.transform.lossyScale.x * 0.56f
                        //    && hitWay*)
                        ////対象が（obj）から見て下方向のとき
                        //{
                        //    Trytarget2 = hit.collider.gameObject;
                        //}
                        if (Trytarget2 == null)
                            Trytarget2 = hit.collider.gameObject;
                    }
                    LinePointers.Add(Trytarget1.transform.position + hitrange);
                    LinePointers.Add(Trytarget2.transform.position + hitrange);
                }
            }
        }
    }

    //外積計算後・左右判定
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
    void LineTracer()
    {
        int oen = 0;
        foreach (Vector2 point in LinePointers)
        {
            oen++;
            line.positionCount = oen;
            line.SetPosition(oen - 1, point);
        }
        //リストの全削除
        LinePointers.Clear();
    }
}
