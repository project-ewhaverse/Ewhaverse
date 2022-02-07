using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class FriendData : MonoBehaviour
{
    private Text FriendID;
    private FriendInfo _friendInfo;

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
        FriendID = GetComponent<Text>();
    }

    public void showName()
    {
        /*
        string onlineStatus;
        if (_friendInfo.IsOnline)
            onlineStatus = "online";
        else
            onlineStatus = "offline";
        FriendID.text = _friendInfo.UserId + " " + onlineStatus;
        */
        FriendID.text = _friendInfo.UserId + " " + _friendInfo.IsOnline;
        Debug.Log(_friendInfo.IsOnline);
    }
}
