using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{

    Vector2 Hit_margin;
    private bool Container_IN = false;
    public bool onetimecreate = false;
    
    //後で返値を与えること。Vector2 m_dirVec;
    public Vector2 Rerurn_dirVec(Vector2 original , RaycastHit2D target, Vector2 m_dirVec)
    {
        Debug.DrawLine(original, target.transform.position, Color.yellow, 10);
        //屈折のための余白 (*1)
        Direction_check(original, target);

        //Rayと水との間に容器があるとき(*2)
//        float distance = Vector2.Distance(transform.position, target.transform.position);
//        RaycastHit2D container_box = Physics2D.Raycast(original, m_dirVec, distance, LayerMask.GetMask("Container"));
//        if (container_box)
//            if (container_box.collider.gameObject.layer == LayerMask.NameToLayer("Container"))
//            {
//                float ori_wat_distance = Vector2.Distance(transform.position, container_box.transform.position);
//                float ori_con_distance = Vector2.Distance(transform.position, target.transform.position);
//                if (ori_wat_distance - ori_con_distance > 0)
//                    Container_IN = true;
//            }
//        //Rayと水の間に容器があるなら容器の上向きベクトルを返す(*2)
        Vector2 vec;
//        if (Container_IN)
//        {
//            Debug.DrawLine(container_box.transform.position, container_box.transform.position + container_box.transform.up, Color.red, 20);
//            return container_box.transform.up;
//        }
//        else
            //Rayと水との間に容器がない場合(*3)
            vec = WaterSurfar(target,m_dirVec);

        Debug.DrawLine(original, original + original * m_dirVec, Color.yellow, 10);
        
        return vec;
    }
    //Rayの飛んできた方向を取り、屈折をずらすための水滴の余白分を記録 (*1)
    void Direction_check(Vector2 original,RaycastHit2D target)
    {
        var UpWay = target.transform.forward;
        target.transform.LookAt(original);
        Hit_margin = target.transform.forward * target.transform.lossyScale.x * 0.3f;
        target.transform.forward = UpWay;
    }

    Vector2 WaterSurfar(RaycastHit2D hit,Vector2 dir)
    {
        //カプセル中心点//カプセルの範囲Vector3(x,y)//カプセル丸みの着く方向()か０か。//角度
        Collider2D[] hit_Around = Physics2D.OverlapCapsuleAll
            (hit.point + (Vector2)Hit_margin * 0.2f,
            new Vector2(hit.transform.lossyScale.x, hit.transform.lossyScale.y),
            CapsuleDirection2D.Vertical,
            transform.rotation.z,
            LayerMask.GetMask("PostProcessing"));


        bool Checker = true;
        if (onetimecreate && hit_Around.Length != 1)
        {
            GameObject Sphere = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Sphere.transform.position = hit.point + (Vector2)Hit_margin * 0.2f;
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
                //    && Hit_margin*)
                ////対象が（obj）から見て下方向のとき
                //{
                //    Trytarget2 = hit.collider.gameObject;
                //}
                if (Trytarget2 == null)
                    Trytarget2 = hit.collider.gameObject;
            }
            //ベクトル3 vec =（target.transform.position - transform.position）.normalized;target方向の向きが出る。
            Vector2 vec = (Trytarget1.transform.position - Trytarget2.transform.position).normalized;
            //            LinePointers.Add(Trytarget1.transform.position + hitrange);
            //            LinePointers.Add(Trytarget2.transform.position + hitrange);
            Debug.DrawLine(Trytarget1.transform.position, Trytarget2.transform.position, Color.green, 10);
            Debug.Log(vec);
            return vec;
        }
        return dir; 
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
    public bool InOut_Container()
    {
        return Container_IN;
    }
}
