using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScript : MonoBehaviour
{
    [SerializeField]
    public AllSceneManager allScene;
    int[,] ClearStageNum;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("AllScene"))
        {
            allScene = GameObject.FindGameObjectWithTag("AllScene").transform.GetComponent<AllSceneManager>();
            ClearStageNum = allScene.GetClearData();
            ClearStageCheck();
        }
    }
    //===================================================
    // ステージクリア状態チェック
    //===================================================
    void ClearStageCheck()
    {
        int num = 0;
        while (num < ClearStageNum.Length / 2)
        {
            switch (ClearStageNum[num, 1])
            {
                case 0:
                    break;
                case 1:
                    StartCoroutine(StageClear(num));
                    break;
            }
            num++;
        }
    }
    //===================================================
    // ステージ選択用
    //===================================================
    public void StageSelect(int Stagenam)
    {
        if (allScene)
        {
            allScene.loadstagenum(Stagenam);
        }
    }
    //===================================================
    // ステージクリア直後処理
    //===================================================
    IEnumerator OneStageClear()
    {
        yield return new WaitForEndOfFrame();
    }
    //===================================================
    // クリアステージ処理
    //===================================================
    IEnumerator StageClear(int val)
    {
        yield return new WaitForEndOfFrame();
    }
}
