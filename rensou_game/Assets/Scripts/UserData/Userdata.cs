//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

using UnityEngine;

namespace Userdata_space
{
    //ゲーム内ユーザーデータ登録・更新・ランキング取得用
    [System.Serializable]
    public class GameData
    {
        public int game_id;
        public string value1;
        public string value2;
        public string value3;
        public int point;
        public int type;
        public int limit;
    }

    //ログイン・ユーザー情報取得用
    [System.Serializable]
    public class Userdata
    {
        public string username;
        public string password;
        public string access_token;
        public string token_type;
        public string expires_in;
        // public string refresh_token;

        public GameData data;
        public User user;
        public GameInfo game_info;

        public int status;

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
    //ユーザー情報
    [System.Serializable]
    public class User
    {
        public int id;
        public string name;
        public Room room;
        public int icon_type;
    }

    //ユーザーの部屋情報
    [System.Serializable]
    public class Room
    {
        public int id;
        public string name;
        public int fc_id;
        public int room_id;
    }

    //ゲーム側が持っているユーザー情報
    [System.Serializable]
    public class GameInfo
    {
        public int game_id;
        public int id;
        public string value1;
        public string value2;
        public string value3;
        public int point;
    }

    //ランキング情報保持用
    [System.Serializable]
    public class Ranking
    {
        public int game_id;
        public int id;
        public string value1;
        public string value2;
        public string value3;
        public int point;
    }

}
