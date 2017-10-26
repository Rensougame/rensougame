using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI表示を変更
/// 作成者：下鳥信也
/// 作成日時：2017/09/04
/// </summary>

public class Text_Change : MonoBehaviour
{
    //マネージャーを取得
    public GameManag m_gamemanag;
    //Textの変更用変数
    Text m_text;

    public bool IsSkip; 

    // Use this for initialization
    void Start()
    {
        m_text = GetComponent<Text>();
        text_change();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //**********************************************
    //パブリック関数
    //**********************************************
    //スキップ回数、問題数の表示を更新
    public void text_change()
    {
        if (IsSkip)
        {
            m_text.text = m_gamemanag.skip_num.ToString();
        }
        else
        {
            m_text.text = m_gamemanag.problem_num_remainder.ToString();
        }
    }
}
