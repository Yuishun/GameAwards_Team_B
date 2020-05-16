using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot_Flower : MonoBehaviour
{
    SpriteRenderer spRend;
    [SerializeField]
    Sprite[] Flowers = new Sprite[10];
    int flowerindex = 0;

    [SerializeField]
    int Timelimit = 5;
    int time = 0;

    // Start is called before the first frame update
    void Start()
    {
        spRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (++time > Timelimit)
        {
            time = 0;
            flowerindex = (flowerindex + 1) % 10;
            spRend.sprite = Flowers[flowerindex];
        }
    }
}
