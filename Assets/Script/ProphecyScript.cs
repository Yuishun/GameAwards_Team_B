using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProphecyScript : MonoBehaviour
{
    public static bool flag = false;
    bool wayflag = true;
    bool onceflag = false;
    Transform nextIcon;
    bool IconFlag = true;

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
    
    void Awake()
    {
        //1度だけのチェック
        if (SceneManagerScript.ProphecyCheck)
        {
            gameObject.SetActive(true);
            SceneManagerScript.ProphecyCheck = false;
            transform.GetChild(0).gameObject.SetActive(true);
            nextIcon = transform.GetChild(0).GetChild(3);
            nextIcon.transform.localPosition = new Vector3(nextIcon.transform.localPosition.x, transform.transform.GetChild(0).GetChild(2).transform.localPosition.y,0);
        }
        else
            Itdestroy();
        

        if (WhichRainObjEffect)
            transform.GetChild(1).transform.GetComponent<ParticleSystemRenderer>().enabled = false;
    }

    void Update()
    {
        if (flag)
        {
            transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            nextIcon.gameObject.SetActive(true);
            flag = false;
            onceflag = true;
            StartCoroutine(IconMove());
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

    IEnumerator IconMove()
    {
        while (IconFlag)
        {
            yield return new WaitForSeconds(0.4f);
            if (wayflag)
                nextIcon.position += new Vector3(0, 1);
            else
                nextIcon.position -= new Vector3(0, 1);
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
}
