using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Button_START"))
            if (SceneManagerScript.m_bFadeInEnd)
            {
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                GameObject.FindWithTag("AllScene").GetComponent<SceneManagerScript>().Loadstagenum(0);
            }
    }
}
