using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class clearscript : MonoBehaviour
{
    [SerializeField]
    Image panel;
    Vector3 pos;
    bool m_bflag = false;
    bool m_bButtoon = false;
    float timer = 0f;
    SceneManagerScript all;
    void Start()
    {
        pos = panel.rectTransform.localPosition;
        Invoke("Flagger",1);
        var manager = GameObject.FindWithTag("AllScene").gameObject;
        if (manager)
            all = manager.GetComponent<SceneManagerScript>();
        

    }
    void Flagger()
    {
        m_bflag = true;
    }


    void Update()
    {
        if (m_bflag)
        {
            timer += Time.deltaTime/10;
            panel.transform.localPosition = Vector3.Lerp(pos, Vector3.zero, timer);
            if (timer > 1)
            {
                m_bflag = false;
                Invoke("ActiveEnd", 1);   
            }
        }
        if (m_bButtoon)
        {
            if (Input.GetButtonDown("Button_A"))
            {
                Debug.Log("A");
                Debug.Log(all);
                if (all)
                    all.Loadstagenum(1000);
            }
        }
    }
    void ActiveEnd()
    {
        panel.transform.GetChild(1).gameObject.SetActive(true);
        m_bButtoon = true;
    }
}
