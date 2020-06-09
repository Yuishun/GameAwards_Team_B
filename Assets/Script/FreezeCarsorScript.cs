using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCarsorScript : MonoBehaviour
{
    [SerializeField]
    float MoveSpeed = 0.1f;
    Transform cam_root;
    SpriteRenderer spr;

    private string Layer_Water = "PostProcessing";
    private float CasorRange;
    
    UIController sc_UIController;

    [SerializeField]
    Sprite Ice_flask, Unzip_flask;
    Vector3 Pos;
    float timer = 0;
    bool ResetPosflag = false;
    
    float distance;
    AudioSource audioSource;
    AudioClip melt_sound, ice_sound;
    void Start()
    {
        ice_sound = Resources.Load<AudioClip>("Sound\\SE\\glass1(cut)");
        melt_sound = Resources.Load<AudioClip>("Sound\\SE\\water-foll(cut)");
        
        audioSource = transform.gameObject.AddComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SE");
        audioSource.playOnAwake = false;

        spr = transform.GetComponent<SpriteRenderer>();
        spr.sprite = Unzip_flask;

        cam_root = Camera.main.transform.root;
        CasorRange = transform.localScale.x * 0.6f;

        sc_UIController = transform.root.GetChild(0).GetComponent<UIController>();
        distance = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
    }
    public void ControllerColliderHit(int type)
    {
        switch (type)
        {
            case 1:
                transform.position -= cam_root.right * MoveSpeed;
                sc_UIController.CarsorWay(1);
                break;
            case 2:
                transform.position += cam_root.right * MoveSpeed;
                sc_UIController.CarsorWay(2);
                break;
            case 3:
                transform.position += cam_root.up * MoveSpeed;
                sc_UIController.CarsorWay(3);
                break;
            case 4:
                transform.position -= cam_root.up * MoveSpeed;
                sc_UIController.CarsorWay(4);
                break;
        }        
    }
    void Update()
    {
        transform.localRotation = cam_root.localRotation;
        if (!ResetPosflag)
        {
            Pos = transform.localPosition;
            if(distance < Mathf.Abs(Vector3.Distance(Pos,Vector3.zero)))
                ResetPosflag = true;
        }
        else
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(Pos, Vector3.zero, timer);
            if (timer >= 1)
            {
                timer = 0;
                ResetPosflag = false;
            }
        }
    }
    public void FreezeImage()
    {
        sc_UIController.IceIcon();
        spr.sprite = Ice_flask;
        Collider2D target = Physics2D.OverlapCircle(transform.position, CasorRange, LayerMask.GetMask(Layer_Water));
        if (target)
        {
            audioSource.clip = ice_sound;
            target.SendMessage("Freeze");
            audioSource.Play();
        }
    }
    public void MeltIMage()
    {
        sc_UIController.UnzipIcon();
        spr.sprite = Unzip_flask;
        Collider2D target = Physics2D.OverlapCircle(transform.position, CasorRange, LayerMask.GetMask(Layer_Water));
        if (target)
        {
            audioSource.clip = melt_sound;
            target.SendMessage("Melt");
            audioSource.Play();
        }
    }

    public void SetRot()
    {
        transform.eulerAngles = cam_root.eulerAngles;
    }
}
