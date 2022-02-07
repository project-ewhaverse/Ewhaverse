using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class FriendListing : MonoBehaviourPunCallbacks
{
    private Dictionary<string, GameObject> friendDict = new Dictionary<string, GameObject>();
    private List<string> friendList = new List<string>();

    [SerializeField]
    private GameObject friendPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private InputField addFriend;

    public void AddFriend()
    {
        friendList.Add(addFriend.text);
        PhotonNetwork.FindFriends(friendList.ToArray());
    }

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

  
    public void SetActiveFriendList()
    {
        gameObject.transform.Find("FriendsList").gameObject.SetActive(true);
        //foreach (Transform child in gameObject.transform)
            //child.gameObject.SetActive(true);
        /*
        Component[] child = content.GetComponentsInChildren<Text>();
        foreach(var friend in child)
        {
            friend.GetComponent<FriendData>().showName();
            //Debug.Log(friend);
        }
        */
    }
    
}
