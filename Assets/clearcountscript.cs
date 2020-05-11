using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class clearcountscript : MonoBehaviour
{
    SceneManagerScript allScene;
    [SerializeField]
    StageSelectScript selectScript;
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.transform.localPosition += new Vector3(rt.rect.width * 0.5f, rt.rect.height * 0.5f);
        RectTransform rtchild = transform.GetChild(0).GetComponent<RectTransform>();
        rtchild.sizeDelta = new Vector2(rt.rect.width, rt.rect.height);
        Text text = rtchild.GetComponent<Text>();
        if (selectScript != null)
        {
            text.text = "ClearStage\n"+selectScript.BackClearStageNum().ToString("F0") + " / 7";
        }
    }
}
