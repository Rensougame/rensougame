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

    //現在の問題数
    public int problem_num = 1;
    //最大問題数
    public int problem_num_max = 10;
    //正解した問題数
    public int passed_problem_num;
    //ヒントを追加した数
    public int hint_additional_num;
    //残りスキップ数
    public int skip_num;
    //問題のID
    int ID = 0;


    //ヒントを出すText
    public Text[] m_hinttext;
    //ヒントのImage
    public Image[] m_hintimage;
    //出ているヒントの数
    public int Hint_num = 2;
    //最初に出ているヒントの数
    public int Hint_num_first = 2;
    //ヒント最大数
    int Hint_num_max = 5;
    //ヒントimageのパス
    const string ROOT = "Textures/";


    //正解Image
    public Image correctImage;
    //不正解Image
    public Image incorrectImage;
    //終了Image
    public Image endImage;
    //背景Image
    public Image backImage;
    //Image表示時間
    public float activeImage;

    //excelを読み込んだデータ
    public Sheet1 m_Book;
    //入力text
    public InputField m_inputField;


    //Text変更
    public Text_Change m_skiptext_change;
    public Text_Change m_problemtext_change;
    public Text skip_text;


    //ランダム重複検査用
    bool[] rand_duplication;

    //パーティクル(紙吹雪)
    public ParticleSystem m_confetti;


    // Use this for initialization
    void Start () {
        rand_duplication = new bool[m_Book.param.Count];
        ID_random();
        m_inputField.ActivateInputField();
        startup_hint(1, Hint_num);
        correctImage.enabled = false;
        incorrectImage.enabled = false;
        endImage.enabled = false;
        backImage.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        emter_push();
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
                    m_hinttext[0].text = m_Book.param[ID].hint1;
                    m_hintimage[0].sprite = Resources.Load<Sprite>( string.Concat(ROOT, m_Book.param[ID].image1) );
                    break;
                case 2:
                    m_hinttext[1].text = m_Book.param[ID].hint2;
                    m_hintimage[1].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_Book.param[ID].image2));
                    break;
                case 3:
                    m_hinttext[2].text = m_Book.param[ID].hint3;
                    m_hintimage[2].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_Book.param[ID].image3));
                    break;
                case 4:
                    m_hinttext[3].text = m_Book.param[ID].hint4;
                    m_hintimage[3].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_Book.param[ID].image4));
                    break;
                case 5:
                    m_hinttext[4].text = m_Book.param[ID].hint5;
                    m_hintimage[4].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_Book.param[ID].image5));
                    break;
                default:
                    break;
            }
        }
    }

    //ヒントの初期化
    void hint_clear()
    {
        for (int j = 0; j < Hint_num_max; j++)
        {
            m_hinttext[j].text = "";
            m_hintimage[j].sprite = null;
        }
    }

    //エンターキーを押した場合、文字判定をする
    void emter_push()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            letter_decision();
        }
    }

    //問題のランダム
    void ID_random()
    {
        ID = Random.Range(0, m_Book.param.Count);
        if (rand_duplication[ID] == true)
        {
            ID_random();
        }
        else
        {
            rand_duplication[ID] = true;
        }
    }

    //正解・不正解後の処理
    void clear()
    {
        correctImage.enabled = false;
        incorrectImage.enabled = false;
        backImage.enabled = false;

        m_confetti.Clear();
        m_confetti.Stop();

        problem_num++;
        ID_random();
        Hint_num = Hint_num_first;

        hint_clear();
        m_inputField.text = "";

        if (problem_num >= m_Book.param.Count)
        {
            skip_text.text = "リタイア";
        }

        startup_hint(1, Hint_num);
        m_skiptext_change.text_change();
        m_problemtext_change.text_change();

        m_inputField.ActivateInputField();
    }

    //全問終了及びリタイア時の処理
    void game_end()
    {
        correctImage.enabled = false;
        incorrectImage.enabled = false;

        m_confetti.Clear();
        m_confetti.Stop();

        backImage.enabled = true;
        endImage.enabled = true;
    }

    //**********************************************
    //パブリック関数
    //**********************************************
    //ヒントを追加(追加した数を数える)
    public void adding_hint()
    {
        if (Hint_num < Hint_num_max)
        {
            Hint_num++;
            startup_hint(Hint_num);
            hint_additional_num++;
        }
        m_inputField.ActivateInputField();
    }

    //スキップ機能(次の問題へ移行)
    public void skip_feature()
    {
        int skip_change = 0;
        if (skip_num > 0 && problem_num < m_Book.param.Count)
        {
            skip_num--;
            if (skip_num == skip_change) skip_text.text = "リタイア";
     
            clear();
        }
        else
        {
            game_end();
        }
    }

    //文字判定
    public void letter_decision()
    {
        if (m_Book.param[ID].answer1 == m_inputField.text || m_Book.param[ID].answer2 == m_inputField.text)
        {
            correctImage.enabled = true;
            backImage.enabled = true;
            passed_problem_num++;

            m_confetti.Play();

            if (problem_num >= m_Book.param.Count)
            {
                Invoke("game_end", activeImage);
            }
            else
            {
                Invoke("clear", activeImage);
            }
        }
        else
        {
            incorrectImage.enabled = true;
            backImage.enabled = true;

            if (problem_num >= m_Book.param.Count)
            {
                Invoke("game_end", activeImage);
            }
            else
            {
                Invoke("clear", activeImage);
            }
        }
    }
}
