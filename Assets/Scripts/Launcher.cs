using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";   //버전

    public GameObject player_prefab;

    private bool isinsquare;

    void Awake()
    {
        isinsquare = true;

        //방장이 씬을 로딩하면, 나머지 사람들도 자동 싱크
        PhotonNetwork.AutomaticallySyncScene = true;

        //게임 버전
        PhotonNetwork.GameVersion = gameVersion;


        //userID, 닉네임 설정
        if (!PhotonNetwork.IsConnected)
        {
            IDConfirm();
            NicknameConfirm();
        }
    }

    //마스터 서버 연결 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 연결 성공!");
        PhotonNetwork.JoinLobby();
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
        AuthenticationValues authValues = new AuthenticationValues(File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        PhotonNetwork.AuthValues = authValues;
        //서버 접속
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    //닉네임 설정
    public void NicknameConfirm()
    {
        PhotonNetwork.NickName = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 입장");

        if (isinsquare)
            EnterSqaure();
    }

    //대광장 입장
    public void EnterSqaure()
    {
        //대광장용 방
        RoomOptions square = new RoomOptions();
        square.IsOpen = true;
        square.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom("Square", square, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        //대광장 플레이어 생성
        if (player_prefab != null)
        {
            GameObject Player = (GameObject)PhotonNetwork.Instantiate(this.player_prefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
            Player.transform.parent = null;
            Player.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Player.transform.Find("Camera").Find("MainCamera").gameObject.SetActive(true);
        }

        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }

    public void LeaveSquare()
    {
        isinsquare = false;
        PhotonNetwork.LeaveRoom();
    }

    public void CreateRoom()
    {
        GameObject.Find("RoomListing").GetComponent<RoomListing>().CreateRoom();
    }
}