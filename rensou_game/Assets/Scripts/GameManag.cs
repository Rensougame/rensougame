using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start () {
        //最初はどちらも等しく
        remaining_problem_num = problem_num;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //**********************************************
    //パブリック関数
    //**********************************************

    //残り問題数を減らす
    public void decrement_remaining_problem_num()
    {
        remaining_problem_num--;
    }

    //ヒントを追加した数を増やす
    public void increment_hint_additional_num()
    {
        hint_additional_num++;
    }

    //スッキプした数を増やす
    public void increment_skipped_num()
    {
        skipped_num++;
    }
}
