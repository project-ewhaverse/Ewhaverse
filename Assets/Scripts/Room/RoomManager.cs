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
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public TMP_Text roomname;

    public Button DeleteRoom;

    PhotonView photonview;

    void Start()
    {
        photonview = PhotonView.Get(this);

        Room ro = PhotonNetwork.CurrentRoom;
        if (ro.Name == null)
            roomname.text = "";
        roomname.text = ro.Name;
        
        if (playerPrefab == null)
        {
            Debug.LogError("< Color = Red >< a > Missing </ a ></ Color > playerPrefab Reference.Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            //룸 안에서 플레이어 인스턴스 나타냄
            GameObject Player = (GameObject)PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            Player.transform.parent = null;
            Player.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Player.transform.Find("Camera").Find("MainCamera").gameObject.SetActive(true);
        }

        if(PhotonNetwork.IsMasterClient)
        {
            DeleteRoom.gameObject.SetActive(true);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            DeleteRoom.gameObject.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Square");  //0번 씬(로비) 로드
        //PhotonNetwork.JoinLobby();
        //PhotonNetwork.LoadLevel("Lobby");
    }

    [PunRPC]
    //방을 나가는 메소드
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("나가기");
    }

    public void LeaveAll()
    {
            photonview.RPC("LeaveRoom", RpcTarget.All);   
    }

}
