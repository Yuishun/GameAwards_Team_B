using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// コントローラーの入力テストclass
public class XboxInputTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        // L Stick
        float lsh = Input.GetAxis("L_Horizontal");
        float lsv = Input.GetAxis("L_Vertical");
        if ((lsh != 0) || (lsv != 0))
        {
            Debug.Log("L stick:" + lsh + "," + lsv);
        }

        // R Stick
        float rsh = Input.GetAxis("R_Horizontal");
        float rsv = Input.GetAxis("R_Vertical");
        if ((rsh != 0) || (rsv != 0))
        {
            Debug.Log("R stick:" + rsh + "," + rsv);
        }

        // D-Pad (十字キー)
        float dph = Input.GetAxis("DPad_Horizontal");
        float dpv = Input.GetAxis("DPad_Vertical");
        if ((dph != 0) || (dpv != 0))
        {
            Debug.Log("D Pad:" + dph + "," + dpv);
        }

        //Trigger
        float tri = Input.GetAxis("LR_Trigger");
        if (tri < 0)
        {
            Debug.Log("L trigger:" + tri);
        }
        else if (tri > 0)
        {
            Debug.Log("R trigger:" + tri);
        }


        // A Button
        if (Input.GetButtonDown("Button_A"))
        {
            Debug.Log("A Button Down");
        }

        // B Button
        if (Input.GetButtonDown("Button_B"))
        {
            Debug.Log("B Button Down");
        }

        // X Button
        if (Input.GetButtonDown("Button_X"))
        {
            Debug.Log("X Button Down");
        }

        // Y Button
        if (Input.GetButtonDown("Button_Y"))
        {
            Debug.Log("Y Button Down");
        }

        // L Button
        if (Input.GetButtonDown("Button_L"))
        {
            Debug.Log("L Button Down");
        }

        // R Button
        if (Input.GetButtonDown("Button_R"))
        {
            Debug.Log("R Button Down");
        }

        // Back Button
        if (Input.GetButtonDown("Button_BACK"))
        {
            Debug.Log("BACK Button Down");
        }

        // Start Button
        if (Input.GetButtonDown("Button_START"))
        {
            Debug.Log("START Button Down");
        }

        // L3 Button
        if (Input.GetButtonDown("Button_L3"))
        {
            Debug.Log("L3 Button Down");
        }

        // R3 Button
        if (Input.GetButtonDown("Button_R3"))
        {
            Debug.Log("R3 Button Down");
        }

    }
}
