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
public class RequestScript : MonoBehaviour
{
    [SerializeField] InputField RequestInputField1;
    [SerializeField] InputField RequestInputField2;
    [SerializeField] Text RequestText1;
    [SerializeField] Text RequestText2;
    [SerializeField] string url;
    public string result;
    void Start()
    {
        buddylist();
        requestlist();
    }
    public void buddylist()
    {
        StartCoroutine(RequestCoroutine("buddyload"));
    }
    public void requestlist()
    {
        StartCoroutine(RequestCoroutine("requestload"));
    }
    IEnumerator RequestCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        if (command.Contains("send"))
        {
            form.AddField("id2", RequestInputField1.text);
        }
        else
        {
            form.AddField("id2", RequestInputField2.text);
        }
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        print(result);
        if (command.Contains("buddyload"))
        {
            RequestText1.text = result;
        }
        if(command.Contains("requestload"))
        {
            RequestText2.text = result;
        }
    }
    public void RequestButton1Click()
    {
        StartCoroutine(RequestCoroutine("requestsend"));
    }
    public void RequestButton2Click()
    {
        StartCoroutine(RequestCoroutine("requestaccept"));
        buddylist();
        requestlist();
    }
    public void RequestButton3Click()
    {
        StartCoroutine(RequestCoroutine("requestreject"));
        requestlist();
    }
}