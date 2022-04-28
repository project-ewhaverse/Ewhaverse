using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class MRmRData : MonoBehaviour
{
    public TMP_Text mrmrName;
    public TMP_Text mrmrMtext;
    public TMP_Text mrmrTime;
    void Awake()
    {
        mrmrName = transform.Find("Entity").transform.Find("Name").GetComponent<TMP_Text>();
        mrmrMtext = transform.Find("Entity").transform.Find("Mtext").GetComponent<TMP_Text>();
        mrmrTime = transform.Find("Entity").transform.Find("Time").GetComponent<TMP_Text>();
    }
    public void showRmR(string id, string mtext, string time)
    {
        mrmrName.text = id;
        mrmrMtext.text = mtext;
        mrmrTime.text = time.Substring(2, 2) + "/" + time.Substring(5, 2) + "/" + time.Substring(8, 2) + " " + time.Substring(11, 2) + ":" + time.Substring(14, 2);
    }
}