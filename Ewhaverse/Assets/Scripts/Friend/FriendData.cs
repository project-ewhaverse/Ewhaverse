using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class FriendData : MonoBehaviour
{
    private FriendInfo _friendInfo;
    private TMP_Text FriendID;      //친구 ID
    private TMP_Text FriendWhere;   //친구 어디에 있는지
    private Image LoginMark;        //친구 로그인 상태
    

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
        FriendID = transform.Find("FriendName").GetComponent<TMP_Text>();
        FriendWhere = transform.Find("Where").GetComponent<TMP_Text>();
        LoginMark = transform.Find("Login_Mark").GetComponent<Image>();
    }

    public void showName()
    {
        FriendID.text = _friendInfo.UserId;
        FriendWhere.text = _friendInfo.Room;
        if (_friendInfo.IsOnline)
            LoginMark.sprite = Resources.Load<Sprite>("UI/Login_Mark");
    }
   
}
