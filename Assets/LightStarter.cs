using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStarter : MonoBehaviour
{
    FollowLight m_lightmove;
    //List<Vector2> LinePointers = new List<Vector2>();
    LineRenderer m_LRenderer;
    public Material mat;
    public LineRenderer line
    {
        get { return m_LRenderer; }
    }

    void Awake()
    {
        m_lightmove = transform.GetComponentInChildren<FollowLight>();
        m_lightmove.dirVec = transform.right;
        m_lightmove.lightpoint = this;

        mat = Resources.Load<Material>("line");
        m_LRenderer = this.gameObject.AddComponent<LineRenderer>();
        m_LRenderer.startWidth = 0.1f;
        m_LRenderer.widthMultiplier = 1;
        m_LRenderer.endWidth = 0.1f;
        m_LRenderer.sortingOrder = 1;
        m_LRenderer.startColor = Color.blue;
        m_LRenderer.endColor = Color.cyan;
        m_LRenderer.material = mat;
        m_LRenderer.numCornerVertices = 3;
        m_LRenderer.SetPosition(0, transform.position);
        m_LRenderer.SetPosition(1, transform.position);
        //LinePointers.Add(transform.position);
    }


    void Update()
    {

    }

}
