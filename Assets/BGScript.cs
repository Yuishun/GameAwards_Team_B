using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //カメラのスケール
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        //スプライトのスケール
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        //比率をスプライトのローカル座標に反映
        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height);
        Vector3 campos = Camera.main.transform.position;
        campos.z = 0;
        transform.position = campos;   
    }
}
