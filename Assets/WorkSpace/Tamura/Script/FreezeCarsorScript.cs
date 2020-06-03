using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCarsorScript : MonoBehaviour
{
    [SerializeField]
    float MoveSpeed = 0.1f;
    Transform cam_root;
    SpriteRenderer spr;
    [SerializeField]
    private Color Freezecolor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
    [SerializeField]
    private Color Meltcolor = new Color(1.0f, 0.4f, 0.0f, 1.0f);

    private string Layer_Water = "PostProcessing";
    private float CasorRange;

    UIController sc_UIController;
    [SerializeField]
    Sprite Ice_flask, Unzip_flask;
    void Start()
    {
        spr = transform.GetComponent<SpriteRenderer>();
        //spr.color = Meltcolor;
        spr.sprite = Unzip_flask;

        cam_root = Camera.main.transform.root;
        CasorRange = transform.localScale.x * 0.6f;

        sc_UIController = transform.root.GetChild(0).GetComponent<UIController>();
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
        sc_UIController.IceIcon();
        //spr.color = Freezecolor;
        spr.sprite = Ice_flask;
        Collider2D target = Physics2D.OverlapCircle(transform.position, CasorRange, LayerMask.GetMask(Layer_Water));
        if (target)
            target.SendMessage("Freeze");
    }
    public void MeltIMage()
    {
        sc_UIController.UnzipIcon();
        //spr.color = Meltcolor;
        spr.sprite = Unzip_flask;
        Collider2D target = Physics2D.OverlapCircle(transform.position, CasorRange, LayerMask.GetMask(Layer_Water));
        if (target)
            target.SendMessage("Melt");
    }
}
