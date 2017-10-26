using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン切り替えの関数
/// 作成者：下鳥信也
/// 作成日時：2017/07/19
/// </summary>

public class SceneManage : SingletonMonoBehaviour<SceneManage>
{

    [Header("アニメーション関係------------------------------------------")]
    public Animator cutain_anim;

    // Use this for initialization
    void Start () {
		
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
    void Update () {
		
	}

    //スタート画面に移行
    public void Title_Load()
    {
        StartCoroutine(Scene_anim_ReCutain_Title());
        
    }

    //メイン画面に移行
    public void Main_Load()
    {
        StartCoroutine(Scene_anim_Main());
    }

    //リザルト画面に移行
    public void Result_Load()
    {
        StartCoroutine(Scene_anim_ReCutain_Result());
        
    }

    //ランキング画面に移行
    public void Ranking_Load()
    {
        SceneManager.LoadScene("Ranking_Scene");
    }

    IEnumerator Scene_anim_Main()
    {
        cutain_anim.SetBool("Cutain", false);
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("MainScene");

        yield return new WaitForSeconds(3.0f);
        cutain_anim.SetBool("Cutain", true);
    }

    IEnumerator Scene_anim_ReCutain_Result()
    {
        cutain_anim.SetBool("Cutain", false);
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("ResultScene");
        cutain_anim.SetBool("Cutain", true);
    }

    IEnumerator Scene_anim_ReCutain_Title()
    {
        cutain_anim.SetBool("Cutain", false);
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("TitleScene");
        cutain_anim.SetBool("Cutain", true);
    }

}
