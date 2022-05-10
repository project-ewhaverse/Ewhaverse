using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class RoomLobby : MonoBehaviour
{
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //대광장 되돌아가기 버튼 함수
    public void BackToSquare()
    {
        PhotonNetwork.LoadLevel("Square");
    }
}
