using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_SceneManege : MonoBehaviour {

    //繰り返しDontDestroyOnLoadのSceneManagerを取得するためのクッション
    public void ButtonPush () {
        SceneManage.Instance.Main_Load();
    }
}
