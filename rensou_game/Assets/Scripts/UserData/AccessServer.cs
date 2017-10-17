using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Userdata_space;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;



public class AccessServer : MonoBehaviour {

    private Userdata data;      //ユーザ情報保持用クラス

    public InputField m_if_ID, m_if_pass;

    public List<Text> text = new List<Text>();

    void Start () {
        //StartCoroutine(Upload());
    }

    IEnumerator Login(string ID, string pass)
    {
        //ユーザ認証処理
        WWWForm form = new WWWForm();

        //formにパラメータをセット
        form.AddField("client_id", "game");
        form.AddField("client_secret", "game");
        form.AddField("username", ID);
        form.AddField("password", calcMd5(pass));
        form.AddField("grant_type", "password");

        //サーバにアクセス
        using (UnityWebRequest www = UnityWebRequest.Post("http://testaccount.4353p-club.com/oauth2/token", form))
        {
            yield return www.Send();

            if (www.isError)
            {
                //エラー
                Debug.Log(www.error);
            }
            else
            {
               //成功　結果を受け取る
               string str = www.downloadHandler.text;
               Debug.Log(www.responseCode.ToString() + ":" + str);

               // 取得したデータをクラスにセット
               data = JsonUtility.FromJson<Userdata>(str);

               Debug.Log(data.access_token); 
            }

            var user_request = new UnityWebRequest("http://testgame.4353p-club.com/user/info", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data.SaveToString());
            user_request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            user_request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            user_request.SetRequestHeader("Content-Type", "application/json");

            yield return user_request.Send();

            if (user_request.isError)
            {
                //エラー
                Debug.Log(user_request.error);
            }
            else
            {
                string str_user = user_request.downloadHandler.text;

                // 取得したデータをクラスにセット
                data = JsonUtility.FromJson<Userdata>(str_user);

                Debug.Log("Status Code: " + user_request.responseCode);

                ToTextWrite();
            }



            ////当該ゲーム情報取得
            //GameData gamedata_request = new GameData
            //{
            //    game_id = 11,    //ゲームID
            //};
            //data.data = gamedata_request;
            //var request = new UnityWebRequest("http://testgame.4353p-club.com/info/user", "POST");
            //byte[] bodyRaw = Encoding.UTF8.GetBytes(data.SaveToString());
            //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //request.SetRequestHeader("Content-Type", "application/json");

            //yield return request.Send();

            //if(request.isError)
            //{
            //    //エラー
            //    Debug.Log(request.error);
            //}
            //else
            //{
            //    string str_2 = request.downloadHandler.text;
            //    data = JsonUtility.FromJson<Userdata>(str_2);

            //    if (data.status == 0)
            //    {
            //        //当該ゲーム情報登録
            //        GameData gamedata_create = new GameData
            //        {
            //            game_id = 11,    //ゲームID
            //            point = 100,    //ポイント（スコア）
            //        };

            //        GameInfo gameinfo_create = new GameInfo
            //        {
            //            value1 = "GET",    //固有値1
            //            value2 = "",    //固有値2
            //            value3 = "",    //固有値3
            //        };
            //        data.data = gamedata_create;
            //        data.game_info = gameinfo_create;
            //        var create = new UnityWebRequest("http://testgame.4353p-club.com/info/user/create", "POST");
            //        byte[] bodyRaw_create = Encoding.UTF8.GetBytes(data.SaveToString());
            //        create.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw_create);
            //        create.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //        create.SetRequestHeader("Content-Type", "application/json");

            //        yield return create.Send();

            //        //デバック
            //        if (create.isError)
            //        {
            //            //エラー
            //            Debug.Log(create.error);
            //        }
            //        else
            //        {
            //            //成功　結果を受け取る
            //            string str = create.downloadHandler.text;
            //            Debug.Log(create.responseCode.ToString() + ":" + str);
            //        }
            //    }

            //    Debug.Log("Status Code: " + request.responseCode);
                

            //    // 取得したデータをクラスにセット
            //    data = JsonUtility.FromJson<Userdata>(str_2);
            //    Debug.Log(data.game_info.value1);
            //    Debug.Log(data.data.point);
            //    Debug.Log(request.responseCode.ToString() + ":" + str_2);
            //}

            


            //ゲーム情報取得処理ここまで
        }

        ////当該ゲーム情報登録
        //GameData gamedata_create = new GameData
        //{
        //    game_id = 1,    //ゲームID
        //    value1 = "GET",    //固有値1
        //    value2 = "",    //固有値2
        //    value3 = "",    //固有値3
        //    point = 0,    //ポイント（スコア）
        //};
        //data.data = gamedata_create;
        //var create = new UnityWebRequest("http://testgame.4353p-club.com/info/user/create", "POST");
        //byte[] bodyRaw_create = Encoding.UTF8.GetBytes(data.SaveToString());
        //create.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw_create);
        //create.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //create.SetRequestHeader("Content-Type", "application/json");

        //yield return create.Send();

        ////デバック
        //if (create.isError)
        //{
        //    //エラー
        //    Debug.Log(create.error);
        //}
        //else
        //{
        //    //成功　結果を受け取る
        //    string str = create.downloadHandler.text;
        //    Debug.Log(create.responseCode.ToString() + ":" + str);
        //}

        ////スコア更新処理
        //GameData gamedata_score_update = new GameData
        //{
        //    point = 100,    //ポイント（スコア）
        //};
        //data.data = gamedata_score_update;
        //UnityWebRequest request_score_update = new UnityWebRequest("http://testgame.4353p-club.com/info/user/update", "POST");
        //byte[] bodyRaw_score_update = Encoding.UTF8.GetBytes(data.SaveToString());
        //request_score_update.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw_score_update);
        //request_score_update.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request_score_update.SetRequestHeader("Content-Type", "application/json");

        //yield return request_score_update.Send();

        ////デバック
        //if (request_score_update.isError)
        //{
        //    //エラー
        //    Debug.Log(request_score_update.error);
        //}
        //else
        //{
        //    //成功　結果を受け取る
        //    string str = request_score_update.downloadHandler.text;
        //    Debug.Log(request_score_update.responseCode.ToString() + ":" + str);
        //}
        ////スコア更新処理ここまで

        ////当該ゲーム情報取得
        //GameData gamedata_request = new GameData
        //{
        //    game_id = 1,    //ゲームID
        //};
        //data.data = gamedata_request;
        //var request = new UnityWebRequest("http://testgame.4353p-club.com/info/user", "POST");
        //byte[] bodyRaw = Encoding.UTF8.GetBytes(data.SaveToString());
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        //request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();

        ////Debug.Log("Status Code: " + request.responseCode);
        //string str_2 = request.downloadHandler.text;
        //Debug.Log(str_2);

        //// 取得したデータをクラスにセット
        //data2 = JsonUtility.FromJson<GameInfo>(str_2);
        //Debug.Log(data2.value1);
        //Debug.Log(request.responseCode.ToString() + ":" + str_2);


        ////ゲーム情報取得処理ここまで
    }

    IEnumerator Ranking()
    {
        if (data != null)
        {
            var ranking = new UnityWebRequest("http://testgame.4353p-club.com/ranking", "POST");
            byte[] bodyRaw_create = Encoding.UTF8.GetBytes(data.SaveToString());
            ranking.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw_create);
            ranking.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            ranking.SetRequestHeader("Content-Type", "application/json");

            yield return ranking.Send();

            //デバック
            if (ranking.isError)
            {
                //エラー
                Debug.Log(ranking.error);
            }
            else
            {
                //成功　結果を受け取る
                string str = ranking.downloadHandler.text;
                Debug.Log(ranking.responseCode.ToString() + ":" + str);
                //data = JsonUtility.FromJson<User>(str);
                Debug.Log(data);
            }
        }
    }

    //確定したらログイン処理
    public void EnterPush()
    {
        StartCoroutine(Login(m_if_ID.text, m_if_pass.text));
    }

    /// <summary>
    /// ランキングボタンが押されたらサーバー通信でランキングを取得
    /// </summary>
    public void ButtonPush()
    {
        StartCoroutine(Ranking());
    }
    /// <summary>
    /// ユーザ情報をtextに書き込む
    /// </summary>
    void ToTextWrite()
    {
        text[0].text = data.user.id.ToString();
        text[1].text = data.user.name.ToString();
        text[2].text = data.user.room.name.ToString();
    }

    //--------------------------------------------------------------------
    /// <summary>  指定された文字列をMD5でハッシュ化し、文字列として返す
    /// </summary>
    /// <param name="srcStr">入力文字列</param>
    /// <returns>入力文字列のMD5ハッシュ値</returns>
    //--------------------------------------------------------------------
    private string calcMd5(string srcStr)
    {

        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        // md5ハッシュ値を求める
        byte[] srcBytes = System.Text.Encoding.UTF8.GetBytes(srcStr);
        byte[] destBytes = md5.ComputeHash(srcBytes);

        // 求めたmd5値を文字列に変換する
        System.Text.StringBuilder destStrBuilder;
        destStrBuilder = new System.Text.StringBuilder();
        foreach (byte curByte in destBytes)
        {
            destStrBuilder.Append(curByte.ToString("x2"));
        }

        // 変換後の文字列を返す
        return destStrBuilder.ToString();
    }
}