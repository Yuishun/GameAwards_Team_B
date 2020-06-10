using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSpawner : MonoBehaviour
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
        _parent = new GameObject("_RainBalls");        
        //_parent.hideFlags = HideFlags.HideInHierarchy;
        WaterDropsObjects[0].transform.SetParent(_parent.transform);
        //WaterDropsObjects[0].transform.localScale = new Vector3(size, size, 1f);
        WaterDropsObjects[0].SetActive(false);

        for (int i = 1; i < WaterDropsObjects.Length; i++)
        {
            WaterDropsObjects[i] = Instantiate(WaterDropsObjects[0], gameObject.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject;
            WaterDropsObjects[i].name = "WaterDrops" + i;
            WaterDropsObjects[i].SetActive(false);
            WaterDropsObjects[i].transform.SetParent(_parent.transform);
           // WaterDropsObjects[i].transform.localScale = new Vector3(size, size, 1f);
            WaterDropsObjects[i].layer = WaterDropsObjects[0].layer;
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
        //if (WaterDropsObjects[WaterDropsObjects.Length - 1].GetComponent<MetaballParticleClass>().
        //    Active == true)
        //    Debug.Log("LengthMax");
        int rand = Random.Range(0, 8);
        if (rand == 0)
        {
            rand = Random.Range(1, 10);

            for (int i = 0; i < WaterDropsObjects.Length; i++)
            {
                

                if (WaterDropsObjects[i].activeSelf == true)
                    continue;
                

                float x = Mathf.Lerp(_topLeft.x, _bottomRight.x, Random.Range(0f, 1f));
                float y = Mathf.Lerp(_topLeft.y, _bottomRight.y, Random.Range(0f, 1f));
                WaterDropsObjects[i].transform.position = new Vector3(x, y, 0);
                float size_ = Random.Range(0.8f, 1.2f);
                WaterDropsObjects[i].transform.localScale = new Vector3(size_, size_, 1f);
                WaterDropsObjects[i].SetActive(true);
                if (--rand < 1)
                    break;
            }

        }

        for (int i = 0; i < WaterDropsObjects.Length; i++)
        {

            if (WaterDropsObjects[i].activeSelf == true)
            {
                Vector3 scale = WaterDropsObjects[i].transform.localScale;
                scale = new Vector3(scale.x * 0.99f, scale.y * 0.99f, 1);
                if (scale.x <= 0.1f)
                    WaterDropsObjects[i].SetActive(false);
                else
                    WaterDropsObjects[i].transform.localScale = scale;
            }
        }
    }
}
