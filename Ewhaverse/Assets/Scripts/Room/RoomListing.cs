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
    [SerializeField]
    private GameObject roomPrefab;
    [SerializeField]
    private Transform scrollContent;

    public TMP_InputField roomNameText;

    public void CreateRoom()
    {
        Debug.Log("�� ���� ��...��ø� ��ٷ��ּ���");

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 10;
        ro.PublishUserId = true;

        //��ǲ�ʵ尡 ���������
        if (string.IsNullOrEmpty(roomNameText.text))
        {
            //���� �̸� �ο�
            roomNameText.text = $"ROOM_{Random.Range(1, 100):000}";
        }

        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

    // �뿡 �� �� ȣ��
    public override void OnJoinedRoom()
    {
        //Debug.Log("We load a " + currevent);
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        bool isActive = scrollContent.gameObject.activeSelf;

        if (!isActive)
            scrollContent.gameObject.SetActive(true);

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

        if (!isActive)
            scrollContent.gameObject.SetActive(false);
    }
}
