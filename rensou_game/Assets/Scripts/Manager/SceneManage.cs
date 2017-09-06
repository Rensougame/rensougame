using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン切り替えの関数
/// 作成者：下鳥信也
/// 作成日時：2017/07/19
/// </summary>

public class SceneManage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //スタート画面に移行
    public void Start_Load()
    {
        SceneManager.LoadScene("Start_Scene");
    }

    //メイン画面に移行
    public void Main_Load()
    {
        SceneManager.LoadScene("Main_Scene");
    }

    //リザルト画面に移行
    public void Result_Load()
    {
        SceneManager.LoadScene("Result_Scene");
    }

    //ランキング画面に移行
    public void Ranking_Load()
    {
        SceneManager.LoadScene("Ranking_Scene");
    }
}
