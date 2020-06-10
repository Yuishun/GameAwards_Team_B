using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    [SerializeField]
    Sprite seed;
    [SerializeField]
    Sprite sprout;
    [SerializeField]
    Sprite bloom;
    [SerializeField]
    Sprite flower;
    [SerializeField]
    Material material;
    SpriteRenderer SpriteRender;
    float Alpha = 1.0f;
    float swingtime = 0;
    bool swingway = true;

    //衝突タイマー
    int m_iCollisionTimer = 0;
    //衝突時間（2秒後ステージ遷移）
    int m_iLimitCollisionTime = 150;
    //衝突フラグ
    bool m_bCollisiontFlag = false;
    Collider2D[] col = new Collider2D[1];
    [SerializeField]
    Color ClearColor = Color.white;
    [SerializeField]
    int stagenum;
    //一度だけゴール判定
    bool m_bGoal = false;
    GameObject ef_flower;
    ParticleSystem.MinMaxGradient[] color;
    AudioSource audioSource;

    ParticleSystem sield;
    void Start()
    {
        SpriteRender = transform.GetComponent<SpriteRenderer>();
        SpriteRender.material = material;
        material.color = new Color(1, 1, 1, 1);
        SpriteRender.sprite = seed;
        StartCoroutine("Swing");
        ef_flower = Resources.Load<GameObject>("FlowerEffect");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = PlayerPrefs.GetFloat("SE");
        audioSource.clip = Resources.Load<AudioClip>("Sound\\SE\\swords04");
        sield = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void GoalFlower()
    {
        audioSource.clip = Resources.Load<AudioClip>("Sound\\SE\\ClearFloweerSE");
        GameObject.FindGameObjectWithTag("GameController").GetComponent<InputController>().NoControll();
        StopCoroutine("Swing");
        StartCoroutine("Blooming");
        transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
    }

    public void SkipFlower()
    {
        StopCoroutine("Blooming");
        SpriteRender.sprite = flower;
        material.color = new Color(1, 1, 1, 1);

        var find = GameObject.Find("AllSceneManager");
        if (find)
            find.GetComponent<SceneManagerScript>().OneStageClear(stagenum);
    }

    IEnumerator Blooming()
    {
        audioSource.Play();
        var rot = Quaternion.Euler(-90, 0, 0);
        
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.up = transform.root.up;
        var eff = Instantiate(ef_flower, transform.position, rot, transform);
        eff.transform.localRotation = rot;
        var Psys = eff.GetComponent<ParticleSystem>();
        var main = Psys.main;
        
        main.startColor = StageSelectScript.nowStageColor;
        if(StageSelectScript.nowStageColor.color.a == 0)
            main.startColor = new Color(255, 255, 255, 255);
        Psys.Play();

        //FadeOut
        for (int i = 0; i < 8 * 2; i++) 
        {
            Alpha -= 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return new WaitForSeconds(0.05f);
        }
        //画像切り替え：芽
        SpriteRender.sprite = sprout;
        //FadeIn
        for (int i = 0; i < 8; i++) 
        {
            Alpha += 0.1f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //FadeOut
        for (int i = 0; i < 8 * 2; i++) 
        {
            Alpha -= 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return new WaitForSeconds(0.05f);
        }
        //画像切り替え：蕾
        SpriteRender.sprite = bloom;
        //FadeIn
        for (int i = 0; i < 8; i++)
        {
            Alpha += 0.1f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        //FadeOut
        for (int i = 0; i < 8 * 2; i++) 
        {
            Alpha -= 0.05f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return new WaitForSeconds(0.05f);
        }
        //画像切り替え：花
        SpriteRender.sprite = flower;
        //FadeIn
        for (int i = 0; i < 8; i++)
        {
            Alpha += 0.1f;
            material.color = new Color(1, 1, 1, Alpha);
            yield return null;
        }
        material.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(1.5f);
        var find = GameObject.FindWithTag("AllScene");
        if (find)
            find.GetComponent<SceneManagerScript>().OneStageClear(stagenum);
    }
    IEnumerator Swing()
    {
        while (true)
        {
            var angle = 0f;
            swingtime += Time.deltaTime;
            if (swingtime >= 1)
            {
                swingtime = 0;
                swingway = !swingway;
                transform.up = transform.root.up;
            }
            if (swingway)
                angle = Mathf.LerpAngle(-10, 10, swingtime);
            else
                angle = Mathf.LerpAngle(10, -10, swingtime);
            transform.localEulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
    }

    private void Update()
    {
        if (!m_bGoal)
        {
            col[0] = null;
            Physics2D.OverlapCircleNonAlloc(transform.position 
                +new Vector3(-0.3f*transform.localScale.x,-0.5f*transform.localScale.y,0),
            0.7f,
            col, LayerMask.GetMask("Light"));
            if (col[0])
            {
                if (col[0].GetComponent<LightMove>().m_color
                    != ClearColor)
                    return;


                if (m_iCollisionTimer++ % 60 == 0)
                {
                    sield.Play();
                    audioSource.Play();
                }
                if (m_iCollisionTimer > m_iLimitCollisionTime)
                {
                    m_bGoal = !m_bGoal;
                    GoalFlower();
                }
            }
            else if (m_iCollisionTimer > 0)
            {
                m_iCollisionTimer = 0;
            }
        }
        else
        {
            if (Input.GetButtonDown("Button_A"))
            {
                SkipFlower();
            }
        }
    }
}
