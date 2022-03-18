using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RegisterScript : MonoBehaviour
{
    [SerializeField] InputField RegisterInputField1;
    [SerializeField] InputField RegisterInputField2;
    [SerializeField] InputField RegisterInputFielda;
    [SerializeField] InputField RegisterInputField3;
    [SerializeField] Text RegisterTexta;
    [SerializeField] string url;
    [SerializeField] string urla;
    [SerializeField] string authstr;
    public void RegisterButtonaClick()
    {
        string addr = RegisterInputField2.text.ToString();
        if (addr.Contains("@ewhain.net") || addr.Contains("@ewhain.net") || addr.Contains("guigim0312@"))
        {
            int authnum = UnityEngine.Random.Range(1000000, 10000000);
            authstr = addr + authnum.ToString();
            StartCoroutine(AuthCoroutine("send", authnum));
        }
    }
    public void RegisterButton1Click()
    {
        string trystr = RegisterInputField2.text + RegisterInputFielda.text;
        if (authstr.Equals(trystr))
        {
            StartCoroutine(RegisterCoroutine("register"));
        }
        else
        {
            RegisterTexta.text = "인증번호가 잘못되었습니다";
        }
    }
    public void RegisterButton2Click() => SceneManager.LoadScene("LoginScene");
    IEnumerator AuthCoroutine(string command, int authnum)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("addr", RegisterInputField2.text);
        form.AddField("num", authnum);
        UnityWebRequest www = UnityWebRequest.Post(urla, form);
        yield return www.SendWebRequest();
        RegisterTexta.text = "인증번호가 전송되었습니다";
    }
    IEnumerator RegisterCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", RegisterInputField1.text);
        form.AddField("password", RegisterInputField3.text);
        form.AddField("nickname", RegisterInputField1.text);
        form.AddField("item", "ia1,ib1,ic1,id1,ie1");
        form.AddField("email", RegisterInputField2.text);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        RegisterTexta.text = result;
    }
}