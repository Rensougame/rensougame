using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{

    public Text time, hint, Totalscore;

    public Button retryButton, titleButton;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(TextRead());
    }

    // Update is called once per frame
    void Update()
    {

    }

    ///////////////////////////////////////////////////
    //コルーチン関数                                 //
    ///////////////////////////////////////////////////
    //カウントダウン
    IEnumerator TextRead()
    {
        yield return new WaitForSeconds(0.6f);

        time.text = GameManag.Instance.second.ToString("f0");
        yield return new WaitForSeconds(3.0f);

        hint.text = GameManag.Instance.hint_additional_num.ToString();
        yield return new WaitForSeconds(3.0f);

        Totalscore.text = GameManag.Instance.Score.ToString();
        yield return new WaitForSeconds(3.0f);

        retryButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(true);
    }

    //////////////////////////////
    //ボタン専用関数            //
    //////////////////////////////
    public void Retry()
    {
        Destroy(GameManag.Instance.gameObject);
        SceneManage.Instance.Main_Load();
    }

    public void title()
    {
        Destroy(GameManag.Instance.gameObject);
        SceneManage.Instance.Title_Load();
    }
}