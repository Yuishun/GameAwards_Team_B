using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainscript : MonoBehaviour
{
    [SerializeField]
    GameObject town;

    void Start()
    {
        var tes = transform.GetComponent<ParticleSystem>().shape;
        Debug.Log(Screen.width);
        Debug.Log(Camera.main.aspect);
        tes.radius = 2.5f * town.transform.lossyScale.x;
        transform.position = town.transform.position + new Vector3(0, town.transform.lossyScale.y * 3);
    }
}
