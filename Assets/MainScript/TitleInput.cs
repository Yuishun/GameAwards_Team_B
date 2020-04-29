using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Button_START"))
            if (SceneManagerScript.m_bFadeInEnd)
                GameObject.FindWithTag("AllScene").GetComponent<SceneManagerScript>().Loadstagenum(0);
    }
}
