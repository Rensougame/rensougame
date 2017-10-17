//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

using UnityEngine;

namespace Userdata_space
{
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

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [System.Serializable]
    public class User
    {
        public int id;
        public string name;
        public Room room;
        public int icon_type;
    }

    [System.Serializable]
    public class Room
    {
        public int id;
        public string name;
        public int fc_id;
        public int room_id;
    }

    [System.Serializable]
    public class GameInfo
    {
        public int menber_id;
        public string value1;
        public string value2;
        public string value3;
    }

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
