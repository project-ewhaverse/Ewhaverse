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
public class Dbflist
{
    public Dbflist(string _id1, string _id2, string _buddy)
    {
        id1 = _id1; id2 = _id2; buddy = _buddy;
    }
    public string id1, id2, buddy;
}
public class FriendListing : MonoBehaviourPunCallbacks
{
    public Dictionary<string, GameObject> friendDict = new Dictionary<string, GameObject>();
    public List<string> friendList = new List<string>();
    public List<Dbflist> dbflist = new List<Dbflist>();
    IEnumerator coroutine1, coroutine2;
    [SerializeField] GameObject friendUI;
    [SerializeField] GameObject friendPrefab;
    [SerializeField] Transform content;
    [SerializeField] TMP_InputField addFriend;
    [SerializeField] string url;
    void Awake()
    {
        coroutine1 = dbFriendCheck();
        coroutine2 = friendUpdate();
    }
    public void SetActiveFList()
    {
        if (friendUI.gameObject.activeSelf)
        {
            friendUI.gameObject.SetActive(false);
            StopCoroutine(coroutine2);
            StopCoroutine(coroutine1);
        }
        else
        {
            friendUI.gameObject.SetActive(true);
            StartCoroutine(coroutine1);
            StartCoroutine(coroutine2);
        }
    }
    IEnumerator dbFriendCheck()
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("command", "flistnum");
            form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
            form.AddField("id2", "");
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
            string result = www.downloadHandler.text;
            if (result != dbflist.Count.ToString())
            {
                dbflist.Clear();
                friendList.Clear();
                WWWForm form1 = new WWWForm();
                form1.AddField("command", "flistload");
                form1.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
                form1.AddField("id2", "");
                UnityWebRequest www1 = UnityWebRequest.Post(url, form1);
                yield return www1.SendWebRequest();
                string rdata = www1.downloadHandler.text;
                if (rdata != "[]")
                {
                    dbflist = JsonConvert.DeserializeObject<List<Dbflist>>(rdata);
                    for (int i = 0; i < dbflist.Count; i++)
                    {
                        friendList.Add(dbflist[i].id2);
                    }
                }
            }
            yield return new WaitForSeconds(3.0f);
        }
    }
    IEnumerator friendUpdate()
    {
        while (true)
        {
            if (friendList.Count != 0)
                PhotonNetwork.FindFriends(friendList.ToArray());
            yield return new WaitForSeconds(3.0f);
        }
    }
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        foreach (var friend in friendList)
        {
            if (!friendDict.ContainsKey(friend.UserId))
            {
                GameObject _friend = Instantiate(friendPrefab, content);
                _friend.GetComponent<FriendData>().showFriend(friend.UserId, friend.IsOnline, friend.Room);
                friendDict.Add(friend.UserId, _friend);
            }
            else
            {
                GameObject tempFriend = friendDict[friend.UserId];
                tempFriend.GetComponent<FriendData>().showFriend(friend.UserId, friend.IsOnline, friend.Room);
            }
        }
    }
}