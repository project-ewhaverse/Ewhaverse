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
    [SerializeField]    private GameObject LoomListView;
    [SerializeField]    private GameObject roomPrefab;
    [SerializeField]    private Transform scrollContent;

    //룸 생성 설정
    public TMP_InputField roomname_text;
    public ToggleGroup room_theme;
    private string theme_scene;

    //룸 테마 선택
    public void SelectTheme()
    {
        IEnumerable<Toggle> theme_selected = room_theme.ActiveToggles();
        foreach (Toggle t in theme_selected)
        {
            theme_scene = t.name;
            Debug.Log(theme_scene);
        }
    }

    /*룸 생성*/
    public void CreateRoom()
    {
        Debug.Log("방 생성 중...잠시만 기다려주세요");

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 10;
        ro.PublishUserId = true;

        //인풋필드가 비어있으면
        if (string.IsNullOrEmpty(roomname_text.text))
        {
            //랜덤 이름 부여
            roomname_text.text = $"ROOM_{Random.Range(1, 100):000}";
        }

        PhotonNetwork.CreateRoom(roomname_text.text, ro);
    }

    /*룸에 들어갈 때 호출*/
    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("Room");
        if (string.IsNullOrEmpty(theme_scene))
            PhotonNetwork.LoadLevel("ClassRoom");
        else 
            PhotonNetwork.LoadLevel(theme_scene);
    }

    /*룸 목록이 업데이트될 때 호출*/
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        bool check = true;

        if (LoomListView.gameObject.activeSelf)
            check = false;

        if(check)
        {
            LoomListView.gameObject.SetActive(true);
        }
            
        
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

        if(check)
        {
            LoomListView.gameObject.SetActive(false);
        }
    }
}
