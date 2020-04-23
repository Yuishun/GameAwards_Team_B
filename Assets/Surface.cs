using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    Vector2 Hit_margin;
    string Layer_Container = "Container";
    string Layer_Water = "PostProcessing";

    private float WaterRange = 0;
    private bool OneCheckWaterRange = false;
    void Start()
    {
        //Time.timeScale = 0.05f;
    }
    //水の半径を取得
    void CheckRange(RaycastHit2D target)
    {
        WaterRange = target.transform.lossyScale.x * target.transform.GetComponent<CircleCollider2D>().radius;
        OneCheckWaterRange = !OneCheckWaterRange;
    }
    //=================================================================
    //入水面ここから
    //=================================================================
    public Vector2 ReInVector2(Vector2 originpos, RaycastHit2D target, Vector2 m_dirvec)
    {
        if (!OneCheckWaterRange) CheckRange(target);

        //Rayの飛んできた方向を取り、屈折をずらすための水滴の余白分を記録 (要らなさそう)
        //thickness(originpos,target);
        var vec = CheckTouchContainer(target, m_dirvec, originpos,true);
        Debug.DrawLine(originpos, target.point, Color.red);
        return vec;
    }
    //=================================================================
    //水面ずらし(必要であれば使うこと)
    //=================================================================
    void thickness(Vector2 origin, RaycastHit2D target)
    {
        var UpWay = target.transform.forward;
        target.transform.LookAt(origin);
        Hit_margin = target.transform.forward * target.transform.lossyScale.x * 0.3f;
        target.transform.forward = UpWay;
    }
    //=================================================================
    //容器・水面判定
    //=================================================================
    Vector2 CheckTouchContainer(RaycastHit2D target, Vector2 dir, Vector2 originpos, bool flag)
    {
        Vector2 revec;
        RaycastHit2D container;
        //容器が存在するとき。容器のベクトルを返す。
        if (flag)
            container = Physics2D.Raycast(target.point, -dir, WaterRange, LayerMask.GetMask(Layer_Container), 0, 2);
        else
            container = Physics2D.Raycast(target.point, dir, WaterRange, LayerMask.GetMask(Layer_Container), 0, 2);
        if (container)
        {
            //容器の壁が縦に長い時
            if (container.transform.lossyScale.x < container.transform.lossyScale.y)
            {
                if (originpos.x < container.transform.position.x)//左側に源
                    revec = -container.transform.right;
                else
                    revec = container.transform.right;
            }
            else
            {
                if (originpos.y < container.transform.position.y)//下側に源
                    revec = -container.transform.up;
                else
                    revec = container.transform.up;
            }
        }
        else
        //容器が存在しないとき。水面の法線ベクトルを返す。
        {
            if (flag)
                revec = Water_In_Normal(target, dir, originpos);
            else
                revec = Water_Out_Normal(target, dir, originpos);
        }
        return revec;
    }
    //=================================================================
    //入水面の計算
    //=================================================================
    Vector2 Water_In_Normal(RaycastHit2D hit, Vector2 dir,Vector2 originpos)
    {
        Collider2D[] hit_Around = Physics2D.OverlapCircleAll(
            hit.point + dir*WaterRange, 
            WaterRange * 2,
            LayerMask.GetMask(Layer_Water));
        //配列が空でないとき
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
                float target_distance = Vector2.Distance(originpos, Around_one.transform.position);

                if (target_distance < min_Firsttarget_distance)
                {
                    min_Firsttarget_distance = target_distance;
                    Trytarget1 = Around_one.gameObject;
                    //左右どちらにあるか判別
                    Side1 = SideDiscrimination(originpos, hit.transform.position, Trytarget1.transform.position);
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
                        Side2 = SideDiscrimination(originpos, hit.transform.position, Around_one.transform.position);
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
            //Trytarget1.transform.GetComponent<SpriteRenderer>().color = Color.black;
            //Trytarget2.transform.GetComponent<SpriteRenderer>().color = Color.white;
            //ここ調整する
            var a = Trytarget1.transform.position;
            var b = Trytarget2.transform.position;
            var c = new Vector3(a.x, 0, 1);

            var side1 = b - a;
            var side2 = c - a;

            var perp = Vector3.Cross(side1, side2);

            //もし、perpが元の方向と遠いほうに伸びたとき、値を逆にする
            var distance_hit_origin = Vector2.Distance(originpos, hit.transform.position);
            var distance_perp_origin = Vector2.Distance(originpos, hit.transform.position + perp);

            if (distance_hit_origin < distance_perp_origin)
                perp = -perp;

            Debug.DrawLine(a, b, Color.blue);
            Debug.DrawLine(a, a + perp, Color.green);


            return perp;
        }
        return dir;
    }

    //=================================================================
    //出水面ここから
    //=================================================================
    public Vector2 ReOutVector2(Vector2 originpos, RaycastHit2D target, Vector2 m_dirvec)
    {
        var vec = CheckTouchContainer(target, m_dirvec, originpos, false);
        Debug.DrawLine(originpos, target.point, Color.yellow);
        return vec;
    }
    //=================================================================
    //出水面の計算
    //=================================================================
    Vector2 Water_Out_Normal(RaycastHit2D hit, Vector2 dir, Vector2 originpos)
    {
        Collider2D[] hit_Around = Physics2D.OverlapCircleAll(
            hit.point + dir * WaterRange * 2,
            WaterRange * 2,
            LayerMask.GetMask(Layer_Water));
        //変更。dirの先にあるように変える？
        //配列が空でないとき
        if (hit_Around.Length > 1)
        {
            float max_Firsttarget_distance = float.MinValue;
            float max_Secondtarget_distance = float.MinValue;
            GameObject Trytarget1 = null;
            GameObject Trytarget2 = null;

            float Side1 = float.MinValue;
            float Side2 = float.MinValue;
            foreach (var Around_one in hit_Around)
            {
                //前回のが次高点の場合の一時置き場
                float onetime_distance = max_Firsttarget_distance;
                float onetime_Side = Side1;
                GameObject onetime_obj = Trytarget1;
                bool CheckFlag = false;
                //まずは普通に取得
                float target_distance = Vector2.Distance(originpos, Around_one.transform.position);

                if (target_distance > max_Firsttarget_distance)
                {
                    max_Firsttarget_distance = target_distance;
                    Trytarget1 = Around_one.gameObject;
                    //左右どちらにあるか判別
                    Side1 = SideDiscrimination(originpos, hit.transform.position, Trytarget1.transform.position);
                    CheckFlag = true;
                }
                //Firstで別のものが入ったときかつ、前回と方向が違う場合
                if (onetime_Side != Side1)
                {
                    if (onetime_distance > max_Secondtarget_distance)
                    {
                        max_Secondtarget_distance = onetime_distance;
                        Trytarget2 = onetime_obj;
                    }
                }
                else
                //判別したものとは違う方向の最高点を取得
                //Firstの前回と方向が同じのものが入った場合あるいは前回のFirstより長い場合
                if (target_distance > max_Secondtarget_distance)
                {
                    if (!CheckFlag)
                        //左右どちらにあるか判別
                        Side2 = SideDiscrimination(originpos, hit.transform.position, Around_one.transform.position);
                    else
                        //チェック済みの時
                        Side2 = Side1;
                    if (Side1 != Side2)
                    {
                        max_Secondtarget_distance = target_distance;
                        Trytarget2 = Around_one.gameObject;
                    }
                }
                if (Trytarget2 == null)
                    Trytarget2 = hit.collider.gameObject;
            }
            var a = Trytarget1.transform.position;
            var b = Trytarget2.transform.position;
            var c = new Vector3(a.x, 0, 1);

            var side1 = b - a;
            var side2 = c - a;

            var perp = Vector3.Cross(side1, side2);

            //もし、perpが元の方向と遠いほうに伸びたとき、値を逆にする
            var distance_hit_origin = Vector2.Distance(originpos, hit.transform.position);
            var distance_perp_origin = Vector2.Distance(originpos, hit.transform.position + perp);

            if (distance_hit_origin > distance_perp_origin)
                perp = -perp;

            Debug.DrawLine(a, b, Color.blue);
            Debug.DrawLine(a, a + perp, Color.green);


            return perp;
        }
        return dir;
    }
    //=================================================================
    //どちら側にいるかの計算
    //=================================================================
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
}
