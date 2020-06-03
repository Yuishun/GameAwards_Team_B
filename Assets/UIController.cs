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
    [SerializeField]
    RectTransform Icon_Frask;
    Image Icon_Frask_Rendrer;
    [SerializeField]
    RectTransform Icon_A, Icon_B, Icon_X, Icon_Y, Icon_R, Icon_L;
    [SerializeField]
    RectTransform Icon_Lst, Icon_Rst;
    bool m_bIconSpin = false;
    [SerializeField,Range(0.1f,1f)]
    float BidIconMagnification = 1;
    Vector2 BigIconvec, smallIconvec;
    Vector2 BigIconpos, smallIconpos;
    [SerializeField, Header("アイコン移動速度"), Range(0.1f, 4)]
    float IconSlideSpeed = 1;
    Image Icon_rotImage;
    [SerializeField]
    Sprite Icon_Ice_Frask, Icon_Unzip_Frask;
    Image Icon_X_Arrow, Icon_Y_Arrow;
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        gravityController = transform.root.GetChild(1).GetComponent<GravityControllerScript>();
        //UI_NowRotframe
        UI_NowRotframe = transform.GetChild(0).GetComponent<RectTransform>();
        UI_NowRotframe.sizeDelta = new Vector2(worldScreenHeight * 20, 80);
        UI_NowRotframe.localPosition += new Vector3(-UI_NowRotframe.rect.width * 0.5f, -UI_NowRotframe.rect.height * 0.5f);
        //UI_MoveRotframe
        UI_MoveRotframe = transform.GetChild(1).GetComponent<RectTransform>();
        UI_MoveRotframe.sizeDelta = UI_NowRotframe.sizeDelta;
        UI_MoveRotframe.localPosition = UI_NowRotframe.localPosition + new Vector3(0,-UI_NowRotframe.rect.height*0.5f) + new Vector3(0, -UI_MoveRotframe.rect.height*0.5f);
        //UI_NowRotText
        UI_NowRotText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        UI_NowRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_NowRotframe.rect.width, UI_NowRotframe.rect.height);
        //UI_MoveRotText
        UI_MoveRotText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        UI_MoveRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_MoveRotframe.rect.width, UI_MoveRotframe.rect.height);

        //Ice/Unzipアイコン左下端
        Icon_Unzip.sizeDelta *= BidIconMagnification;
        Icon_Ice.transform.localPosition += new Vector3(Icon_Ice.rect.width * 0.5f, Icon_Ice.rect.height * 0.5f);
        Icon_Unzip.transform.localPosition += new Vector3(Icon_Unzip.rect.width * 0.5f, Icon_Unzip.rect.height * 0.5f);
        BigIconvec = Icon_Unzip.sizeDelta;
        BigIconpos = Icon_Unzip.localPosition;
        smallIconvec = Icon_Ice.sizeDelta;
        smallIconpos = Icon_Ice.localPosition;
        //Icon_Frask
        Icon_Frask_Rendrer = Icon_Frask.gameObject.GetComponent<Image>();
        Icon_Frask_Rendrer.sprite = Icon_Ice_Frask;

        //RotIcon
        var rotwidth = UI_NowRotframe.sizeDelta.x;
        Icon_Rot.sizeDelta = new Vector2(rotwidth * 0.4f, rotwidth * 0.4f);
        Icon_Rot.transform.localPosition += new Vector3(-rotwidth * 0.56f, Icon_Rot.rect.height * 0.8f);

        Icon_rotImage = Icon_Rot.transform.GetChild(0).GetComponent<Image>();
        

        //LRボタン位置(RotIconに付随して動くため、RotIconを移動させること)
        var xypos = rotwidth;
        Icon_R.transform.localScale *= 0.8f;
        Icon_L.transform.localScale *= 0.8f;
        Icon_X.transform.localScale *= 0.8f;
        Icon_Y.transform.localScale *= 0.8f;
        Icon_R.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3( xypos*0.2f, xypos*0.3f, 0);
        Icon_L.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3(-xypos*0.2f, xypos*0.3f, 0);
        Icon_X.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3( xypos*0.2f,-xypos*0.3f, 0);
        Icon_Y.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3(-xypos*0.2f,-xypos*0.3f, 0);
        Icon_X_Arrow = Icon_X.GetChild(0).GetComponent<Image>();
        Icon_Y_Arrow = Icon_Y.GetChild(0).GetComponent<Image>();

        //LRStick
        Icon_Lst.transform.localPosition += new Vector3(80,0,0);
        Icon_Rst.transform.localPosition += new Vector3(120,0,0);


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
                    Icon_Rot.localScale = new Vector3(-1, 1, 1);
                }
                break;
            case 2:
                UI_MoveRotText.text = "左へ" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "中";
                Icon_Rot.transform.Rotate(Vector3.forward, 1);
                if (m_bIconSpin)
                {
                    m_bIconSpin = !m_bIconSpin;
                    Icon_Rot.localScale = new Vector3(1, 1, 1);
                }
                break;
        }
        var rollangle = gravityController.BackAngle();
        switch (rollangle)
        {
            case 10:
                Icon_rotImage.fillAmount = 0.33f;
                Icon_X_Arrow.fillAmount = 1f;
                Icon_Y_Arrow.fillAmount = 0.66f;
                break;
            case 15:
                Icon_rotImage.fillAmount = 0.66f;
                Icon_X_Arrow.fillAmount = 0.33f;
                Icon_Y_Arrow.fillAmount = 1f;
                break;
            case 20:
                Icon_rotImage.fillAmount = 1f;
                Icon_X_Arrow.fillAmount = 0.66f;
                Icon_Y_Arrow.fillAmount = 0.33f;
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
        Icon_Frask_Rendrer.sprite = Icon_Ice_Frask;
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
        Icon_Frask_Rendrer.sprite = Icon_Unzip_Frask;
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

