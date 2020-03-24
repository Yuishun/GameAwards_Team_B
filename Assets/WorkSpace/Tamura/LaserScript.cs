using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    LineRenderer line;
    Vector2 Point1, Point2;
    List<Vector2> LinePointers = new List<Vector2>();

    public Material mat;
    Calcutration calc = new Calcutration();
    WaterMemory wm = new WaterMemory();
    private bool Alone = true;

    public bool Flagger = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        LinePointers.Add(transform.position);
        //Rayの照射を水面処理へ受け渡し
        //WaterSurface(RayIrradiation());
        watett(RayIrradiation());
        NextLinePos();
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

        //Rayのデバッグ描画
        //        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.blue, 3, false);
        //Rayの物体までの描画
        LinePointers.Add(hit.point);
        return hit;
    }
    public bool onetimeFlag = true;
    void watett(RaycastHit2D hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PostProcessing"))
        {
            //範囲円内の水判定のコライダを全て記録
            Collider2D[] cols = Physics2D.OverlapCircleAll(hit.transform.position, hit.transform.lossyScale.x * 2, LayerMask.GetMask("PostProcessing"));

            Vector2 vec1 = transform.position;
            Vector2 vec2 = hit.transform.position;
            GameObject TRYPos1, TRYPos2;
            TRYPos1 = cols[0].gameObject;
            double Leng1, Leng2;
            Leng1 = collL(vec1, vec2);
            Leng2 = collL(vec1, cols[0].transform.position);
            TRYPos2 = cols[0].gameObject;
            foreach (Collider2D col in cols)
            {
                double onetime = collL(vec1, col.transform.position);

                if (Leng1 > onetime)
                {
                    if (onetimeFlag)
                    {
                        Debug.Log(Leng1);
                        Debug.Log(onetime);
                    }
                    Leng2 = Leng1;
                    TRYPos2 = TRYPos1;
                    Leng1 = onetime;
                    TRYPos1 = col.gameObject;
                }
            }
            LinePointers.Add(TRYPos1.transform.position);
            LinePointers.Add(TRYPos2.transform.position);
            if (onetimeFlag)
            {
                Debug.Log(Leng1);
                Debug.Log(Leng2);
            }
            onetimeFlag = false;
        }
    }
    double collL(Vector2 vec1,Vector2 vec2)
    {
        double dx, dy;
        double x1, x2, y1, y2;
        x1 = vec1.x;
        y1 = vec1.y;
        x2 = vec2.x;
        y2 = vec2.x;

        dx = x2 - x1;
        dy = y2 - y1;
        double L = Mathf.Sqrt((float)(dx * dx + dy * dy));
        return L;
    }
    //==========================================================================
    //水面処理2D
    //==========================================================================
    void WaterSurface(RaycastHit2D hit)
    {
        //=====================================
        //水判定
        //=====================================
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PostProcessing"))
        {
            //範囲円内の水判定のコライダを全て記録
            Collider2D[] cols = Physics2D.OverlapCircleAll(hit.transform.position, hit.transform.lossyScale.x * 2, LayerMask.GetMask("PostProcessing"));
            
            List<WaterMemory> Mem = new List<WaterMemory>();
            //範囲内コライダの探索==================================================================================
            //線分の開始地点A座標と、線分の終了地点B座標で、線分の1点目から2点目に向かってどちら側かを判定。
            //判定点C座標を　col　とする。

            //まず線分A-Bの単位ベクトルを求める
            Calcutration vecAB = calc.calcMethod(transform.position, hit.transform.position);
            Mem.Add(wm.wmMethod(vecAB.length, 0, hit.transform.position));
            int cal = 0;

            foreach (Collider2D col in cols)
            {
                cal++;
                WaterMemory wm = new WaterMemory();
                Calcutration calc = new Calcutration();
                if (Alone)
                    Alone = false;
                //同様に求める点colへのA-colの単位ベクトルを求める。
                Calcutration vecAcol = calc.calcMethod(transform.position, col.transform.position);
                //求めた2つの単位ベクトルZ成分を0として外積を求める。
                float S = vecAB.x * vecAcol.y - vecAB.y * vecAcol.x;

                //記録(距離・左右・位置)
                Mem.Add(wm.wmMethod(vecAcol.length, S, col.transform.position));
            }
            
            bool eflag = false;
            if(eflag)
            {
                //Mem記録を参照し、再計算
                //if (!Flagger)
                //{//反転中＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
                if (!Alone)
                {
                    //   Flagger = !Flagger;
                    int val = 0;
                    foreach (WaterMemory once in Mem)
                        val++;

                    //距離ソート

                    WaterMemory tmp;
                    Collider2D collidetmp;
                    for (int i = 0; i < cal; ++i)
                    {
                        for (int j = i + 1; j < cal; ++j)
                        {
                            if (Mem[i + 1].length > Mem[j + 1].length)
                            {
                                tmp = Mem[i + 1];
                                Mem[i + 1] = Mem[j + 1];
                                Mem[j + 1] = tmp;

                                collidetmp = cols[i];
                                cols[i] = cols[j];
                                cols[j] = collidetmp;
                            }
                        }
                    }

                    for (int i = 1; i < val - 1; ++i)
                        LinePointers.Add(cols[i].transform.position);
                    {
                        //チェック用
                        //                    for (int f = 0; f < 4; f++)
                        //                    {
                        //                        Debug.Log(Mem[f+1].vec);
                        //                        Debug.Log(Mem[f+1].length);
                        //                        Debug.Log(cols[f].transform.position);
                        //                    }
                        /*
                        for (int i = 1; i < val - 1; ++i)
                            LinePointers.Add(cols[i].transform.position);


                        Debug.Log(cols[0].transform.position);
                        Debug.Log(cols[1].transform.position);
                        Debug.Log(cols[2].transform.position);
                        Debug.Log(cols[3].transform.position);
                        Debug.Log(cols[4].transform.position);
                        //水面最短点2点を取得
                        Point1 = cols[1].transform.position;
                        Point2 = cols[2].transform.position;

                        cols[1].transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Circle 1");
                        cols[2].transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Circle 1");
                        cols[3].transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Circle 1");
                        cols[4].transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Circle 1");
    */

                        //                    if (Mem[2].length > Mem[0].length)
                        //                    {
                        //                        Point2 = Mem[0].vec;
                        //                        if (Mem[1].length > Mem[0].length)
                        //                        {
                        //                            Point2 = Point1;
                        //                            Point1 = Mem[0].vec;
                        //                        }
                        //                    }
                        //                    Debug.Log(Mem[0].vec);
                        //                    Debug.Log(Point1);
                        //                    Debug.Log(Point2);
                    }
                }
            }
            Mem.Clear();
        }
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
    //=============================================================
    //登録された２点から
    //=============================================================
    void NextLinePos()
    {
        if (Flagger)
        {
            Flagger = !Flagger;
            Vector2 point = new Vector2((Point1.x + Point2.x) * 0.5f, (Point1.y + Point2.y) * 0.5f);

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = point;            
        }
    }
}

//=============================================================
//外積・距離計算クラス
//=============================================================
class Calcutration
{
    public float x;
    public float y;
    public float length;

    public Calcutration calcMethod(Vector2 vec1,Vector2 vec2)
    {
        Calcutration calc = new Calcutration();


        double dx, dy;
        double x1, x2, y1, y2;
        x1 = vec1.x;
        y1 = vec1.y;
        x2 = vec2.x;
        y2 = vec2.x;

        dx = x2 - x1;
        dy = y2 - y1;

        double L = Mathf.Sqrt((float)(dx * dx + dy * dy));

        calc.x = (float)(dx / L + x1);
        calc.y = (float)(dy / L + y1);
        calc.length = (float)L;
        return calc;
    }
}

//=============================================================
//記憶クラス
//=============================================================
class WaterMemory
{
    public float length;
    public float side;
    public Vector2 vec;
    
    public WaterMemory wmMethod(float len,float S,Vector2 pos)
    {
        WaterMemory wm = new WaterMemory();
        wm.length = len;
        wm.side = S;
        wm.vec = pos;
        return wm;
    }
}
