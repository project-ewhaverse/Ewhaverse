using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class RoomData : MonoBehaviour
{
    private Text RoomInfoText;
    private RoomInfo _roomInfo;

    public InputField userIDText;

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // EX : room_03 (1/2)
            RoomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            //버튼의 클릭 이벤트에 함수를 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
    }

    private void Awake()
    {
        RoomInfoText = GetComponentInChildren<Text>();
        userIDText = GameObject.Find("userId").GetComponent<InputField>();

    }
    
    void OnEnterRoom(string roomName)
    {
        RoomOptions room = new RoomOptions();
        room.IsOpen = true;
        room.IsVisible = true;
        room.MaxPlayers = 10;

        PhotonNetwork.NickName = userIDText.text;
        PhotonNetwork.JoinOrCreateRoom(roomName, room, TypedLobby.Default);
    }
}
