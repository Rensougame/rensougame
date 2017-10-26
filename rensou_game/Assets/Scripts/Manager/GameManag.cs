using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// メイン画面での各種データを扱う
/// 作成者：下鳥信也
/// 作成日時：2017/07/20
/// </summary>

public class GameManag : SingletonMonoBehaviour<GameManag>
{
    public int Score;

    [Header("問題数関係------------------------------------------")]
    //現在の問題数
    public int problem_num = 1;
    //現在の残り問題
    [System.NonSerialized]
    public int problem_num_remainder;
    //最大問題数
    public int problem_num_max = 10;
    //正解した問題数
    public int passed_problem_num;
    //ヒントを追加した全体の数
    public int hint_additional_num;
    //ヒントを追加した数
    public int hint_add_num;
    //残りスキップ数
    public int skip_num;
    //問題のID
    public int ID = 0;

    [Header("ヒント関係------------------------------------------")]

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

    [Header("背景・正解・不正解関係------------------------------------------")]

    //正解Image
    public Image correctImage;
    //不正解Image
    public Image incorrectImage;
    //終了Image
    public GameObject endImage;
    //背景Image
    public Image backImage;
    //確認Image
    public GameObject confirmation;
    //Image表示時間
    public float activeImage;

    [Header("excel・入力関係------------------------------------------")]

    //excelを読み込んだデータ
    public Entity_Sheet1 m_QA;
    //入力text
    public InputField m_inputField;

    [Header("UIテキスト関係------------------------------------------")]

    //Text変更
    public Text_Change m_skiptext_change;
    public Text_Change m_problemtext_change;
    public Text skip_text;
    public Text count_text;

    [Header("パーティクル関係------------------------------------------")]

    //ランダム重複検査用
    bool[] rand_duplication;

    //パーティクル(紙吹雪)
    public ParticleSystem m_confetti;

    [Header("サウンド関係------------------------------------------")]

    //サウンド関係
    public AudioClip m_correct;
    public AudioClip m_wrong;
    AudioSource Source;

    [Header("アニメーション関係------------------------------------------")]

    public Animator confirmation_anim;

    [Header("経過時間------------------------------------------")]
    //時間測定用
    public float second;

    //タイマーのOn・Off
    bool Timeflg;

    //xlsx列t調整
    int adjustment = 1;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

        rand_duplication = new bool[m_QA.param.Count];
        ID_random();
        m_inputField.ActivateInputField();

        correctImage.enabled = false;
        incorrectImage.enabled = false;
        endImage.SetActive(false);
        backImage.enabled = false;

        problem_num_remainder = problem_num_max - problem_num;
        Source = GetComponent<AudioSource>();

        StartCoroutine(CountdownCoroutine());

    }

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        emter_push();
        Timer(Timeflg);
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
                    m_hinttext[0].text = m_QA.param[ID].hint1;
                    m_hintimage[0].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_QA.param[ID].image1));
                    break;
                case 2:
                    m_hinttext[1].text = m_QA.param[ID].hint2;
                    m_hintimage[1].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_QA.param[ID].image2));
                    break;
                case 3:
                    m_hinttext[2].text = m_QA.param[ID].hint3;
                    m_hintimage[2].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_QA.param[ID].image3));
                    break;
                case 4:
                    m_hinttext[3].text = m_QA.param[ID].hint4;
                    m_hintimage[3].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_QA.param[ID].image4));
                    break;
                case 5:
                    m_hinttext[4].text = m_QA.param[ID].hint5;
                    m_hintimage[4].sprite = Resources.Load<Sprite>(string.Concat(ROOT, m_QA.param[ID].image5));
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            letter_decision();
        }
    }

    //問題のランダム
    void ID_random()
    {
        ID = Random.Range(0, m_QA.param.Count - adjustment);
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
        //正解・不正解の非表示
        correctImage.enabled = false;
        incorrectImage.enabled = false;
        backImage.enabled = false;

        //パーティクル停止
        m_confetti.Clear();
        m_confetti.Stop();

        problem_num++;      //現在の問題数カウントアップ
        problem_num_remainder = problem_num_max - problem_num;      //残り出題数
        ID_random();        //出題する問題をランダムに
        Hint_num = Hint_num_first;      //ヒント数リセット
        hint_add_num = 0;

        hint_clear();
        m_inputField.text = "";

        if (problem_num >= m_QA.param.Count)
        {
            skip_text.text = "リタイア";
        }

        startup_hint(1, Hint_num);
        m_skiptext_change.text_change();
        m_problemtext_change.text_change();

        m_inputField.ActivateInputField();

        Timeflg = true;
    }

    //全問終了時の処理
    void game_end()
    {
        correctImage.enabled = false;
        incorrectImage.enabled = false;

        m_confetti.Clear();
        m_confetti.Stop();

        confirmation.SetActive(false);

        backImage.enabled = true;
        endImage.SetActive(true);

        SceneManage.Instance.Result_Load();
    }

    //リタイア時の処理
    void give_up_confirmation()
    {
        backImage.enabled = true;
        confirmation.SetActive(true);

        confirmation_anim.Play("confirmationAnimation", 0, 0.0f);
    }

    //タイマー処理
    void Timer(bool Timeflag)
    {
        if(Timeflag)
        {
            second += Time.deltaTime;
        }
    }

    //ヒントの数でスコア変動処理
    void Hint_add_score(int hints)
    {
        switch(hints)
        {
            case 0:
                Score += 50;
                break;
            case 1:
                Score += 40;
                break;
            case 2:
                Score += 30;
                break;
            case 3:
                Score += 10;
                break;
        }
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
            hint_add_num++;
        }
        m_inputField.ActivateInputField();
    }

    //スキップ機能(次の問題へ移行)
    public void skip_feature()
    {
        int skip_change = 0;
        if (skip_num > 0 && problem_num < m_QA.param.Count)
        {
            skip_num--;
            if (skip_num == skip_change) skip_text.text = "リタイア";

            clear();
        }
        else
        {
            Timeflg = false;
            give_up_confirmation();
        }
    }

    //文字判定
    public void letter_decision()
    {
        if (m_QA.param[ID].answer1 == m_inputField.text)
        {
            Timeflg = false;

            correctImage.enabled = true;
            backImage.enabled = true;
            passed_problem_num++;

            m_confetti.Play();
            Source.clip = m_correct;
            Source.Play();

            Hint_add_score(hint_add_num);

            if (problem_num >= m_QA.param.Count)
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
            Timeflg = false;

            incorrectImage.enabled = true;
            backImage.enabled = true;

            Source.clip = m_wrong;
            Source.Play();

            if (problem_num >= m_QA.param.Count)
            {
                Invoke("game_end", activeImage);
            }
            else
            {
                Invoke("clear", activeImage);
            }
        }
    }
    //////////////////////////////
    //ボタン専用関数            //
    //////////////////////////////
    public void confirmation_yes()
    {
        game_end();
    }

    public void confirmation_no()
    {
        backImage.enabled = false;
        confirmation.SetActive(false);
        Timeflg = true;
    }

    ///////////////////////////////////////////////////
    //コルーチン関数                                 //
    ///////////////////////////////////////////////////
    //カウントダウン
    IEnumerator CountdownCoroutine()
    {
        count_text.gameObject.SetActive(true);
        startup_hint(1, Hint_num);

        count_text.text = "3";
        yield return new WaitForSeconds(1.0f);

        count_text.text = "2";
        yield return new WaitForSeconds(1.0f);

        count_text.text = "1";
        yield return new WaitForSeconds(1.0f);

        count_text.text = "スタート!";

        count_text.text = "";
        count_text.gameObject.SetActive(false);
        
        Timeflg = true;
    }
}
