using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";   //버전
    public Text text;   //서버 연결 알림 

    public InputField userIDText;   //userId

    void Awake()
    {
        //방장이 씬을 로딩하면, 나머지 사람들도 자동 싱크
        PhotonNetwork.AutomaticallySyncScene = true;
        
        //게임 버전
        PhotonNetwork.GameVersion = gameVersion;
        
        //서버 접속
        //if(!PhotonNetwork.IsConnected)
            //PhotonNetwork.ConnectUsingSettings();

        

    }

    //마스터 서버 연결 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        text.text = "서버 연결 성공!";
    }

    //마스터 서버 연결 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        text.text = "연결 중...";
        PhotonNetwork.ConnectUsingSettings();
    }

    //아이디 입력
    public void IDConfirm()
    {
        AuthenticationValues authValues = new AuthenticationValues(userIDText.text);
        PhotonNetwork.AuthValues = authValues;
        Debug.Log(PhotonNetwork.AuthValues.UserId);
        //서버 접속
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnJoinedLobby()
    {
        text.text = "로비 입장";
        //PhotonNetwork.FindFriends(new string[] { "1" });
    }


}
