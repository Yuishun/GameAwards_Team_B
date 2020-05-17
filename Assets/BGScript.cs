using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    [SerializeField]
    GameObject SeaPlate;
    void Start()
    {
        Vector2 point = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        var ScreenUnderPos = new Vector2(0, -point.y);


        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //カメラのスケール
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        
        //スプライトのスケール
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        //比率をスプライトのローカル座標に反映
        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height);
        //transform.localScale *= 1.15f;


        Vector3 campos = Camera.main.transform.position;
        campos.z = 0;
        transform.position = campos;
        
                 

        var par = transform.GetChild(1).GetComponent<ParticleSystem>().shape;
        par.radius = 2.5f * transform.lossyScale.x;
        transform.GetChild(1).position = transform.position + new Vector3(0, transform.lossyScale.y * 3);

        var island = transform.GetChild(0).transform;
        island.localPosition -= new Vector3(0, island.root.transform.localScale.y * 0.5f);
        //island.localScale /= 1.15f;


        
        //海下端
        var sea = SeaPlate.transform.position - SeaPlate.transform.forward * SeaPlate.transform.lossyScale.y * 5;
        

        var disY = sea.y - ScreenUnderPos.y;
        SeaPlate.transform.position -= new Vector3(0, disY * 2);
        SeaPlate.transform.localScale = new Vector3(transform.lossyScale.x, 2, transform.lossyScale.y) * 0.52f;
        

    }
}

