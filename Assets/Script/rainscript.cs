using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainscript : MonoBehaviour
{
    [SerializeField]
    GameObject town;
    GameObject cloud1;
    GameObject cloud2;
    GameObject cloud3;

    void Start()
    {
        var tes = transform.GetComponent<ParticleSystem>().shape;
        tes.radius = 2.5f * town.transform.lossyScale.x;
        transform.position = town.transform.position + new Vector3(0, town.transform.lossyScale.y * 2);
        cloud1 = transform.GetChild(0).gameObject;
        cloud2 = Instantiate(cloud1, cloud1.transform.position + new Vector3(4, 0), Quaternion.identity, transform);
        cloud3 = Instantiate(cloud1, cloud1.transform.position - new Vector3(4, 0), Quaternion.identity, transform);
    }

    void Update()
    {
        cloud1.transform.position += new Vector3(0,0);
        cloud2.transform.position += new Vector3(0,0);
        cloud3.transform.position += new Vector3(0,0);
    }
}
