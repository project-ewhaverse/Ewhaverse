using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class FriendData : MonoBehaviour
{
    public TMP_Text flistName;
    public TMP_Text flistWhere;
    void Awake()
    {
        flistName = transform.Find("Name").GetComponent<TMP_Text>();
        flistWhere = transform.Find("Where").GetComponent<TMP_Text>();
    }
    public void showFriend(string id)
    {
        flistName.text = id;
        flistWhere.text = "Ä£±¸";
    }
}