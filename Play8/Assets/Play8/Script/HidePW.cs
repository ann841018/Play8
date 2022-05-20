using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facebook.Unity
{
    public class HidePW : MonoBehaviour
    {
        [SerializeField] Text Account;
        [SerializeField] Text RememberAccount;
        [SerializeField] Text RememberPassword;
        [SerializeField] Text PlayerPassword;
        [SerializeField] Text HidePassword;
        [SerializeField] Toggle See;
        [SerializeField] GameObject Hide;
        [SerializeField] GameObject InputAccount;
        [SerializeField] GameObject InputPW;


        [SerializeField] Text Wrong_Text;
        [SerializeField] GameObject Wrong_Panel;
        [SerializeField] GameObject Check;

        public static bool Creat;

        void Start() { }// Use this for initialization

        void Update()// Update is called once per frame
        {
            if (Creat == false)
            {
                if (PlayerPrefs.GetString("Account") != "")//記住帳號
                {
                    if (Account.text == "")//不打帳號
                    {
                        InputAccount.SetActive(false);
                        RememberAccount.text = PlayerPrefs.GetString("Account");
                        Account.gameObject.SetActive(false);
                        if (Player8_System.RememberPW == true)//開記得密碼
                        {
                            if (PlayerPrefs.GetString("pwd") != "")//有記得密碼
                            {
                                InputPW.SetActive(false);
                                RememberPassword.text = PlayerPrefs.GetString("pwd");
                                RememberPassword.gameObject.SetActive(true);
                                PlayerPassword.gameObject.SetActive(false);
                                if (See.isOn == true) Hide.SetActive(false);
                                else Hide.SetActive(true);
                                if (HidePassword.text.Length < RememberPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                                if (HidePassword.text.Length > RememberPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (RememberPassword.text.Length));
                            }
                            else//沒有記得密碼
                            {
                                InputPW.SetActive(true);
                                RememberPassword.gameObject.SetActive(false);
                                PlayerPassword.gameObject.SetActive(true);
                                PlayerPrefs.SetString("pwd", "");
                                if (See.isOn == true) Hide.SetActive(false);
                                else Hide.SetActive(true);
                                if (HidePassword.text.Length < PlayerPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                                if (HidePassword.text.Length > PlayerPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (PlayerPassword.text.Length));
                            }
                        }
                        else if (Player8_System.RememberPW == false)//關記住密碼
                        {
                            InputPW.SetActive(true);
                            RememberPassword.gameObject.SetActive(false);
                            PlayerPassword.gameObject.SetActive(true);
                            PlayerPrefs.SetString("pwd", "");
                            if (See.isOn == true) Hide.SetActive(false);
                            else Hide.SetActive(true);
                            if (HidePassword.text.Length < PlayerPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                            if (HidePassword.text.Length > PlayerPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (PlayerPassword.text.Length));
                        }
                    }
                    else if (Account.text == PlayerPrefs.GetString("Account"))//打原本的帳號
                    {
                        Account.gameObject.SetActive(true);
                        InputPW.SetActive(false);
                        RememberPassword.text = PlayerPrefs.GetString("pwd");
                        RememberPassword.gameObject.SetActive(true);
                        PlayerPassword.gameObject.SetActive(false);
                        if (See.isOn == true) Hide.SetActive(false);
                        else Hide.SetActive(true);
                        if (HidePassword.text.Length < RememberPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                        if (HidePassword.text.Length > RememberPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (RememberPassword.text.Length));
                    }
                    else if (Account.text != PlayerPrefs.GetString("Account"))//打新的帳號
                    {
                        Account.gameObject.SetActive(true);
                        InputPW.SetActive(true);
                        RememberPassword.text = "";
                        RememberAccount.text = "";
                        RememberPassword.gameObject.SetActive(false);
                        PlayerPassword.gameObject.SetActive(true);
                        if (See.isOn == true) Hide.SetActive(false);
                        else Hide.SetActive(true);
                        if (HidePassword.text.Length < PlayerPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                        if (HidePassword.text.Length > PlayerPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (PlayerPassword.text.Length));
                    }
                }
                else if (PlayerPrefs.GetString("Account") == "")//沒有記住帳號
                {
                    if (See.isOn == true) Hide.SetActive(false);
                    else Hide.SetActive(true);
                    if (HidePassword.text.Length < PlayerPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                    if (HidePassword.text.Length > PlayerPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (PlayerPassword.text.Length));
                }
            }
            else if (Creat == true)//創帳號頁面
            {
                if (See.isOn == true) Hide.SetActive(false);
                else Hide.SetActive(true);
                if (HidePassword.text.Length < PlayerPassword.text.Length) HidePassword.text = HidePassword.text + "*";
                if (HidePassword.text.Length > PlayerPassword.text.Length) HidePassword.text = HidePassword.text.Substring(0, (PlayerPassword.text.Length));
            }
        }

        /*public void PassWordLong()
        {
            string PassWord_Str = PlayerPassword.text.ToString();
            string[] unable = { " ", ",", ".", "/", "<", ">", "?", ";", ":", "[", "]", "{", "}", "|", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "`", "~" };
            byte[] byte_len = System.Text.Encoding.Default.GetBytes(PassWord_Str);//中文2單位英數1單位

            if (byte_len.Length < 6 || byte_len.Length > 32)
            {
                Wrong_Panel.SetActive(true); 
                Wrong_Text.text = "密碼長度限制6~32個大小寫英文、數字";
                Check.SetActive(false);
            }
        } */


        public void LogIn_Panel()
        {
            Creat = false;
        }
        public void Cteat_Panel()
        {
            Creat = true;
        }
    }
}
