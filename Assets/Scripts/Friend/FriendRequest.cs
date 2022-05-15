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
using Photon.Pun;
using Photon.Realtime;
using TMPro;
[System.Serializable]
public class Dbrlist
{
    public Dbrlist(string _id1, string _id2, string _buddy)
    {
        id1 = _id1; id2 = _id2; buddy = _buddy;
    }
    public string id1, id2, buddy;
}
public class FriendRequest : MonoBehaviour
{
    public Dictionary<string, GameObject> requestDict = new Dictionary<string, GameObject>();
    public List<Dbrlist> dbrlist = new List<Dbrlist>();
    public List<string> tmplist = new List<string>();
    IEnumerator coroutine1;
    [SerializeField] GameObject requestUI;
    [SerializeField] GameObject requestPrefab;
    [SerializeField] Transform content;
    [SerializeField] TMP_InputField addFriend;
    [SerializeField] TMP_InputField acceptFriend;
    [SerializeField] string url;
    void Awake()
    {
        coroutine1 = dbRequestCheck();
    }
    public void SetActiveFReq()
    {
        if (requestUI.gameObject.activeSelf)
        {
            requestUI.gameObject.SetActive(false);
            StopCoroutine(coroutine1);
        }
        else
        {
            requestUI.gameObject.SetActive(true);
            StartCoroutine(coroutine1);
        }
    }
    IEnumerator dbRequestCheck()
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("command", "rlistnum");
            form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
            form.AddField("id2", "");
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
            string result = www.downloadHandler.text;
            if (result != dbrlist.Count.ToString())
            {
                StartCoroutine(requestUpdate());
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
    IEnumerator requestUpdate()
    {
        dbrlist.Clear();
        WWWForm form1 = new WWWForm();
        form1.AddField("command", "rlistload");
        form1.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form1.AddField("id2", "");
        UnityWebRequest www1 = UnityWebRequest.Post(url, form1);
        yield return www1.SendWebRequest();
        string rdata = www1.downloadHandler.text;
        if (rdata != "[]")
        {
            dbrlist = JsonConvert.DeserializeObject<List<Dbrlist>>(rdata);
        }
        requestPaint();
    }
    public void requestPaint()
    {
        tmplist.Clear();
        foreach (string reqid2 in requestDict.Keys)
        {
            tmplist.Add(reqid2);
        }
        if(tmplist.Count > dbrlist.Count)
        {
            for (int i = 0; i < tmplist.Count; i++)
            {
                if (dbrlist.Exists(x => x.id2 == tmplist[i]))
                    continue;
                else
                {
                    GameObject _temp = requestDict[tmplist[i]];
                    requestDict.Remove(tmplist[i]);
                    Destroy(_temp);
                }
            }
        }
        else
        {
            for (int i = 0; i < dbrlist.Count; i++)
            {
                if (!requestDict.ContainsKey(dbrlist[i].id2))
                {
                    GameObject _request = Instantiate(requestPrefab, content);
                    _request.GetComponent<RequestData>().showRequest(dbrlist[i].id2, dbrlist[i].buddy);
                    requestDict.Add(dbrlist[i].id2, _request);
                }
                else
                {
                    GameObject tempRequest = requestDict[dbrlist[i].id2];
                    tempRequest.GetComponent<RequestData>().showRequest(dbrlist[i].id2, dbrlist[i].buddy);
                }
            }
        }
    }
    public void ClickAddBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = addFriend.text;
        if (id1 != id2)
        {
            StartCoroutine(ReqBtnCoroutine("reqadd"));
        }
    }
    public void ClickAddCancelBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = addFriend.text;
        if (id1 != id2)
        {
            StartCoroutine(ReqBtnCoroutine("reqaddcancel"));
        }
    }
    public void ClickAcceptBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = acceptFriend.text;
        if (id1 != id2)
        {
            StartCoroutine(ReqBtnCoroutine("reqaccept"));
        }
    }
    public void ClickRejectBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = acceptFriend.text;
        if (id1 != id2)
        {
            StartCoroutine(ReqBtnCoroutine("reqreject"));
        }
    }
    IEnumerator ReqBtnCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        if (command.Contains("reqadd"))
        {
            form.AddField("id2", addFriend.text);
        }
        else
        {
            form.AddField("id2", acceptFriend.text);
        }
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        print(result);
    }
}