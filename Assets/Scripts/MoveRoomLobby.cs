using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class MoveRoomLobby : MonoBehaviourPunCallbacks
{
    private bool show_roomlist;

    // Start is called before the first frame update
    void Start()
    {
        show_roomlist = false;
    }

    public void SetShowLobbyOn()
    {
        show_roomlist = true;
    }

    // Update is called once per frame
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("RoomLobby");
    }
}
