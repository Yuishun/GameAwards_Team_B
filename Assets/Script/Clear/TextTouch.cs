using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTouch : MonoBehaviour
{
    [SerializeField]
    float distance;
    float t;   

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > 0.7f)
        {
            t = 0;
            Vector2 pos = transform.position;
            pos.y -= distance;
            transform.position = pos;
            distance *= -1;
        }
    }
}
