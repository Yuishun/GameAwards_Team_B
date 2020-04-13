using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCarsorScript : MonoBehaviour
{
    [SerializeField]
    float MoveSpeed = 0.1f;
    Transform cam_root;
    SpriteRenderer spr;
    private Color Freezecolor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
    private Color Meltcolor = new Color(1.0f, 0.4f, 0.0f, 1.0f);

    private string Layer_Water = "PostProcessing";
    private string Layer_Ice = "IceProcessing";
    private float CasorRange;


    void Start()
    {
        spr = transform.GetComponent<SpriteRenderer>();
        cam_root = Camera.main.transform.root;
        CasorRange = transform.localScale.x * 0.6f;
    }
    public void ControllerColliderHit(int type)
    {
        switch (type)
        {
            case 1:
                transform.position -= cam_root.right * MoveSpeed;
                break;
            case 2:
                transform.position += cam_root.right * MoveSpeed;
                break;
            case 3:
                transform.position += cam_root.up * MoveSpeed;
                break;
            case 4:
                transform.position -= cam_root.up * MoveSpeed;
                break;
        }
    }

    public void FreezeImage()
    {
        spr.color = Freezecolor;
        Collider2D target = Physics2D.OverlapCircle(transform.position, CasorRange, LayerMask.GetMask(Layer_Water));
        
        if (target)
            target.SendMessage("Freeze");
    }
    public void MeltIMage()
    {
        spr.color = Meltcolor;
        Collider2D target = Physics2D.OverlapCircle(transform.position, CasorRange, LayerMask.GetMask(Layer_Ice));
        if (target)
            target.SendMessage("Melt");
    }
}
