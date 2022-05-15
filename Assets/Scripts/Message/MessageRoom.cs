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
public class M2
{
    public M2(string _rid, string _mid, string _id1, string _id2, string _time, string _mtext)
    {
        rid = _rid; mid = _mid; id1 = _id1; id2 = _id2; time = _time; mtext = _mtext;
    }
    public string rid, mid, id1, id2, time, mtext;
}
public class MessageRoom : MonoBehaviour
{
    public Dictionary<string, GameObject> m2Dict = new Dictionary<string, GameObject>();
    public List<M2> m2List = new List<M2>();
    public string m2Index;
    public bool m2Bool;
    IEnumerator coroutine1, coroutine2;
    [SerializeField] GameObject m1UI;
    [SerializeField] GameObject m2UI;
    [SerializeField] GameObject m2SPrefab, m2RPrefab;
    [SerializeField] Transform content;
    [SerializeField] TMP_InputField sendMessage;
    [SerializeField] string url;
    [SerializeField] VerticalLayoutGroup ct;
    void Awake()
    {
        m2Bool = false;
        coroutine1 = m2UiCheck();
        coroutine2 = m2DbCheck();
        StartCoroutine(coroutine1);
    }
    public void m2SetClose()
    {
        if (m2UI.gameObject.activeSelf && m2Bool == true)
        {
            StopCoroutine(coroutine2);
            for (int i = 1; i < m2List.Count; i++)
            {
                GameObject _temp = m2Dict[i.ToString()];
                m2Dict.Remove(i.ToString());
                Destroy(_temp);
            }
            m2Bool = false;
            m2UI.gameObject.SetActive(false);
            StartCoroutine(coroutine1);
        }
    }
    IEnumerator m2UiCheck()
    {
        while (true)
        {
            if (m2UI.gameObject.activeSelf && m2Bool == false)
            {
                m2Index = "0";
                m2Bool = true;
                m1UI.gameObject.SetActive(false);
                StartCoroutine(coroutine2);
                StopCoroutine(coroutine1);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
    IEnumerator m2DbCheck()
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("command", "m2num");
            form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
            form.AddField("id2", File.ReadAllText(Application.persistentDataPath + "/SyncM1.txt"));
            form.AddField("time", "");
            form.AddField("mtext", "");
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
            string result = www.downloadHandler.text;
            if (result != m2Index)
            {
                m2Index = result;
                m2List.Clear();
                WWWForm form1 = new WWWForm();
                form1.AddField("command", "m2load");
                form1.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
                form1.AddField("id2", File.ReadAllText(Application.persistentDataPath + "/SyncM1.txt"));
                form1.AddField("time", "");
                form1.AddField("mtext", "");
                UnityWebRequest www1 = UnityWebRequest.Post(url, form1);
                yield return www1.SendWebRequest();
                string rdata = www1.downloadHandler.text;
                if (rdata != "[]")
                {
                    m2List = JsonConvert.DeserializeObject<List<M2>>(rdata);
                }
                m2ListPaint();
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ct.transform);
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
    public void m2ListPaint()
    {
        for (int i = 1; i < m2List.Count; i++)
        {
            if (!m2Dict.ContainsKey(m2List[i].mid))
            {
                GameObject _m2;
                if (m2List[i].id1 == File.ReadAllText(Application.persistentDataPath + "/Sync.txt"))
                {
                    _m2 = Instantiate(m2SPrefab, content);
                    _m2.GetComponent<MRmSData>().showRmS(m2List[i].id1, m2List[i].mtext, m2List[i].time);
                }
                else
                {
                    _m2 = Instantiate(m2RPrefab, content);
                    _m2.GetComponent<MRmRData>().showRmR(m2List[i].id1, m2List[i].mtext, m2List[i].time);
                }
                m2Dict.Add(m2List[i].mid, _m2);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ct.transform);
        }
    }
    public void m2ClickSend()
    {
        string mtext = sendMessage.text;
        if (mtext != "")
        {
            DateTime dt = DateTime.Now;
            StartCoroutine(m2SendMsg(dt.ToString("yyyy-MM-dd-HH-mm"), mtext));
        }
    }
    IEnumerator m2SendMsg(string time, string mtext)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "m2send");
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("id2", File.ReadAllText(Application.persistentDataPath + "/SyncM1.txt"));
        form.AddField("time", time);
        form.AddField("mtext", mtext);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        print(result);
    }
}