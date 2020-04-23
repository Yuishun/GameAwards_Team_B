using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextScript : MonoBehaviour
{
    //　読み込んだテキストを出力するUIテキスト
    [SerializeField]
    private Text BookText;
    //　Resourcesフォルダから直接テキストを読み込む
    private string loadText;
    //　配列に入れる
    private string[] splitText;
    
    private bool TextEndFlag = false;
    

    [SerializeField][Range(0.01f,0.3f)]
    float interval = 0.05f;                         // 1文字の表示にかける時間
    private int currentSentenceNum = 0;             // 現在表示している文章番号
    private string currentSentence = string.Empty;  // 現在の文字列
    private float Displaytime = 0;                  // 表示にかかる時間
    private float DisplayStarttime = 3;             // 文字列の表示を開始した時間
    private int lastUpdateCharCount = -1;           // 表示中の文字数
    
    void Start()
    {
        loadText = (Resources.Load("Story", typeof(TextAsset)) as TextAsset).text;

        BookText.text = "";
        currentSentence = loadText;
        splitText = loadText.Split(char.Parse("▼"));
        
        StartCoroutine(StartText());
    }

    void Update()
    {
        if (!TextEndFlag)
        {
            if (AllSceneManager.m_bFadeInEnd)
            {
                if (Time.time > DisplayStarttime + Displaytime)
                {
                    if (currentSentenceNum < splitText.Length)
                        NextText();
                    else
                        TextEnd();
                }
                int nowCharCount = (int)(currentSentence.Length * Mathf.Clamp01((Time.time - DisplayStarttime) / Displaytime));

                if (lastUpdateCharCount != nowCharCount && !TextEndFlag)
                {
                    BookText.text = currentSentence.Substring(0, nowCharCount);
                    lastUpdateCharCount = nowCharCount;
                }
            }
        }
    }
    //=====================================================
    // 次の文字を用意
    //=====================================================
    void NextText()
    {
        currentSentence = splitText[currentSentenceNum];
        Displaytime = currentSentence.Length * interval;
        DisplayStarttime = Time.time;
        currentSentenceNum++;
        lastUpdateCharCount = 0;
    }
    //=====================================================
    // 文章終了時
    //=====================================================
    void TextEnd()
    {
        ProphecyScript.flag = true;
        TextEndFlag = true;
    }
    //=====================================================
    // 文章スキップ用
    //=====================================================
    public void SkipText()
    {
        TextEndFlag = true;
        StartCoroutine(SkipToText());
        BookText.text = currentSentence.Substring(0, currentSentence.Length-2);
    }
    IEnumerator SkipToText()
    {
        yield return new WaitForSeconds(1.0f);
        ProphecyScript.flag = true;
    }
    IEnumerator StartText()
    {
        while (!AllSceneManager.m_bFadeInEnd)
            yield return new WaitForEndOfFrame();
        NextText();
    }
}