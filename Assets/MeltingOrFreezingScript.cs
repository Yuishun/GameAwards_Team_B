using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingOrFreezingScript : MonoBehaviour
{
    private string Layer_Water = "PostProcessing";
    float Range;
    public bool FreezingFlag = false;
    Rigidbody2D rb;
    void Start()
    {
        Range = transform.lossyScale.x * transform.GetComponent<CircleCollider2D>().radius;
        rb = transform.GetComponent<Rigidbody2D>();
    }
    public void Melt()
    {
        Time.timeScale = 0;
        if (FreezingFlag)
        {
            FreezingFlag = false;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.None;
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask(Layer_Water), -1, 1);
            if (targets != null)
            {
                if (targets[0])
                    foreach (Collider2D col in targets)
                    {
                        col.transform.GetComponent<MeltingOrFreezingScript>().GetStatus(false);
                    }
            }
        }
        Time.timeScale = 1.0f;
    }
    public void Freeze()
    {
        Time.timeScale = 0;
        if (!FreezingFlag)
        {
            FreezingFlag = true;
            rb.isKinematic = true;
            rb.velocity *= 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask(Layer_Water), -1, 1);
            if (targets != null)
            {
                if (targets[0])
                    foreach (Collider2D col in targets)
                    {
                        col.transform.GetComponent<MeltingOrFreezingScript>().GetStatus(true);
                    }
            }
        }
        Time.timeScale = 1.0f;
    }

    public void GetStatus(bool FreezeFlag)
    {
        if (FreezeFlag)
            Freeze();
        else
            Melt();
    }
}
