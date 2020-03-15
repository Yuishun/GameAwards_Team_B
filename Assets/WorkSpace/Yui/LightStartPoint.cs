using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStartPoint : MonoBehaviour
{
    LightMove m_lightmove;

    LineRenderer m_LRenderer;
    public LineRenderer line
    {
        get { return m_LRenderer; }
    }

    void Awake()
    {
        m_lightmove = transform.GetComponentInChildren<LightMove>();
        m_lightmove.dirVec = transform.right;
        m_lightmove.lightpoint = this;
        m_LRenderer = GetComponent<LineRenderer>();
        m_LRenderer.SetPosition(0, transform.position);
    }

    
    void Update()
    {
        
    }
}
