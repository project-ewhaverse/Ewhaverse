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

    //친구 목록 10초마다 업데이트
    private IEnumerator FriendUpdate()
    {
        while (true)
        {
            if(PhotonNetwork.InLobby && FriendUI.gameObject.activeSelf && friendList.Count != 0)
                PhotonNetwork.FindFriends(friendList.ToArray());
            yield return new WaitForSeconds(1.0f);
        }
    }

    //친구 추가
    public void AddFriend()
    {
        friendList.Add(addFriend.text);
        //PhotonNetwork.FindFriends(friendList.ToArray());
    }

    //친구 목록 업데이트시 호출
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
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
