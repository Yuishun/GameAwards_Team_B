using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStartPoint : MonoBehaviour
{
    LightMove m_lightmove;

    public Material _mat;

    List<Vector3> points = new List<Vector3>();
    List<Vector2> vectors = new List<Vector2>();
    List<Color> colors = new List<Color>();
    List<Color> colors2 = new List<Color>();
    

    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> tris = new List<int>();
    

    Mesh mesh;
    int offset = 0;
    float xoffset = 0;
    float penSize = 0.3f;          // 筆の太さ
    //float uScrollSpeed = 0.18f;  // テクスチャの伸びる速度

    private void Awake()
    {
        m_lightmove = transform.GetComponentInChildren<LightMove>();
        m_lightmove.dirVec = transform.right;
        m_lightmove.lightpoint = this;

    }

    void CreateMesh(float size,int i)
    {
         //Vector2 plus90 = top + new Vector2(-dir.y, dir.x) * size;
         //Vector2 minus90 = top + new Vector2(dir.y, -dir.x) * size;

        Vector2 verticesVec = Vector2.Lerp(vectors[i - 1], vectors[i], 0.5f);
        Mathf.Abs(verticesVec.x);
        Mathf.Abs(verticesVec.y);

        Vector2 point = transform.InverseTransformPoint(points[i]);
        Vector2 pluspoint = point + verticesVec * size;
        Vector2 minuspoint = point + -verticesVec * size;


        // 頂点を追加
        this.vertices.Add(minuspoint);
        this.vertices.Add(pluspoint);

        // UVを追加
        this.uvs.Add(new Vector2(xoffset, 0));
        this.uvs.Add(new Vector2(xoffset, 1));
        //xoffset += (top - prev).magnitude / 6.0f;////uScrollSpeed; 

        // インデックスを追加
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

        // 頂点を２つ生成
        tp = transform.InverseTransformPoint(tp);
        this.vertices.Add(tp + transform.up * penSize);
        this.vertices.Add(tp - transform.up * penSize);
      
        // uv座標を設定
        this.uvs.Add(new Vector2(0, 1f));
        this.uvs.Add(new Vector2(0, 0));
        this.offset = 0;

        // 初期色
        for(int i=0; i < 2; i++)
        this.colors.Add(Color.yellow);

        vectors.Add(transform.right);

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
}
