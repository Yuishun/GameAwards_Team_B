using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearText : MonoBehaviour
{
    [SerializeField] List<string> messageList = new List<string>();//会話文リスト
    [SerializeField] float novelSpeed;//一文字一文字の表示する速さ
    int novelListIndex = 0; //現在表示中の会話文の配列
    Text textArea;
    public GameObject TextTouch; //文を出し切ったら表示する
    bool isSkipFlag = false; //スキップ可能かどうか
    int messageCount = 0; //現在表示の文字数

    bool lastText = false;
    [SerializeField]
    GameObject endobject;

    // Start is called before the first frame update
    void Start()
    {
        this.textArea = GetComponent<Text>();
        StartCoroutine(Novel());
    }

    // Update is called once per frame
    void Update()
    {
        if (lastText)
        {
            if (Input.GetButtonDown("Button_A"))
            {
                this.textArea.text = "";
                TextTouch.SetActive(false);
                endobject.SetActive(true);
            }
        }
        if (Input.GetButtonDown("Button_A") && isSkipFlag)
        {
            this.textArea.text = messageList[novelListIndex];
            messageCount = messageList[novelListIndex].Length;
        }
    }

    private IEnumerator Novel()
    {
        this.textArea.text = ""; //テキストのリセット

        while (messageList[novelListIndex].Length > messageCount)//文字をすべて表示していない場合ループ
        {
            isSkipFlag = true;

            this.textArea.text += messageList[novelListIndex][messageCount];//一文字追加
            messageCount++;//現在の文字数
            yield return new WaitForSeconds(novelSpeed);//任意の時間待つ
        }

        novelListIndex++; //次の会話文配列
        isSkipFlag = false;
        TextTouch.SetActive(true);
        if (novelListIndex >= messageList.Count)
        {
            lastText = true;            
            yield break;
        }
        yield return new WaitUntil(Touch);
        TextTouch.SetActive(false);
        messageCount = 0;
        if (novelListIndex < messageList.Count)//全ての会話を表示していない場合
        {
            StartCoroutine(Novel());
        }
    }
    bool Touch()
    {
        return Input.GetButtonDown("Button_A");
    }
}