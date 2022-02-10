using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

using UnityEngine.UI;
using TMPro;

public class RoomListing : MonoBehaviourPunCallbacks
{
    //룸 목록 저장
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    //룸 표시 프리팹 & 프리팹이 차일드화시킬 부모 객체
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    private Transform scrollContent;

    public TMP_InputField roomNameText;

    public void CreateRoom()
    {
        Debug.Log("방 생성 중...잠시만 기다려주세요");

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 10;
        ro.PublishUserId = true;

        //인풋필드가 비어있으면
        if (string.IsNullOrEmpty(roomNameText.text))
        {
            //랜덤 이름 부여
            roomNameText.text = $"ROOM_{Random.Range(1, 100):000}";
        }

        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

    // 룸에 들어갈 때 호출
    public override void OnJoinedRoom()
    {
        //Debug.Log("We load a " + currevent);
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach (var room in roomList)
        {
            //룸이 삭제된 경우
            if (room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            //룸 정보가 변경된 경우
            else
            {
                //룸 정보 처음 생성
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }
                //룸 정보 갱신
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }
}
