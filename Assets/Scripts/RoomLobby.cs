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

    //�뱤�� �ǵ��ư��� ��ư �Լ�
    public void BackToSquare()
    {
        PhotonNetwork.LoadLevel("Square");
    }
}
