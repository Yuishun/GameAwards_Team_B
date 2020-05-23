using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_Rotate_Yellow : MonoBehaviour
{
    [SerializeField]
    Transform LContainer, RContainer;
    Vector3 MinAngle, MaxAngle;
    float t = 0;

    float PrevAngle;

    // Start is called before the first frame update
    void Start()
    {
        MinAngle = RContainer.localEulerAngles;
        MaxAngle = new Vector3(0,0,30);
        PrevAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (LContainer.localEulerAngles.z == MaxAngle.z)
            return;

        if(PrevAngle - 5 > transform.localEulerAngles.z
            || PrevAngle + 5 < transform.localEulerAngles.z)
        {
            t += 0.02f;
            LContainer.localEulerAngles = Vector3.Lerp(-MinAngle,MaxAngle,t);
            RContainer.localEulerAngles = Vector3.Lerp(MinAngle,-MaxAngle,t);
            PrevAngle = transform.localEulerAngles.z;
        }
    }
}
