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
    [SerializeField, Header("本の表紙・中1・中2")]
    Texture booktex, booktex2, booktex3;
    RectTransform bookleft, bookright;
    RawImage bookl_maintex, bookr_maintex;
    [SerializeField]
    Color color1, color2;
    [SerializeField]
    AudioSource audioSE;
    void Awake()
    {
        audioSE.volume = PlayerPrefs.GetFloat("SE");
        //1度だけのチェック
        if (SceneManagerScript.ProphecyCheck)
        {
            audioSE.Stop();
            gameObject.SetActive(true);
            SceneManagerScript.ProphecyCheck = false;
            transform.GetChild(0).gameObject.SetActive(true);
            nextIcon = transform.GetChild(0).GetChild(7);
            nextIcon.transform.localPosition = 
                new Vector3(nextIcon.transform.localPosition.x,
                transform.transform.GetChild(0).GetChild(6).transform.localPosition.y+10,0);
            bookleft = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            bookright = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
            bookl_maintex = bookleft.GetComponent<RawImage>();
            bookr_maintex = bookright.GetComponent<RawImage>();
            StartCoroutine("BookOpen");
        }
        else
            Itdestroy();
    }

    void Update()
    {
        if (!BookOpener)
        {
            if (flag)
            {
                transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
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
                    {
                        audioSE.Play();
                        Itdestroy();
                    }
                    else
                        textScript.SkipText();
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
    }

    IEnumerator BookOpen()
    {
        bookleft.gameObject.transform.SetSiblingIndex(4);
        bookleft.localPosition = new Vector3(bookleft.rect.width, 0, 0);
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
                    bookleft.localPosition = new Vector3(-bookleft.rect.width*2, 0, 0);
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
