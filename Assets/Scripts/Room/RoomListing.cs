using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

using UnityEngine.UI;
using TMPro;

public class RoomListing : MonoBehaviourPunCallbacks
{
    //�� ��� ����
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    //�� ǥ�� ������ & �������� ���ϵ�ȭ��ų �θ� ��ü
    [SerializeField]    private GameObject LoomListView;
    [SerializeField]    private GameObject roomPrefab;
    [SerializeField]    private Transform scrollContent;

    //�� ���� ����
    public TMP_InputField roomname_text;
    public ToggleGroup room_theme;
    private string theme_scene;

    //�� �׸� ����
    public void SelectTheme()
    {
        IEnumerable<Toggle> theme_selected = room_theme.ActiveToggles();
        foreach (Toggle t in theme_selected)
        {
            theme_scene = t.name;
            Debug.Log(theme_scene);
        }
    }

    /*�� ����*/
    public void CreateRoom()
    {
        Debug.Log("�� ���� ��...��ø� ��ٷ��ּ���");

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 10;
        ro.PublishUserId = true;

        //��ǲ�ʵ尡 ���������
        if (string.IsNullOrEmpty(roomname_text.text))
        {
            //���� �̸� �ο�
            roomname_text.text = $"ROOM_{Random.Range(1, 100):000}";
        }

        PhotonNetwork.CreateRoom(roomname_text.text, ro);
    }

    /*�뿡 �� �� ȣ��*/
    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("Room");
        if (string.IsNullOrEmpty(theme_scene))
            PhotonNetwork.LoadLevel("ClassRoom");
        else 
            PhotonNetwork.LoadLevel(theme_scene);
    }

    /*�� ����� ������Ʈ�� �� ȣ��*/
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
            //���� ������ ���
            if (room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            //�� ������ ����� ���
            else
            {
                //�� ���� ó�� ����
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }
                //�� ���� ����
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
