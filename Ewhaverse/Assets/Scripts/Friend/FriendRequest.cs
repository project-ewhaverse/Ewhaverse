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
    [SerializeField] GameObject requestUI;
    [SerializeField] GameObject requestPrefab;
    [SerializeField] Transform content;
    [SerializeField] TMP_InputField addFriend;
    [SerializeField] TMP_InputField accpetFriend;
    [SerializeField] string url;
    IEnumerator coroutine1, coroutine2;
    public List<Dbrlist> dbrlist = new List<Dbrlist>();
    void Awake()
    {
        coroutine1 = requestUpdate();
        coroutine2 = dbRequestCheck();
    }
    public void SetActiveFReq()
    {
        if (requestUI.gameObject.activeSelf)
        {
            requestUI.gameObject.SetActive(false);
            StopCoroutine(coroutine2);
            StopCoroutine(coroutine1);
        }
        else
        {
            requestUI.gameObject.SetActive(true);
            StartCoroutine(coroutine1);
            StartCoroutine(coroutine2);
        }
    }
    IEnumerator requestUpdate()
    {
        while (true)
        {
            if (PhotonNetwork.InLobby && requestUI.gameObject.activeSelf && dbrlist.Count != 0)
            {
                OnRequestListUpdate();
            }
            yield return new WaitForSeconds(3.0f);
        }
    }
    public void OnRequestListUpdate()
    {
        for(int i = 0; i < dbrlist.Count; i++)
        {
            GameObject tempRequest;
            if (!requestDict.ContainsKey(dbrlist[i].id2))
            {
                GameObject _request = Instantiate(requestPrefab, content);
                _request.GetComponent<RequestData>().showRequest(dbrlist[i].id2.ToString(), dbrlist[i].buddy.ToString());
                requestDict.Add(dbrlist[i].id2, _request);
            }
            else
            {
                requestDict.TryGetValue(dbrlist[i].id2, out tempRequest);
                tempRequest.GetComponent<RequestData>().showRequest(dbrlist[i].id2, dbrlist[i].buddy);
            }
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
            if(result != dbrlist.Count.ToString())
            {
                WWWForm form1 = new WWWForm();
                form1.AddField("command", "rlistload");
                form1.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
                form1.AddField("id2", "");
                UnityWebRequest www1 = UnityWebRequest.Post(url, form1);
                yield return www1.SendWebRequest();
                string rdata = www1.downloadHandler.text;
                if (rdata != "[]")
                {
                    dbrlist.Clear();
                    dbrlist = JsonConvert.DeserializeObject<List<Dbrlist>>(rdata);
                }
            }
            yield return new WaitForSeconds(3.0f);
        }
    }
}