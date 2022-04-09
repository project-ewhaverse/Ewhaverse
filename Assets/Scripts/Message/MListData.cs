using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class MListData : MonoBehaviour
{
    public TMP_Text mlistName;
    public TMP_Text mlistMtext;
    public TMP_Text mlistTime;
    void Awake()
    {
        mlistName = transform.Find("Name").GetComponent<TMP_Text>();
        mlistMtext = transform.Find("Mtext").GetComponent<TMP_Text>();
        mlistTime = transform.Find("Time").GetComponent<TMP_Text>();
    }
    public void showList(string id, string mtext, string time)
    {
        mlistName.text = id;
        if (mtext.Length > 10)
        {
            mlistMtext.text = mtext.Substring(0, 10) + "...";
        }
        else
        {
            mlistMtext.text = mtext;
        }
        mlistTime.text = time;
    }
}