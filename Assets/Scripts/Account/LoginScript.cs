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
public class LoginScript : MonoBehaviour
{
    [SerializeField] InputField LoginInputField1;
    [SerializeField] InputField LoginInputField2;
    [SerializeField] string url;
    public void LoginButton1Click() => StartCoroutine(LoginCoroutine("login"));
    public void LoginButton2Click() => SceneManager.LoadScene("RegisterScene");
    IEnumerator LoginCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id", LoginInputField1.text);
        form.AddField("password", LoginInputField2.text);
        form.AddField("email", "");
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        print(result);
        File.WriteAllText(Application.persistentDataPath + "/Sync.txt", result);
    }
}