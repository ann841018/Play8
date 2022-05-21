using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Facebook.Unity
{
    public class Player8_System : MonoBehaviour
    {
        #region//宣告
        Text PlayerAccount;
        Text PlayerPassword;
        Text CheckPassword;

        [SerializeField] Text Wrong_Text;
        [SerializeField] GameObject Wrong_Panel;
        [SerializeField] GameObject Check;
        [SerializeField] Toggle AgreeToggle;

        [SerializeField] string GetGameID;//wls
        [SerializeField] string GameAppID;//564281734011348

        public static bool FacebookLogIn;
        public static bool FacebookBind;

        string GetKey;//CCeYZ7M2jB4w3rGPzypKcJe3XnkRtEgT//Kate產出的
        string URL;//https://member.play8.com.tw/SDK_Test/api.php

        string Play8ID;
        string Play8AppID = "286128994796219";
        string GameID;
        string TokenID;
        [SerializeField] string UID;

        string APIMethod;//狀態
        string Account;//帳號
        string pwd;//密碼
        string hash;//檢核碼
        int time;//時間
        int status;
        bool CanCreate;

        public static bool RememberPW = true;
        public static bool AgreeRule = false;

        #endregion

        void Start() { PlayerData playerData = new PlayerData(); if (PlayerPrefs.GetInt("Rule") == 1) AgreeToggle.isOn = true; }// Use this for initialization  
        void Update() {
            if (AgreeRule == true) PlayerPrefs.SetInt("Rule", 1);
            if (FacebookLogIn == true) FBLogIn();
            if (FacebookBind == true) FBBind();
        }// Update is called once per frame
        public class PlayerData//收取回傳資訊
        {
            public string uid;
            public string status;
            public string code;
        }

        public void LogIn()//登入
        {
            status = 0;
            if (AgreeToggle.isOn == false)
            {
                Wrong_Panel.SetActive(true);
                Wrong_Text.text = "請勾選同意服務條款";
            }
            if (AgreeToggle.isOn == true)
            {
                PlayerAccount = GameObject.FindGameObjectWithTag("LogInAccount").GetComponent<Text>();
                PlayerPassword = GameObject.FindGameObjectWithTag("LogInPassword").GetComponent<Text>();
                URL = "https://member.play8.com.tw/SDK_Test/api.php";
                GetKey = "CCeYZ7M2jB4w3rGPzypKcJe3XnkRtEgT";//Kate產出的
                APIMethod = "check";
                Account = PlayerAccount.text;
                MD5 md5Hash = MD5.Create();
                pwd = GetMD5Hash(md5Hash, PlayerPassword.text);
                time = GetTimeStamp();
                hash = sha256(APIMethod + GetGameID + Account + pwd + time + GetKey);
                StartCoroutine(SendGet(URL + "?method=" + APIMethod + "&GameID=" + GetGameID + "&Account=" + Account + "&pwd=" + pwd + "&time=" + time + "&hash=" + hash));//GET方法
            }
        }
        public void Create()//創帳號
        {
            status = 1;
            if (AgreeToggle.isOn == false)
            {
                Wrong_Panel.SetActive(true);
                Wrong_Text.text = "請勾選同意服務條款";
            }
            if (AgreeToggle.isOn == true)
            {
                PlayerAccount = GameObject.FindGameObjectWithTag("CreatAccount").GetComponent<Text>();
                PlayerPassword = GameObject.FindGameObjectWithTag("CreatPassword").GetComponent<Text>();
                CheckPassword = GameObject.FindGameObjectWithTag("CreatCheckPassword").GetComponent<Text>();
                URL = "https://member.play8.com.tw/SDK_Test/api.php";
                GetKey = "CCeYZ7M2jB4w3rGPzypKcJe3XnkRtEgT";//Kate產出的
                APIMethod = "create";
                Account = PlayerAccount.text;
                MD5 md5Hash = MD5.Create();
                if (PlayerPassword.text.Length >= 6 && PlayerPassword.text.Length <= 32)
                {
                    if (PlayerPassword.text == CheckPassword.text) { pwd = GetMD5Hash(md5Hash, PlayerPassword.text); CanCreate = true; }
                    else { Wrong_Panel.SetActive(true); Wrong_Text.text = "密碼不相同"; CanCreate = false; }
                }
                else { Wrong_Panel.SetActive(true); Wrong_Text.text = "密碼長度限制6~32個大小寫英文、數字"; CanCreate = false; }
                time = GetTimeStamp();
                hash = sha256(APIMethod + GetGameID + Account + pwd + time + GetKey);
                if (CanCreate == true) StartCoroutine(SendGet(URL + "?method=" + APIMethod + "&GameID=" + GetGameID + "&Account=" + Account + "&pwd=" + pwd + "&time=" + time + "&hash=" + hash));//GET方法
            }
        }
        public void FBLogIn()//FB登入
        {
            status = 2;
            if (AgreeToggle.isOn == false)
            {
                Wrong_Panel.SetActive(true);
                Wrong_Text.text = "請勾選同意服務條款";
            }
            if (AgreeToggle.isOn == true)
            {
                Play8ID = Facebook_Control.Play8ID;
                GameID = Facebook_Control.PlayerID;
                TokenID = Facebook_Control.BusinessToken;
                URL = "https://member.play8.com.tw/mobile_sdk/fb_openid/login_test";
                GetKey = "Kj2apm8kt#TWjF!z";
                time = GetTimeStamp();
                if(Play8ID=="")hash = sha256(GameID + GameAppID + TokenID + time + GetKey);
                else hash = sha256(Play8ID + Play8AppID+GameID + GameAppID + TokenID + time + GetKey);

                WWWForm form = new WWWForm();
                form.AddField("time", time);
                form.AddField("hash", hash);
                form.AddField("GameID", GameID);
                form.AddField("GameAppID", GameAppID);
                form.AddField("TokenID", TokenID);
                if (Play8ID != "") form.AddField("Play8ID", Play8ID);
                if (Play8ID != "") form.AddField("Play8AppID", Play8AppID);

                if (Play8ID == "") StartCoroutine(SendPost(URL + "?GameID=" + GameID + "&GameAppID=" + GameAppID + "&TokenID=" + TokenID + "&time=" + time + "&hash=" + hash, form));//POST方法
                else StartCoroutine(SendPost(URL + "?Play8ID=" + Play8ID + "&Play8AppID=" + Play8AppID + "&GameID=" + GameID + "&GameAppID=" + GameAppID + "&TokenID=" + TokenID +  "&time=" + time + "&hash=" + hash, form));//POST方法
            }
            FacebookLogIn = false;
        }
        public void FBBind()//FB綁定
        {
            status = 3;
            if (AgreeToggle.isOn == false)
            {
                Wrong_Panel.SetActive(true);
                Wrong_Text.text = "請勾選同意服務條款";
            }
            if (AgreeToggle.isOn == true)
            {
                Play8ID = Facebook_Control.Play8ID;
                print(Play8ID);
                GameID = Facebook_Control.PlayerID;
                TokenID = Facebook_Control.BusinessToken;
                UID =  UID.ToLower();
                URL = "https://member.play8.com.tw/mobile_sdk/fb_openid/direct_test";
                GetKey = "zJUxgUIX9QL5dywhgVNdimJZUGFhWyvq";
                time = GetTimeStamp();
                if (Play8ID == "") hash = sha256(GameID + GameAppID + TokenID + UID + time + GetKey);
                else hash = sha256(Play8ID + Play8AppID + GameID + GameAppID + TokenID + UID + time + GetKey);

                WWWForm form = new WWWForm();
                form.AddField("time", time);
                form.AddField("hash", hash);
                form.AddField("GameID", GameID);
                form.AddField("GameAppID", GameAppID);
                form.AddField("TokenID", TokenID);
                form.AddField("UID", UID);
                print(UID);
                if (Play8ID != "")
                {
                    form.AddField("Play8ID", Play8ID);
                    form.AddField("Play8AppID", Play8AppID);
                }
                if (Play8ID == "") StartCoroutine(SendPost(URL + "?GameID=" + GameID + "&GameAppID=" + GameAppID + "&TokenID=" + TokenID +"&UID="+ UID + "&time=" + time + "&hash=" + hash, form));//POST方法
                else StartCoroutine(SendPost(URL + "?Play8ID=" + Play8ID + "&Play8AppID=" + Play8AppID + "&GameID=" + GameID + "&GameAppID=" + GameAppID + "&TokenID=" + TokenID + "&UID=" + UID + "&time=" + time + "&hash=" + hash, form));//POST方法
            }
            FacebookBind = false;
        }

        private string GetMD5Hash(MD5 md5Hash, string input)//加密
        {
            //Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            //Create a new StringBuilder to collect the bytes and create a string.
            StringBuilder builder = new StringBuilder();

            //Loop through each byte of the hashed data and format each one as a hexadecimal strings.
            for (int cnt = 0; cnt < data.Length; cnt++)
            {
                builder.Append(data[cnt].ToString("x2"));
            }

            //Return the hexadecimal string
            return builder.ToString();
        }
        private bool VerifyMD5Hash(MD5 md5Hash, string input, string hash)//加密
        {
            //Hash the input
            string hashOfInput = GetMD5Hash(md5Hash, input);
            //Create a StringComparer to compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
        static int GetTimeStamp()//時間戳
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            int ret;
            ret = Convert.ToInt32(ts.TotalSeconds);
            return ret;
        }
        static string sha256(string text)//Hash
        {
            var crypt = new SHA256Managed();
            string hash = "";
            byte[] hashValue = crypt.ComputeHash(Encoding.ASCII.GetBytes(text));
            foreach (byte x in hashValue) { hash += String.Format("{0:x2}", x); }
            return hash;
        }

        IEnumerator SendGet(string _url)//傳資料P8
        {
            WWW getData = new WWW(_url);
            yield return getData;
            if (getData.error != null) { Debug.Log(getData.error); }
            else
            {
                Debug.Log(getData.text);
                string GetID = JsonUtility.FromJson<PlayerData>(getData.text).uid;
                string GetStatus = JsonUtility.FromJson<PlayerData>(getData.text).status;
                string GetCode = JsonUtility.FromJson<PlayerData>(getData.text).code;
                Log(status,Convert.ToInt32(GetCode));
            }
        }
        IEnumerator SendPost(string _url, WWWForm _wForm)//傳資料FB
        {
            print(_url);
            WWW postData = new WWW(_url, _wForm);
            yield return postData;
            if (postData.error != null){Debug.Log(postData.error);}
            else
            {
                Debug.Log(postData.text);
                string GetID = JsonUtility.FromJson<PlayerData>(postData.text).uid;
                string GetStatus = JsonUtility.FromJson<PlayerData>(postData.text).status;
                string GetCode = JsonUtility.FromJson<PlayerData>(postData.text).code;
                Log(status, Convert.ToInt32(GetCode));
            }
        }
        
        public void OpenURL()//忘記密碼
        {
            Application.OpenURL("https://member.play8.com.tw/member/forgot_pwd");
        }
        public void OpenRule()//規章
        {
            Application.OpenURL("http://www.play8.com.tw/service_center/sc_gamerules.html?Category=5");
        }
        public void ConfirmRule() { AgreeRule = !AgreeRule; }//同意規章
        public void RememberPassWord() { RememberPW = !RememberPW; }//記住密碼
        public void Clear() { PlayerPrefs.DeleteAll(); }//清除紀錄

        void Log(int status,int code)//回傳錯誤
        {
            Wrong_Panel.SetActive(true);
            Check.SetActive(false);

            switch (code)
            {
                case 1:
                    {
                        if (status == 0) Wrong_Text.text = "驗證成功";
                        if (status == 1) Wrong_Text.text = "建立成功";
                        PlayerPrefs.SetString("Account", PlayerAccount.text);
                        if (RememberPW == true) { PlayerPrefs.SetString("pwd", PlayerPassword.text); }
                    }
                    break;
                case 2:
                    if (status == 0) Wrong_Text.text = "帳號不存在";
                    if (status == 1) Wrong_Text.text = "帳號已存在"; break;
                case 3: Wrong_Text.text = "密碼錯誤"; break;

                #region

                case 301: Wrong_Text.text = "新增Play8帳號失敗"; break;
                case 302: Wrong_Text.text = "新增Play8帳號數值非預期"; break;
                case 303: Wrong_Text.text = "新增Play8帳號時開通遊戲失敗"; break;

                case 401: Wrong_Text.text = "method參數錯誤"; break;
                case 402: Wrong_Text.text = "GameID不能為空值"; break;
                case 403: Wrong_Text.text = "帳號錯誤"; break;
                case 404: Wrong_Text.text = "密碼不能為空值"; break;
                case 405: Wrong_Text.text = "hash不能為空值"; break;
                case 406: Wrong_Text.text = "hash值錯誤"; break;
                case 407: Wrong_Text.text = "time不能為空值"; break;
                case 408: Wrong_Text.text = "time值時間逾時"; break;
                case 409: Wrong_Text.text = "GameID值錯誤"; break;
                case 410: Wrong_Text.text = "開通遊戲失敗"; break;
                case 411: Wrong_Text.text = "FB綁定的Play8登入失敗"; break;

                case 1000: Wrong_Text.text = "操作成功"; break;
                case 1001: Wrong_Text.text = "不允許的IP"; break;
                case 1002: Wrong_Text.text = "Unix時間戳(time)為空"; break;
                case 1003: Wrong_Text.text = "驗證碼(hash)為空"; break;
                case 1004: Wrong_Text.text = "FB帳號編號(GameID)為空"; break;
                case 1005: Wrong_Text.text = "FB App ID(GameAppID)為空"; break;
                case 1006: Wrong_Text.text = "FB 帳號代碼(TokenID)為空"; break;
                case 1007:
                    if (status == 2) Wrong_Text.text = "Unix時間戳(time)不符規定";
                    if (status == 3) Wrong_Text.text = "遊戲原廠帳號編號(UID)為空"; break;
                case 1008:
                    if (status == 2) Wrong_Text.text = "驗證碼(hash)不符規定";
                    if (status == 3) Wrong_Text.text = "Unix時間戳(time)不符規定"; break;
                case 1009:
                    if (status == 2) Wrong_Text.text = "FB帳號編號(Play8ID)不符規定";
                    if (status == 3) Wrong_Text.text = "驗證碼(hash)不符規定"; break;
                case 1010:
                    if (status == 2) Wrong_Text.text = "FB App ID(Play8AppID)不符規定";
                    if (status == 3) Wrong_Text.text = "FB帳號編號(Play8ID)不符規定"; break;
                case 1011:
                    if (status == 2) Wrong_Text.text = "FB帳號編號(GameID)不符規定";
                    if (status == 3) Wrong_Text.text = "FB App ID(Play8AppID)不符規定"; break;
                case 1012:
                    if (status == 2) Wrong_Text.text = "FB App ID(GameAppID)不符規定";
                    if (status == 3) Wrong_Text.text = "FB帳號編號(GameID)不符規定"; break;
                case 1013:
                    if (status == 2) Wrong_Text.text = "FB 帳號代碼(TokenID)不符規定";
                    if (status == 3) Wrong_Text.text = "FB App ID(GameAppID)不符規定"; break;
                case 1014: Wrong_Text.text = "FB 帳號代碼(TokenID)不符規定"; break;
                case 1015: Wrong_Text.text = "遊戲原廠帳號編號(UID)不符規定"; break;

                case 2001: Wrong_Text.text = "請求超時"; break;
                case 2002: Wrong_Text.text = "驗證碼(hash)錯誤"; break;
                case 2003:
                    if (status == 2) Wrong_Text.text = "伺服器維修中";
                    if (status == 3) Wrong_Text.text = "遊戲原廠帳號編號(UID)錯誤"; break;
                case 2004:
                    if (status == 2) Wrong_Text.text = "登入遊戲異常 - 1";
                    if (status == 3) Wrong_Text.text = "伺服器維修中"; break;
                case 2005: Wrong_Text.text = "綁定遊戲異常 - 1"; break;
                    #endregion

            }
        }
    }
}