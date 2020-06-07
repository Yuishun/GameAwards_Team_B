using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField, Range(15, 20)]
    float RotFrame_size = 20;
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
    RectTransform Icon_Lst, Icon_Arrow;
    bool m_bIconSpin = false;
    [SerializeField, Range(0.1f, 1f)]
    float BidIconMagnification = 1;
    Vector2 BigIconvec, smallIconvec;
    Vector2 BigIconpos, smallIconpos;
    [SerializeField, Header("アイコン移動速度"), Range(0.1f, 4)]
    float IconSlideSpeed = 1;
    Image Icon_rotImage;
    [SerializeField]
    Sprite Icon_Ice_Frask, Icon_Unzip_Frask;
    Image Icon_X_Arrow, Icon_Y_Arrow;
    [SerializeField]
    Sprite arrow, arrowing;
    Image arrowimage;
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        gravityController = transform.root.GetChild(1).GetComponent<GravityControllerScript>();
        //UI_NowRotframe
        UI_NowRotframe = transform.GetChild(0).GetComponent<RectTransform>();
        UI_NowRotframe.sizeDelta = new Vector2(worldScreenHeight * RotFrame_size, RotFrame_size * 4);
        UI_NowRotframe.localPosition += new Vector3(-UI_NowRotframe.rect.width * 0.5f, -UI_NowRotframe.rect.height * 0.5f);
        //UI_MoveRotframe
        UI_MoveRotframe = transform.GetChild(1).GetComponent<RectTransform>();
        UI_MoveRotframe.sizeDelta = UI_NowRotframe.sizeDelta;
        UI_MoveRotframe.localPosition = UI_NowRotframe.localPosition + new Vector3(-UI_NowRotframe.rect.width, 0);//new Vector3(0, -UI_NowRotframe.rect.height * 0.5f) + new Vector3(0, -UI_MoveRotframe.rect.height * 0.5f);
        //UI_NowRotText
        UI_NowRotText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        UI_NowRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_NowRotframe.rect.width, UI_NowRotframe.rect.height);
        //UI_MoveRotText
        UI_MoveRotText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        UI_MoveRotText.gameObject.GetComponent<RectTransform>().sizeDelta = UI_MoveRotframe.sizeDelta;

        //Ice/Unzipアイコン左下端
        Icon_Unzip.sizeDelta *= BidIconMagnification;
        Icon_Ice.transform.localPosition += new Vector3(Icon_Ice.rect.width * 0.5f, Icon_Ice.rect.height * 0.5f);
        Icon_Unzip.transform.localPosition += new Vector3(Icon_Unzip.rect.width * 0.5f, Icon_Unzip.rect.height * 1.5f);

        //RotIcon
        //LRボタン位置(RotIconに付随して動くため、RotIconを移動させること)
        var roll0 = Icon_Rot.GetChild(0).GetComponent<RectTransform>();
        var roll1 = Icon_R.GetChild(0).GetComponent<RectTransform>();
        var roll2 = Icon_L.GetChild(0).GetComponent<RectTransform>();
        var roll3 = Icon_X.GetChild(0).GetComponent<RectTransform>();
        var roll4 = Icon_Y.GetChild(0).GetComponent<RectTransform>();
        var rotwidth = UI_NowRotframe.sizeDelta.x;
        roll0.sizeDelta =
        Icon_R.sizeDelta = Icon_L.sizeDelta = Icon_X.sizeDelta = Icon_Y.sizeDelta =
            roll1.sizeDelta = roll2.sizeDelta = roll3.sizeDelta = roll4.sizeDelta =
            Icon_Rot.sizeDelta = new Vector2(rotwidth, rotwidth) * 0.4f;

        Icon_Rot.transform.localPosition = Vector3.zero;
        Icon_Rot.transform.localPosition += new Vector3(-rotwidth * 0.5f, rotwidth * 0.5f);

        Icon_R.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3(rotwidth * 0.2f, rotwidth * 0.3f, 0);
        Icon_L.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3(-rotwidth * 0.2f, rotwidth * 0.3f, 0);
        Icon_X.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3(rotwidth * 0.2f, -rotwidth * 0.3f, 0);
        Icon_Y.transform.localPosition = Icon_Rot.transform.localPosition + new Vector3(-rotwidth * 0.2f, -rotwidth * 0.3f, 0);

        //rollArrow調整用
        Icon_X_Arrow = Icon_X.GetChild(0).GetComponent<Image>();
        Icon_Y_Arrow = Icon_Y.GetChild(0).GetComponent<Image>();
        Icon_rotImage = Icon_Rot.transform.GetChild(0).GetComponent<Image>();

        //Arrow positon
        Icon_Arrow.sizeDelta = Icon_Rot.sizeDelta * 2;
        arrowimage = Icon_Arrow.transform.GetComponent<Image>();
        arrowimage.sprite = arrow;
        Icon_Arrow.transform.localPosition += new Vector3(Icon_Arrow.rect.width * 0.5f, 0, 0);
        arrowimage.color = Color.red;

        //Icon_Frask
        Icon_Frask_Rendrer = Icon_Frask.gameObject.GetComponent<Image>();
        Icon_Frask_Rendrer.sprite = Icon_Unzip_Frask;
        Icon_Frask.sizeDelta = Icon_Arrow.sizeDelta * 0.5f;
        Icon_Frask.transform.localPosition = Icon_Arrow.localPosition + new Vector3(-rotwidth, rotwidth) * 0.2f;
        
        //Icon_Lst
        Icon_Lst.transform.localPosition = Icon_Arrow.localPosition;
        Icon_Lst.sizeDelta = Icon_Arrow.sizeDelta * 0.5f;
        
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
                if (m_bIconSpin)
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
    int oldval = 0;
    public void CarsorWay(int val)
    {
        if (val == 0)
            arrowimage.sprite = arrow;
        else
            arrowimage.sprite = arrowing;
        if (oldval != val)
            switch (val)
            {
                case 0:
                    arrowimage.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    arrowimage.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case 2:
                    arrowimage.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case 3:
                    arrowimage.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 4:
                    arrowimage.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
            }
        oldval = val;
    }
    public void IceIcon()
    {
        arrowimage.color = Color.cyan;
        Icon_Frask_Rendrer.sprite = Icon_Ice_Frask;

    }
    public void UnzipIcon()
    {
        arrowimage.color = Color.red;
        Icon_Frask_Rendrer.sprite = Icon_Unzip_Frask;

    }
}
