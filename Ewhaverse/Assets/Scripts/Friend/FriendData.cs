using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class FriendData : MonoBehaviour
{
    public FriendInfo _friendInfo;
    public TMP_Text flistName;
    public TMP_Text flistWhere;
    public Image flistMark;
    public FriendInfo FriendInfo
    {
        get
        {
            return _friendInfo;
        }
        set
        {
            _friendInfo = value;
        }
    }
    void Awake()
    {
        flistName = transform.Find("Name").GetComponent<TMP_Text>();
        flistWhere = transform.Find("Where").GetComponent<TMP_Text>();
        flistMark = transform.Find("Mark").GetComponent<Image>();
    }
    public void showFriend()
    {
        flistName.text = _friendInfo.UserId;
        if (_friendInfo.IsOnline)
        {
            if (_friendInfo.Room == "")
                flistWhere.text = "¥Î±§¿Â";
            else
                flistWhere.text = _friendInfo.Room;
            flistMark.sprite = Resources.Load<Sprite>("UI/Login_Mark");
        }
        else
        {
            flistWhere.text = "";
            flistMark.sprite = Resources.Load<Sprite>("UI/Logout_Mark");
        }
    }
}