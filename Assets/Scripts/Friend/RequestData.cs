using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class RequestData : MonoBehaviour
{
    public TMP_Text freqName;
    public TMP_Text freqFrom;
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
        }
        else if (buddy.Equals("receive"))
        {
            freqFrom.text = "받은 요청";
        }
    }
}