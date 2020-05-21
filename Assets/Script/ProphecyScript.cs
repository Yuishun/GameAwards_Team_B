using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProphecyScript : MonoBehaviour
{
    public static bool flag = false;
    bool wayflag = true;
    bool onceflag = false;
    [SerializeField]
    Transform nextIcon;
    bool IconFlag = true;
    bool BookOpener = true;

    [SerializeField]
    private RawImage m_iImage = null;
    private Texture2D m_texture = null;
    
    [SerializeField]
    TextScript textScript;
    [SerializeField]
    StageSelectScript stageSelectScript;
    private Vector3 targetPos;
    
    
    [SerializeField, Header("座標取得対象パーティクル")]
    private ParticleSystem m_targetParticleSystem;
    public ParticleSystem targetParticleSystem
    {
        get { return m_targetParticleSystem; }
    }
    [SerializeField]
    GameObject RainObjEffect;
    [SerializeField, Header("雨の表現。粒or流。✓=粒")]
    bool WhichRainObjEffect;

    [SerializeField, Header("雨。軽量版")]
    ParticleSystem RainEffect;
    [SerializeField, Header("雨粒。✓=軽量版")]
    bool WhichRainBlot;

    [SerializeField, Header("本の表紙・中1・中2")]
    Texture booktex, booktex2, booktex3;
    RectTransform bookleft, bookright;
    RawImage bookl_maintex, bookr_maintex;
    [SerializeField]
    Color color1, color2;
    void Awake()
    {
        //1度だけのチェック
        if (SceneManagerScript.ProphecyCheck)
        {
            gameObject.SetActive(true);
            SceneManagerScript.ProphecyCheck = false;
            transform.GetChild(0).gameObject.SetActive(true);
            nextIcon = transform.GetChild(0).GetChild(4);
            nextIcon.transform.localPosition = 
                new Vector3(nextIcon.transform.localPosition.x,
                transform.transform.GetChild(0).GetChild(3).transform.localPosition.y+10,0);
            bookleft = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            bookright = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
            bookl_maintex = bookleft.GetComponent<RawImage>();
            bookr_maintex = bookright.GetComponent<RawImage>();

            StartCoroutine("BookOpen");
        }
        else
            Itdestroy();
        

        if (WhichRainObjEffect)
            transform.GetChild(1).transform.GetComponent<ParticleSystemRenderer>().enabled = false;

    }

    void Update()
    {
        if (!BookOpener)
        {
            if (flag)
            {
                transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                nextIcon.gameObject.SetActive(true);
                StartCoroutine(IconMove());
                flag = false;
                onceflag = true;
            }
            else
            {
                if (Input.GetButtonDown("Button_A"))
                {
                    if (onceflag)
                        Itdestroy();
                    else
                        textScript.SkipText();
                }
            }
            if (WhichRainObjEffect)
            {
                if (targetParticleSystem.particleCount > 0)
                {
                    var particleCount = targetParticleSystem.particleCount;
                    ParticleSystem.Particle[] targetParticles = new ParticleSystem.Particle[particleCount];
                    targetParticleSystem.GetParticles(targetParticles);
                    foreach (var particle in targetParticles)
                    {
                        if (particle.remainingLifetime >= 1.9f)
                        {
                            targetPos = targetParticleSystem.transform.TransformPoint(particle.position);
                            //Debug用
                            //DropPaint(screenpos);
                            if (WhichRainBlot)
                            {
                                RainEffect.transform.position = targetPos;
                                RainEffect.Emit(1);
                            }
                            else
                                Instantiate(RainObjEffect, targetPos, Quaternion.identity);
                        }
                    }
                }
            }
        }
    }
    IEnumerator IconMove()
    {
        while (IconFlag)
        {
            yield return new WaitForSeconds(0.4f);
            if (wayflag)
                nextIcon.localPosition += new Vector3(0, 10);
            else
                nextIcon.localPosition -= new Vector3(0, 10);
            wayflag = !wayflag;
        }
    }

    void Itdestroy()
    {
        stageSelectScript.SelectButtonActivateion();
        IconFlag = false;
        SceneManagerScript.SetButton_Active();
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    IEnumerator BookOpen()
    {
        bookleft.gameObject.transform.SetSiblingIndex(1);
        bookleft.localPosition = new Vector3(400, 0, 0);
        var timer = 0f;
        var flag = false;

        bookleft.pivot = new Vector2(0, 0.5f);
        while (timer <= 1) 
        {
            timer += Time.deltaTime*0.25f;
            if (!flag)
            {
                if (timer < 0.5f)
                {
                    float angle = Mathf.LerpAngle(0f, 90f, timer*2);
                    bookleft.eulerAngles = new Vector3(0, angle, 0);

                    bookr_maintex.color = Color.Lerp(color1, color2, timer * 2);
                }
                else
                {
                    flag = true;
                    bookl_maintex.texture = booktex2;
                    bookleft.localPosition = new Vector3(-800, 0, 0);
                    bookleft.pivot = new Vector2(1, 0.5f);
                }
            }
            if (flag)
            {
                float angle = Mathf.LerpAngle(-90f, 0f, timer * 2 - 1);
                bookleft.eulerAngles = new Vector3(0, angle, 0);

                bookl_maintex.color = Color.Lerp(color1, color2, timer * 2 - 1);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        BookOpener = false;
        textScript.BookReadStart();
    }
}
