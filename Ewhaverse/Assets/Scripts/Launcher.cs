using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";   //버전

    public InputField userIDText;   //userId

    void Awake()
    {
        //방장이 씬을 로딩하면, 나머지 사람들도 자동 싱크
        PhotonNetwork.AutomaticallySyncScene = true;
        
        //게임 버전
        PhotonNetwork.GameVersion = gameVersion;
        
        /* 나중에 userId 설정하게 되면 수정 필요 */
        //서버 접속
        PhotonNetwork.ConnectUsingSettings();

    }

    //마스터 서버 연결 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("서버 연결 성공!");
    }

    //마스터 서버 연결 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("연결 중...");
        PhotonNetwork.ConnectUsingSettings();
    }

    //아이디 설정 
    public void IDConfirm()
    {
        AuthenticationValues authValues = new AuthenticationValues(userIDText.text);
        PhotonNetwork.AuthValues = authValues;
     
        //서버 접속
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장");
        //PhotonNetwork.FindFriends(new string[] { "1" });
    }


}
