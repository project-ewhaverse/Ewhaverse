using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class MRoomData : MonoBehaviour
{
    public TMP_Text mroomName;
    public TMP_Text mroomMtext;
    public TMP_Text mroomTime;
    void Awake()
    {
        mroomName = transform.Find("Name").GetComponent<TMP_Text>();
        mroomMtext = transform.Find("Mtext").GetComponent<TMP_Text>();
        mroomTime = transform.Find("Time").GetComponent<TMP_Text>();
    }
    public void showRoom(string id, string mtext, string time)
    {
        mroomName.text = id;
        mroomMtext.text = mtext;
        mroomTime.text = time;
    }
}