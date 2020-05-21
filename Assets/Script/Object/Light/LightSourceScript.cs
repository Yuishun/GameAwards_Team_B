using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceScript : MonoBehaviour
{
    LineRenderer line;
    public Material mat;
    List<Vector2> LinePointers = new List<Vector2>();
    Vector2 m_dirVec;
    [SerializeField]
    private float maxDistance = 10;
    [SerializeField]
    LayerMask FrameLayer, WaterLayer;
    void Start()
    {
        mat = Resources.Load<Material>("line");
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        line.startColor = Color.red;
        line.endColor = Color.magenta;
        line.material = mat;
        line.numCornerVertices = 3;
        m_dirVec = transform.right;
    }
    //=============================================================
    //メイン
    //=============================================================
    void Update()
    {
        RaycastHit2D hir = RayIrradiation();
    }

    //=============================================================
    //Ray
    //=============================================================
    private RaycastHit2D RayIrradiation()
    {
        Ray2D ray = new Ray2D(transform.position, m_dirVec);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,
            maxDistance, LayerMask.GetMask("Default", "PostProcessing"));
        if (hit.collider.gameObject.layer == WaterLayer)
            ;
        return hit;
    }

    //=============================================================
    //登録された地点を辿り、線を引く
    //=============================================================
    void LineTracer()
    {
        int linecount = 0;
        foreach (Vector2 point in LinePointers)
        {
            linecount++;
            line.positionCount = linecount;
            line.SetPosition(linecount - 1, point);
        }
        //リストの全削除
        LinePointers.Clear();
    }
}
