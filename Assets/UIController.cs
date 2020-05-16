using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    RectTransform UI_NowRotframe, UI_MoveRotframe;
    Text UI_NowRotText, UI_MoveRotText;
    string nowrot_afterwards = " / 360 度";
    string moverot_afterwards = "度回転";
    GravityControllerScript gravityController;
    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        gravityController = transform.root.GetChild(0).GetComponent<GravityControllerScript>();
        
        UI_NowRotframe = transform.GetChild(0).GetComponent<RectTransform>();
        UI_NowRotframe.sizeDelta = new Vector2(worldScreenHeight*20, 100);
        UI_NowRotframe.localPosition += new Vector3(UI_NowRotframe.rect.width * 0.5f, -UI_NowRotframe.rect.height * 0.5f);

        UI_MoveRotframe = transform.GetChild(1).GetComponent<RectTransform>();
        UI_MoveRotframe.sizeDelta = UI_NowRotframe.sizeDelta;
        UI_MoveRotframe.localPosition = UI_NowRotframe.localPosition + new Vector3(0,-UI_NowRotframe.rect.height*0.5f) + new Vector3(0, -UI_MoveRotframe.rect.height*0.5f);

        UI_NowRotText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        UI_NowRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_NowRotframe.rect.width, UI_NowRotframe.rect.height);

        UI_MoveRotText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        UI_MoveRotText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(UI_MoveRotframe.rect.width, UI_MoveRotframe.rect.height);
        
    }

    // Update is called once per frame
    void Update()
    {
        var way = gravityController.BackRollWay();
        switch (way)
        {
            case 0:
                UI_MoveRotText.text = "次は" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "　";
                break;
            case 1:
                UI_MoveRotText.text = "右へ" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "中";
                break;
            case 2:
                UI_MoveRotText.text = "左へ" + gravityController.BackAngle().ToString("F0") + moverot_afterwards + "中";
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
}
