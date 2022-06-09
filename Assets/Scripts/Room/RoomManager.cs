using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.UI;
using TMPro;


public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; //아바타 프리팹

    public GameObject maincamera;   //메인 카메라
    public TMP_Text roomname;       //방 이름

    public Button DeleteRoom;       //방장일 때 방 삭제할 수 있는 버튼
    public Transform startlocation; //아바타 생성될 위치
    PhotonView photonview;          //RPC를 사용하기 위한 포톤 뷰
    public Motion motioncontroller;

    void Start()
    {
        photonview = PhotonView.Get(this);  //내 포톤뷰 가져오기

        Room ro = PhotonNetwork.CurrentRoom;    //현재 입장한 방 정보
        if (ro.Name == null)    
            roomname.text = "";
        roomname.text = ro.Name;    //방 이름 표기
        
        if (playerPrefab != null)
        {
            //룸 안에서 플레이어 인스턴스 나타냄
            GameObject Player = (GameObject)PhotonNetwork.Instantiate(this.playerPrefab.name, startlocation.position, Quaternion.identity, 0);
            Player.transform.Find("Camera").Find("MainCamera").gameObject.SetActive(true);
            maincamera.gameObject.SetActive(false);
            motioncontroller.animator = Player.transform.Find("avatar").GetComponent<Animator>();
        }

        //방장이라면
        if(PhotonNetwork.IsMasterClient)
        {
            DeleteRoom.gameObject.SetActive(true);  //방 삭제 버튼 활성화
        }
    }

    //Master Client가 방장. 방장이 퇴장하여 새로운 방장으로 바뀌었을 때
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)           //새로운 방장 화면에
        {
            DeleteRoom.gameObject.SetActive(true);  //방 삭제 버튼 활성화
        }
    }  

    [PunRPC]
    //방을 나가는 메소드
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //방 삭제 >> 모두 방 나가기를 RPC로 실행
    public void LeaveAll()
    {
            photonview.RPC("LeaveRoom", RpcTarget.All);   
    }

    //방을 나갈 때 실행되는 콜백
    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene("RoomLobby");  //로비 로드
        PhotonNetwork.LoadLevel("RoomLobby");
        maincamera.gameObject.SetActive(true);
    }
}
