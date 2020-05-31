using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStartPoint : MonoBehaviour
{
    LightMove m_lightmove;

    public Material _mat;
    public Color vertColor = Color.gray;

    List<Vector2> points = new List<Vector2>();
    List<Vector2> vectors = new List<Vector2>();
    List<Color> colors = new List<Color>();
    

    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> tris = new List<int>();
    

    Mesh mesh;
    int offset = 0;
    float xoffset = 0;
    float penSize = 0.25f;          // 筆の太さ
    Collider2D[] cols = new Collider2D[1];

    private void Awake()
    {
        m_lightmove = transform.GetComponentInChildren<LightMove>();
        m_lightmove.dirVec = transform.right;
        m_lightmove.lightpoint = this;

    }

    void CreateMesh(float size,int i)
    {
        Vector2 verticesVec;
        if (i == points.Count - 1)
        {
            verticesVec = vectors[i];
            //verticesVec.y = Mathf.Abs(verticesVec.y);
        }
        else
        {
            Vector2 Lvec1 = new Vector2(-vectors[i - 1].y, vectors[i - 1].x);
            Vector2 Lvec2 = new Vector2(-vectors[i].y, vectors[i].x);
            verticesVec = Vector2.Lerp(Lvec1, Lvec2, 0.5f).normalized;

            cols[0] = null;
            if (Physics2D.OverlapCircleNonAlloc(points[i], 0.05f,
                cols, LayerMask.GetMask("Default")) == 1)
            {
                if (cols[0].transform.tag == "Mirror")
                {
                    verticesVec = new Vector2(verticesVec.y, -verticesVec.x);
                }
            }
        }


        //Debug.Log("vertVec" + verticesVec);
        verticesVec = transform.InverseTransformVector(verticesVec);            
        
        Vector2 point = transform.InverseTransformPoint(points[i]);
        Vector2 leftpoint = point + verticesVec * size;
        Vector2 rightpoint = point - verticesVec * size;

        // 頂点を追加
        if (LineSegmentsIntersection(vertices[offset], leftpoint,
            vertices[offset + 1], rightpoint))
        {
            this.vertices.Add(rightpoint);
            this.vertices.Add(leftpoint);
        }
        else
        {
            this.vertices.Add(leftpoint);
            this.vertices.Add(rightpoint);
        }        

        //Debug.Log("vec" + transform.InverseTransformVector(vectors[i - 1])
        //    + "vert" + (vertices[offset + 2] - vertices[offset]).normalized);

        // UVを追加
        this.uvs.Add(new Vector2(xoffset, 0));
        this.uvs.Add(new Vector2(xoffset, 1));
        xoffset = (xoffset + 1) % 2;

        //インデックスを追加
        
        this.tris.Add(offset);
        this.tris.Add(offset + 1);
        this.tris.Add(offset + 2);
        this.tris.Add(offset + 1);
        this.tris.Add(offset + 3);
        this.tris.Add(offset + 2);
        
        offset += 2;

    }

    public void DrawLine()
    {
        for(int i = 1; i < points.Count ; i++)
        {
            CreateMesh(penSize, i);
        }

        mesh.vertices = this.vertices.ToArray();
        mesh.uv = this.uvs.ToArray();
        mesh.triangles = this.tris.ToArray();
        mesh.colors = this.colors.ToArray();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().material = _mat;

    }

    public void LineReset()
    {
        Vector3 tp = this.transform.position;
        // 初期化
        this.points.Clear();
        this.vectors.Clear();
        this.colors.Clear();
        this.vertices.Clear();
        this.uvs.Clear();
        this.tris.Clear();

        // 開始点を保存
        this.points.Add(tp);

        // ベクトルを保存
        vectors.Add(transform.right);

        // 頂点を２つ生成
        tp = transform.InverseTransformPoint(tp);        
        Vector3 vec = transform.InverseTransformVector(transform.up);        
        
        this.vertices.Add(tp + vec * penSize);
        this.vertices.Add(tp - vec * penSize);
      
        // uv座標を設定
        this.uvs.Add(new Vector2(0, 0f));
        this.uvs.Add(new Vector2(0, 1));
        this.offset = 0;
        this.xoffset = 1;

        // 初期色
        for(int i=0; i < 2; i++)
        this.colors.Add(vertColor);

        // メッシュ生成
        this.mesh = new Mesh();
    }

    public void AddLineRenderer(Vector3 point,Vector2 vector,Color color)
    {
        points.Add(point);
        vectors.Add(vector);
        for(int i=0;i<2;i++)
        colors.Add(color);
    }

    public static bool LineSegmentsIntersection(
    Vector2 p1,
    Vector2 p2,
    Vector2 p3,
    Vector3 p4)
    //out Vector2 intersection)
    {
       // intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        //intersection.x = p1.x + u * (p2.x - p1.x);
        //intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}
