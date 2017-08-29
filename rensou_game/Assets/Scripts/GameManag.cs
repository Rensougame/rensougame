using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// メイン画面での各種データを扱う
/// 作成者：下鳥信也
/// 作成日時：2017/07/20
/// </summary>

public class GameManag : MonoBehaviour {

    //問題数
    public int problem_num;

    //残り問題数
    public int remaining_problem_num;

    //ヒントを追加した数
    public int hint_additional_num;

    //スキップ数
    public int skip_num;

    //スキップした数
    public int skipped_num;

    //ヒントを出すText
    public Text[] hinttext;

    //excelを読み込んだデータ
    public Sheet1 m_Book;

    //問題のID
    int ID = 0;

    //出ているヒントの数
    int Hint_num = 3;

    //ヒント最大数
    int Hint_num_max = 5;

    //入力text
    public InputField inputField;

    // Use this for initialization
    void Start () {
        //最初はどちらも等しく
        remaining_problem_num = problem_num;

        startup_hint(1, Hint_num);
    }
	
	// Update is called once per frame
	void Update () {
    }

    //**********************************************
    //プライベート関数
    //**********************************************
    //問題のヒントを表示
    //引数1：指定された番号のヒントを出す　引数2：連続して出す場合指定する
    void startup_hint(int num, int num_con = 1)
    {
        for (int i = 0; i < num_con; i++, num++)
        {
            switch (num)
            {
                case 1:
                    hinttext[0].text = m_Book.param[ID].hint1;
                    break;
                case 2:
                    hinttext[1].text = m_Book.param[ID].hint2;
                    break;
                case 3:
                    hinttext[2].text = m_Book.param[ID].hint3;
                    break;
                case 4:
                    hinttext[3].text = m_Book.param[ID].hint4;
                    break;
                case 5:
                    hinttext[4].text = m_Book.param[ID].hint5;
                    break;
                default:
                    break;
            }
        }
    }

    //**********************************************
    //パブリック関数
    //**********************************************
    //残り問題数を減らす
    public void decrement_remaining_problem_num()
    {
        remaining_problem_num--;
    }

    //ヒントを追加(追加した数を数える)
    public void adding_hint()
    {
        if (Hint_num < Hint_num_max)
        {
            Hint_num++;
            startup_hint(Hint_num);
            hint_additional_num++;
        }
    }

    //スッキプした数を増やす
    public void increment_skipped_num()
    {
        skipped_num++;
    }

    //文字判定
    public void letter_decision()
    {
        if (m_Book.param[ID].answer == inputField.text)
        {
            Debug.Log("正解！");
        }
        else
        {
            Debug.Log("不正解！");
        }
    }
}
