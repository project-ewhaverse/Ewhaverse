using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";   //버전

    public GameObject player_prefab;    //플레이어 프리팹
    public GameObject maincamera;       //메인 카메라
    public Motion motioncontroller;


    void Awake()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;    //방장이 씬을 로딩하면, 나머지 사람들도 자동 싱크

        //userID, 닉네임 설정
        if (!PhotonNetwork.IsConnected)
        {
            IDConfirm();
            NicknameConfirm();
        }

        if (PhotonNetwork.InLobby)
            EnterSqaure();
        Debug.Log(PhotonNetwork.InLobby);
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
            Debug.Log("대광장 입장!");
            GameObject Player = (GameObject)PhotonNetwork.Instantiate(player_prefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
            Player.transform.Find("Camera").Find("MainCamera").gameObject.SetActive(true);
            maincamera.gameObject.SetActive(false);
            motioncontroller.animator = Player.transform.Find("avatar").GetComponent<Animator>();

            PlayerInfo.FindPlayerObject();
            if (PlayerInfo.is_customizing)
            {
                PlayerInfo.UpdateSquarePos();
                PlayerInfo.is_customizing = false;
            }
        }
    }

    public void LeaveSquare()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        maincamera.gameObject.SetActive(true);
        
        if (PlayerInfo.is_customizing)
            SceneManager.LoadScene("Customize");
        else
            SceneManager.LoadScene("RoomLobby");
        
    }

    //커스터마이징
    public void CustomBtnClicked()
    {
        PlayerInfo.is_customizing = true;
        PlayerInfo.PresentLoca();
        LeaveSquare();
    }
}