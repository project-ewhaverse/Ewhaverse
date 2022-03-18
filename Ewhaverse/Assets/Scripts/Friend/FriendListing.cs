using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class FriendListing : MonoBehaviourPunCallbacks
{
    private Dictionary<string, GameObject> friendDict = new Dictionary<string, GameObject>();
    private List<string> friendList = new List<string>();

    [SerializeField]
    private GameObject FriendUI;
    [SerializeField]
    private GameObject friendPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TMP_InputField addFriend;

    private void Awake()
    {
        StartCoroutine(FriendUpdate());
    }

    //ģ�� ��� 10�ʸ��� ������Ʈ
    private IEnumerator FriendUpdate()
    {
        while (true)
        {
            if(PhotonNetwork.InLobby && FriendUI.gameObject.activeSelf)
                PhotonNetwork.FindFriends(friendList.ToArray());
            yield return new WaitForSeconds(10.0f);
        }
    }

    //ģ�� �߰�
    public void AddFriend()
    {
        friendList.Add(addFriend.text);
        PhotonNetwork.FindFriends(friendList.ToArray());
    }

    //ģ�� ��� ������Ʈ�� ȣ��
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        base.OnFriendListUpdate(friendList);
        /*
        Debug.Log(friendList.Count);
        for(int i = 0; i < friendList.Count; i++)
        {
            FriendInfo friend = friendList[i];
            Debug.LogFormat("{0}", friend);
        }
        */

        foreach (var friend in friendList)
        {
            
            GameObject tempFriend;
            if(!friendDict.ContainsKey(friend.UserId))
            {
                GameObject _friend = Instantiate(friendPrefab, content);
                _friend.GetComponent<FriendData>().FriendInfo = friend;
                _friend.GetComponent<FriendData>().showName();
                friendDict.Add(friend.UserId, _friend);
            }
            else
            {
                friendDict.TryGetValue(friend.UserId, out tempFriend);
                tempFriend.GetComponent<FriendData>().FriendInfo = friend;
                tempFriend.GetComponent<FriendData>().showName();
            }
            
            Debug.Log("Friend info received " + friend.UserId + " is online? " + friend.IsOnline);
        }      
    }

  
    public void SetActiveFriend()
    {
        if (FriendUI.gameObject.activeSelf)
            FriendUI.gameObject.SetActive(false);
        else
            FriendUI.gameObject.SetActive(true);
    }
    
}
