using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            ColorChangeAll();
        }
    }

    void ColorChangeAll()
    {
        foreach(Transform child in transform)
        {
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            sprite.material.SetInt("_Ace", 1);//1で氷0で水
        }
    
    }

    public void TypeChange(List<SpriteRenderer> list,int type)
    {
        for(int i=0; i < list.Count; i++)
        {
            list[i].material.SetInt("_Ace", type);
        }
    }
}
