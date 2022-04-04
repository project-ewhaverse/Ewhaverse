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
    public Image flistMark;
    void Awake()
    {
        flistName = transform.Find("Name").GetComponent<TMP_Text>();
        flistWhere = transform.Find("Where").GetComponent<TMP_Text>();
        flistMark = transform.Find("Mark").GetComponent<Image>();
    }
    public void showFriend(string id, bool online, string room)
    {
        flistName.text = id;
        if (online)
        {
            if (room == "")
                flistWhere.text = "¥Î±§¿Â";
            else
                flistWhere.text = room;
            flistMark.sprite = Resources.Load<Sprite>("UI/Login_Mark");
        }
        else
        {
            flistWhere.text = "";
            flistMark.sprite = Resources.Load<Sprite>("UI/Logout_Mark");
        }
    }
}