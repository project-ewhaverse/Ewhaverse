using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        if (time == "0000-00-00-00-00")
        {
            mlistTime.text = "";
        }
        else
        {
            mlistTime.text = time.Substring(2, 2) + "/" + time.Substring(5, 2) + "/" + time.Substring(8, 2) + " " + time.Substring(11, 2) + ":" + time.Substring(14, 2);
        }
    }
    public void m1ClickRoom()
    {
        File.WriteAllText(Application.persistentDataPath + "/SyncM1.txt", mlistName.text);
    }
}