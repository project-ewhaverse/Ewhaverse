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
public class M1
{
    public M1(string _rid, string _mid, string _id1, string _id2, string _time, string _mtext)
    {
        rid = _rid; mid = _mid; id1 = _id1; id2 = _id2; time = _time; mtext = _mtext;
    }
    public string rid, mid, id1, id2, time, mtext;
}
public class MessageListing : MonoBehaviour
{
    public Dictionary<string, GameObject> m1Dict = new Dictionary<string, GameObject>();
    public List<M1> m1List = new List<M1>();
    public string m1Index;
    IEnumerator coroutine1, coroutine2;
    [SerializeField] GameObject m1UI;
    [SerializeField] GameObject m2UI;
    [SerializeField] GameObject m1Prefab;
    [SerializeField] Transform content;
    [SerializeField] TMP_InputField addMessage;
    [SerializeField] string url;
    void Awake()
    {
        coroutine1 = m1UiCheck();
        coroutine2 = m1DbCheck();
    }
    public void m1SetActive()
    {
        if (m1UI.gameObject.activeSelf)
        {
            m1UI.gameObject.SetActive(false);
            StopCoroutine(coroutine2);
            StopCoroutine(coroutine1);
        }
        else
        {
            m1Index = "0";
            m1UI.gameObject.SetActive(true);
            File.WriteAllText(Application.persistentDataPath + "/SyncM1.txt", "");
            StartCoroutine(coroutine1);
            StartCoroutine(coroutine2);
        }
    }
    IEnumerator m1UiCheck()
    {
        while (true)
        {
            string tmp = File.ReadAllText(Application.persistentDataPath + "/SyncM1.txt");
            if (tmp != "")
            {
                m1UI.gameObject.SetActive(false);
                m2UI.gameObject.SetActive(true);
                StopCoroutine(coroutine2);
                StopCoroutine(coroutine1);
            }
            yield return new WaitForSeconds(3.0f);
        }
    }
    IEnumerator m1DbCheck()
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("command", "m1num");
            form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
            form.AddField("id2", "");
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
            string result = www.downloadHandler.text;
            if (result != m1Index)
            {
                m1Index = result;
                m1List.Clear();
                WWWForm form1 = new WWWForm();
                form1.AddField("command", "m1load");
                form1.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
                form1.AddField("id2", "");
                UnityWebRequest www1 = UnityWebRequest.Post(url, form1);
                yield return www1.SendWebRequest();
                string rdata = www1.downloadHandler.text;
                if (rdata != "[]")
                {
                    m1List = JsonConvert.DeserializeObject<List<M1>>(rdata);
                }
                m1ListPaint();
            }
            yield return new WaitForSeconds(3.0f);
        }
    }
    public void m1ListPaint()
    {
        string tmpid;
        for (int i = 0; i < m1List.Count; i++)
        {
            if (m1List[i].id1 == File.ReadAllText(Application.persistentDataPath + "/Sync.txt"))
            {
                tmpid = m1List[i].id2;
            }
            else
            {
                tmpid = m1List[i].id1;
            }
            if (!m1Dict.ContainsKey(tmpid))
            {
                GameObject _m1 = Instantiate(m1Prefab, content);
                _m1.GetComponent<MListData>().showList(tmpid, m1List[i].mtext, m1List[i].time);
                m1Dict.Add(tmpid, _m1);
            }
            else
            {
                GameObject tempm1 = m1Dict[tmpid];
                tempm1.GetComponent<MListData>().showList(tmpid, m1List[i].mtext, m1List[i].time);
            }
        }
    }
    public void m1ClickBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = addMessage.text;
        if (id1 != id2)
        {
            StartCoroutine(m1AddRoom(id2));
        }
    }
    IEnumerator m1AddRoom(string id2)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "m1add");
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("id2", id2);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        print(result);
    }
}