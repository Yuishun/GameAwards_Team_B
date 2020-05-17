using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSpawner2 : MonoBehaviour
{

    /// <summary>
    /// Drops objects array.
    /// </summary>
    public GameObject[] WaterDropsObjects;

    /// <summary>
    /// The size of each drop.
    /// </summary>
    [Range(0f, 2f)] public float size = .45f;

    [SerializeField] GameObject mainCamera;
    Vector2 _topLeft, _bottomRight;

    GameObject _parent;
    // Start is called before the first frame update
    void Start()
    {
        _parent = new GameObject("_RainBalls2");        
        //_parent.hideFlags = HideFlags.HideInHierarchy;
        WaterDropsObjects[0].transform.SetParent(_parent.transform);
        //WaterDropsObjects[0].transform.localScale = new Vector3(size, size, 1f);
        //WaterDropsObjects[0].GetComponent<MetaballParticleClass>().Init();
        WaterDropsObjects[0].GetComponent<RainDropParticle>().Init();

        for (int i = 1; i < WaterDropsObjects.Length; i++)
        {
            WaterDropsObjects[i] = Instantiate(WaterDropsObjects[0], gameObject.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
            WaterDropsObjects[i].name = "WaterDrops" + i;
            //WaterDropsObjects[i].GetComponent<MetaballParticleClass>().Init();
            WaterDropsObjects[i].GetComponent<RainDropParticle>().Init();
            WaterDropsObjects[i].transform.SetParent(_parent.transform);            
            WaterDropsObjects[i].layer = WaterDropsObjects[0].layer;
            //WaterDropsObjects[i].SetActive(false);
        }

        WaterDropsObjects[0].SetActive(false);

        // 画面の左上を取得
        Camera _mainCamera = mainCamera.GetComponent<Camera>();
        Vector3 topLeft = _mainCamera.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        _topLeft = topLeft;

        // 画面の右下を取得
        Vector3 bottomRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        _bottomRight = bottomRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (WaterDropsObjects[WaterDropsObjects.Length - 1].GetComponent<RainDropParticle>().
            Active == true)
            Debug.Log("LengthMax");
        int rand = Random.Range(0, 10);
        if (rand == 0)
        {
            rand = Random.Range(1, 8);

            for (int i = 0; i < WaterDropsObjects.Length; i++)
            {
                RainDropParticle MetaBall = WaterDropsObjects[i].GetComponent<RainDropParticle>();

                if (MetaBall.Active == true)
                    continue;
                

                float x = Mathf.Lerp(_topLeft.x, _bottomRight.x, Random.Range(0f, 1f));
                float y = Mathf.Lerp(_topLeft.y, _bottomRight.y, Random.Range(0f, .3f));
                WaterDropsObjects[i].transform.position = new Vector3(x, y, 0);
                float size_ = Random.Range(0.6f, 1.3f);
                WaterDropsObjects[i].transform.localScale = new Vector3(size_ * 0.5f , size_ , 1f);
                MetaBall.rb.gravityScale = Random.Range(0.1f, 3f);
                //StartCoroutine(MetaBall.FallGravity(new Vector2(0, Random.Range(-1f,-10f))));
                MetaBall.Active = true;
                if (--rand < 1)
                    break;
            }

        }

        for (int i = 0; i < WaterDropsObjects.Length; i++)
        {
            RainDropParticle MetaBall = WaterDropsObjects[i].GetComponent<RainDropParticle>();

            if (MetaBall.Active == true)
            {
                

                if (WaterDropsObjects[i].transform.position.y <=
                    _bottomRight.y - 2f)
                    MetaBall.Active = false;
                
            }
        }
    }
}
