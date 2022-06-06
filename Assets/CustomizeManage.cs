using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class CustomizeManage : MonoBehaviourPunCallbacks
{ 
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}
