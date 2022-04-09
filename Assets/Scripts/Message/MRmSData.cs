using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class MRmSData : MonoBehaviour
{
    public TMP_Text mrmsName;
    public TMP_Text mrmsMtext;
    public TMP_Text mrmsTime;
    void Awake()
    {
        mrmsName = transform.Find("Entity").transform.Find("Name").GetComponent<TMP_Text>();
        mrmsMtext = transform.Find("Entity").transform.Find("Mtext").GetComponent<TMP_Text>();
        mrmsTime = transform.Find("Entity").transform.Find("Time").GetComponent<TMP_Text>();
    }
    public void showRmS(string id, string mtext, string time)
    {
        mrmsName.text = id;
        mrmsMtext.text = mtext;
        mrmsTime.text = time.Substring(2, 2) + "/" + time.Substring(5, 2) + "/" + time.Substring(8, 2) + " " + time.Substring(11, 2) + ":" + time.Substring(14, 2);
    }
}