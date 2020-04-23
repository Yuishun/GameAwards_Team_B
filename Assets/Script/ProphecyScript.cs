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
    //RectTransform rect;
    [SerializeField]
    TextScript textScript;
    private Vector3 targetPos;
    //[SerializeField] private Canvas canvas;
    
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
    //[SerializeField, Header("✓=Debug")]
    //bool DebugFlag;

    void Awake()
    {
        //1度だけのチェック
        if (AllSceneManager.ProphecyCheck)
        {
            AllSceneManager.ProphecyCheck = false;
            transform.GetChild(0).gameObject.SetActive(true);
            nextIcon = transform.GetChild(0).GetChild(3);
        }
        else
            Itdestroy();
        
        //if (DebugFlag)
        //{
        //    rect = m_iImage.gameObject.GetComponent<RectTransform>();
        //    StartCoroutine(Capture());
        //}

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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (onceflag)
                    Itdestroy();
                else
                    textScript.SkipText();
            }
        }
        if (WhichRainObjEffect)
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
        IconFlag = false;
        AllSceneManager.SetButton_Active();
        Destroy(gameObject);
    }

    /*
    //===========================================================
    // Debug.スクリーン座標に変換、赤点を打つ
    //===========================================================
    void DropPaint(Vector3 Pos)
    {
        var screenpos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, Pos);
        int width = 8;
        int height = 8;
        var Xposting = m_texture.width  * screenpos.x / Screen.width;
        var Yposting = m_texture.height * screenpos.y / Screen.height;
        for (int i = 0; i < height; ++i)
        {
            int y = (int)(Yposting + i);
            for (int j = 0; j < width; ++j)
            {
                int x = (int)(Xposting + j);
                if (y >= 0 && y <= m_texture.height)
                    if (x >= 0 && x <= m_texture.width)
                        m_texture.SetPixel(x, y, Color.red);
            }
        }
        m_texture.Apply();
    }
    //===========================================================
    // スクショを取って変換、RawImageのテクスチャに適応している。Debug用
    //===========================================================
    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();
        var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);

        TextureScale.Bilinear(texture, (int)(Screen.width * 0.947f), (int)(Screen.height * 0.935f));

        texture.ReadPixels(new Rect(25, 25, Screen.width - 55, Screen.height - 50), 0, 0);
        texture.Apply();

        byte[] pngdata = texture.EncodeToPNG();
        texture.LoadImage(pngdata);
        //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //m_iImage.material.SetTexture("_MainTex", texture);
        m_texture = texture;
        
        m_iImage.texture = m_texture;
        m_texture.Apply();
    }
    */
}
