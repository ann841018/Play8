using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facebook.Unity
{
    public class Facebook_Control : MonoBehaviour
    {
        //[SerializeField] Text text;//顯示回傳值
        //[SerializeField] Image Player_Pic;

        public static string Play8ID;//Play8ID    
        public static string PlayerID;//GameID
        public static string BusinessToken;//TokenID

        string PlayerName;//姓名
        string PlayerPic;//頭貼

        string Status;//LogIn||Bind
        string ReturnInfo;//回傳內容
               
        void Start() {FB.Init(); }// Use this for initialization       

        void Update() { }// Update is called once per frame

        public class json
        {
            public string name;
            public string token_for_business;
            public string id;
        }

        public void LogIn()
        {
            FB.LogInWithReadPermissions(new List<string>(){ "public_profile", "email" }, Result);
            Status = "FacebookLogIn";
        }
        public void Bind()
        {
            FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, Result);
            Status = "FacebookBind";
        }
        public void LogOut() { FB.LogOut(); }

        protected void Result(IResult result)
        {
            if (result == null)
            {
                //text.text = "Null Response\n";
                return;
            }
            // Some platforms return the empty string instead of null.
            if (!string.IsNullOrEmpty(result.Error))
            {
                //text.text = "Error Response:\n" + result.Error;
            }
            else if (result.Cancelled)
            {
                //text.text = "Cancelled Response:\n" + result.RawResult;
            }
            else if (!string.IsNullOrEmpty(result.RawResult))
            {
                //text.text = "Success Response:\n" + result.RawResult;
                FB.API("/me?fields=name,token_for_business", HttpMethod.GET, PlayerInfo, new Dictionary<string, string>() { });
            }
            else
            {
                //text.text = "Empty Response\n";
            }
        }

        protected void PlayerInfo(IResult result)
        {
            json InfoJson = new json();
            InfoJson = JsonUtility.FromJson<json>(result.RawResult.ToString());
            PlayerID = InfoJson.id;
            PlayerName = InfoJson.name;
            PlayerPic = "http://graph.facebook.com/" + InfoJson.id + "/picture?type=large";
            BusinessToken = InfoJson.token_for_business;
            ReturnInfo = InfoJson.id + "\n" + InfoJson.name + "\n" + InfoJson.token_for_business;
            FB.API("me/ids_for_business", HttpMethod.GET, IDForBusiness, new Dictionary<string, string>());
            //StartCoroutine("LoadPicture", PlayerPic);           
        }

        protected void IDForBusiness(IResult result)//ids_for_business
        {
            string Businessdata = result.RawResult.ToString().Remove(0,9);
            string[] data = Businessdata.Split(']');
            print(data[0]);
            if (data[0].IndexOf("play8") >= 0)
            {
                int Play8Num = data[0].IndexOf("member.play8.com.tw") - 96;
                Businessdata = data[0].Remove(0, Play8Num);
                data = Businessdata.Split('{');
                data = data[1].Split(':');
                Play8ID = data[1].Trim('\"');
                data = Play8ID.Split('\"');
                Play8ID = data[0];
                ReturnInfo = ReturnInfo + "\n" + Play8ID;
                //text.text = ReturnInfo;                
            }
            if (Status == "FacebookLogIn") Player8_System.FacebookLogIn = true;
            else if (Status == "FacebookBind") Player8_System.FacebookBind = true;
        }

        /*IEnumerator LoadPicture(string URL)//頭貼
        {
            WWW www = new WWW(URL);
            yield return www;
            Player_Pic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
        }*/
    }
}