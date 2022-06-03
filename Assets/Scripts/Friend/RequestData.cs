using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class RequestData : MonoBehaviour
{
    public TMP_Text freqName;
    public TMP_Text freqFrom;
    [SerializeField] GameObject btnAccept, btnReject;
    [SerializeField] string url;
    void Awake()
    {
        freqName = transform.Find("Name").GetComponent<TMP_Text>();
        freqFrom = transform.Find("From").GetComponent<TMP_Text>();
    }
    public void showRequest(string id, string buddy)
    {
        freqName.text = id;
        if (buddy.Equals("send"))
        {
            freqFrom.text = "보낸 요청";
            btnAccept.SetActive(false);
            btnReject.SetActive(false);

        }
        else if (buddy.Equals("receive"))
        {
            freqFrom.text = "받은 요청";
        }
    }
    public void ClickAcceptBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = freqName.text;
        if (id1 != id2)
        {
            StartCoroutine(REntityCoroutine("reqaccept"));
        }
    }
    public void ClickRejectBtn()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = freqName.text;
        if (id1 != id2)
        {
            StartCoroutine(REntityCoroutine("reqreject"));
        }
    }
    IEnumerator REntityCoroutine(string command)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("id2", freqName.text);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        print(result);
    }
}