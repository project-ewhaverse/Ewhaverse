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
    [SerializeField] InputField RegisterInputField3;
    [SerializeField] string url;
    public void RegisterButton1Click()
    {
        if (RegisterInputField2.text.ToString().Contains("@ewhain.net") || RegisterInputField2.text.ToString().Contains("@ewha.ac.kr"))
        {
            StartCoroutine(RegisterCoroutine("register"));
        }
        else
        {
            print("이화 메일로만 가입할 수 있습니다.");
        }
    }
    public void RegisterButton2Click() => SceneManager.LoadScene("LoginScene");
    IEnumerator RegisterCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", RegisterInputField1.text);
        form.AddField("password", RegisterInputField3.text);
        form.AddField("nickname", RegisterInputField1.text);
        form.AddField("item", "ia1,ib1,ic1,id1,ie1");
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        print(result);
    }
}