using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    RectTransform UI_NowRotframe, UI_MoveRotframe;
    Text UI_NowRotText, UI_MoveRotText;
    [SerializeField]
    string nowrot_afterwards = " / 360 度";
    [SerializeField]
    string moverot_afterwards = "度回転";
    GravityControllerScript gravityController;
    [SerializeField]
    RectTransform Icon_Ice, Icon_Unzip, Icon_Rot;
    bool m_bIconSpin = false;
    [SerializeField]
    float BidIconMagnification = 3;
    Vector2 BigIconvec, smallIconvec;
    Vector2 BigIconpos, smallIconpos;
    [SerializeField, Header("アイコン移動速度"), Range(0.1f, 4)]
    float IconSlideSpeed = 1;
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        gravityController = transform.root.GetChild(1).GetComponent<GravityControllerScript>();
        
        UI_NowRotframe = transform.GetChild(0).GetComponent<RectTransform>();
        UI_NowRotframe.sizeDelta = new Vector2(worldScreenHeight*20, 100);
        UI_NowRotframe.localPosition += new Vector3(-UI_NowRotframe.rect.width * 0.5f, -UI_NowRotframe.rect.height * 0.5f);

        UI_MoveRotframe = transform.GetChild(1).GetComponent<RectTransform>();
        UI_MoveRotframe.sizeDelta = UI_NowRotframe.sizeDelta;
        UI_MoveRotframe.localPosition = UI_NowRotframe.localPosition + new Vector3(0,-UI_NowRotframe.rect.height*0.5f) + new Vector3(0, -UI_MoveRotframe.rect.height*0.5f);

        UI_NowRotText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        UI_NowRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_NowRotframe.rect.width, UI_NowRotframe.rect.height);

        UI_MoveRotText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        UI_MoveRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_MoveRotframe.rect.width, UI_MoveRotframe.rect.height);

        //IceUnzipアイコン左下端
        Icon_Unzip.sizeDelta *= BidIconMagnification;
        Icon_Ice.transform.localPosition += new Vector3(Icon_Ice.rect.width * 0.5f, Icon_Ice.rect.height * 0.5f);
        Icon_Unzip.transform.localPosition += new Vector3(Icon_Unzip.rect.width * 0.5f, Icon_Unzip.rect.height * 0.5f);
        BigIconvec = Icon_Unzip.sizeDelta;
        BigIconpos = Icon_Unzip.localPosition;
        smallIconvec = Icon_Ice.sizeDelta;
        smallIconpos = Icon_Ice.localPosition;
        

        Icon_Rot.sizeDelta = new Vector2(UI_NowRotframe.sizeDelta.x, UI_NowRotframe.sizeDelta.x);
        Icon_Rot.transform.localPosition = UI_MoveRotframe.localPosition - new Vector3(0, Icon_Rot.rect.height * 0.5f + UI_MoveRotframe.rect.height * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        var way = gravityController.BackRollWay();
        switch (way)
        {
            case 0:
                UI_MoveRotText.text = "次は" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "。";
                m_bIconSpin = true;
                break;
            case 1:
                UI_MoveRotText.text = "右へ" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "中";
                Icon_Rot.transform.Rotate(Vector3.forward, -1);
                if(m_bIconSpin)
                {
                    m_bIconSpin = !m_bIconSpin;
                    Icon_Rot.localScale = new Vector3(1, 1, 1);
                }
                break;
            case 2:
                UI_MoveRotText.text = "左へ" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "中";
                Icon_Rot.transform.Rotate(Vector3.forward, 1);
                if (m_bIconSpin)
                {
                    m_bIconSpin = !m_bIconSpin;
                    Icon_Rot.localScale = new Vector3(-1, 1, 1);
                }
                break;
        }

        var diff = -Camera.main.transform.up;
        var axis = Vector3.Cross(-Vector3.up, diff);
        var angle = Vector3.Angle(-Vector3.up, diff);
        if (axis.z < 0)
        
            UI_NowRotText.text = (360 - angle).ToString("F0") + nowrot_afterwards;
        else
            UI_NowRotText.text = angle.ToString("F0") + nowrot_afterwards;
    }
    public void IceIcon()
    {
        StartCoroutine("IceTurn");
    }
    public void UnzipIcon()
    {
        StartCoroutine("UnzipTurn");
    }

    //**************************************************************************::
    //アイコン動作用
    //**************************************************************************::
    IEnumerator IceTurn()
    {
        Icon_Ice.gameObject.transform.SetSiblingIndex(3);
        yield return new WaitForEndOfFrame();
        var timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime * IconSlideSpeed;
            Icon_Ice.localPosition = Vector3.Lerp(smallIconpos, BigIconpos, timer);
            Icon_Unzip.localPosition = Vector3.Lerp(BigIconpos, smallIconpos, timer);
            
            Icon_Ice.sizeDelta = Vector3.Lerp(smallIconvec, BigIconvec, timer); 
            Icon_Unzip.sizeDelta = Vector3.Lerp(BigIconvec, smallIconvec, timer);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator UnzipTurn()
    {
        Icon_Unzip.gameObject.transform.SetSiblingIndex(3);
        yield return new WaitForEndOfFrame();
        var timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime * IconSlideSpeed;
            Icon_Ice.localPosition = Vector3.Lerp(BigIconpos, smallIconpos, timer);
            Icon_Unzip.localPosition = Vector3.Lerp(smallIconpos, BigIconpos, timer);

            Icon_Ice.sizeDelta = Vector3.Lerp(BigIconvec, smallIconvec, timer); 
            Icon_Unzip.sizeDelta = Vector3.Lerp(smallIconvec, BigIconvec, timer);
            yield return new WaitForEndOfFrame();
        }        
    }
}

