using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";   //버전

    public GameObject player_prefab;    //플레이어 프리팹
    public GameObject maincamera;       //메인 카메라
    public PlayerInfo playerinfo;

    private bool isinsquare;    //광장 위치 여부

    void Awake()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;    //방장이 씬을 로딩하면, 나머지 사람들도 자동 싱크

        isinsquare = true;  //첫 연결은 광장으로 연결해야하므로 true로 초기화

        //userID, 닉네임 설정
        if (!PhotonNetwork.IsConnected)
        {
            IDConfirm();
            NicknameConfirm();
        }

        if (PhotonNetwork.InLobby)
            EnterSqaure();
    }


    //마스터 서버 연결 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //마스터 서버 연결 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
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
        if (player_prefab != null && isinsquare)
        {
            Debug.Log("대광장 입장!");
            GameObject Player = (GameObject)PhotonNetwork.Instantiate(player_prefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
            Player.transform.Find("Camera").Find("MainCamera").gameObject.SetActive(true);
            maincamera.gameObject.SetActive(false);

            //Playerinfo.inlobby = true;      
            PlayerInfo.FindPlayerObject();
            //PlayerInfo.UpdateSquarePos();
            playerinfo.Avatar();
        }
    }

    public void LeaveSquare()
    {
        if (isinsquare)
            PhotonNetwork.LeaveRoom();

        isinsquare = false;
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("RoomLobby");
        maincamera.gameObject.SetActive(true);
    }
}