using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*내 정보를 저장하는 클래스*/
public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public static PlayerInfo info;

    GameObject my_player;   //내 플레이어
    GameObject child;

    bool inlobby;   //로비

    private float pos_x;    //transform.poition
    private float pos_y;
    private float pos_z;

    private float rot;

    //아바타 정보
    AvatarInfo avatarinfo = new AvatarInfo();


    int front;  //delete
    int back;   //delete

    /*싱글턴 사용*/
    private void Awake()
    {
        if (info != null)
        {
            Destroy(gameObject);
            return;
        }
        info = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        inlobby = true;
        pos_x = 0f;
        pos_y = 2f;
        pos_z = 0f;
        rot = 0f;

        ReadAvatarInfo();
        //front = Random.Range(0, 5);   //delete
        //back = Random.Range(0, 5);    //delete
    }

    /*
    // Update is called once per frame
    void Update()
    {

        if (PhotonNetwork.InLobby)
            inlobby = true;
        else if(PhotonNetwork.InRoom)
        { 
            if(PhotonNetwork.CurrentRoom.Name == "Square")
            {
                if(my_player != null)
                {
                    pos_x = my_player.transform.position.x;
                    pos_y = my_player.transform.position.y;
                    pos_z = my_player.transform.position.z;
                    rot = child.transform.rotation.eulerAngles.y;
                }                
            }
            else
            {
                inlobby = false;
                pos_x = 0f;
                pos_y = 2f;
                pos_z = 0f;
            }
        }
    }
    */

    public void FindPlayerObject()
    {
        my_player = GameObject.Find(PhotonNetwork.AuthValues.UserId).gameObject;
        child = my_player.transform.Find("avatar").gameObject;
    }

    public void UpdateSquarePos()
    {
        //대광장 >> 로비 >> 대광장
        if(inlobby && PhotonNetwork.CurrentRoom.Name == "Square")
        {        
            Debug.Log("대광장 >> 로비 >> 대광장");
            my_player.transform.position = new Vector3(pos_x, pos_y, pos_z);
            child.transform.rotation = Quaternion.Euler(0, rot, 0);
            Debug.LogFormat("{0}, {1}, {2}", pos_x, pos_y, pos_z);
            inlobby = false;
        }
    }

    public (int, int) Get() //delete
    {
        return (front, back);
    }

    public void ReadAvatarInfo()
    {
        string result = File.ReadAllText(Application.persistentDataPath + "/CustomJson.txt");
        string[] info = result.Split(new string[] { "," }, StringSplitOptions.None);
        
        //피부색
        String[] skincolor = info[0].Split(new string[] { "/" }, StringSplitOptions.None);
        avatarinfo.skin.r = float.Parse(skincolor[0]);
        avatarinfo.skin.g = float.Parse(skincolor[1]);
        avatarinfo.skin.b = float.Parse(skincolor[2]);
        
        //눈
        String[] eyecolor = info[2].Split(new string[] { "/" }, StringSplitOptions.None);
        avatarinfo.eye.type = int.Parse(info[1].Replace("Eye", ""));
        avatarinfo.eye.r = float.Parse(eyecolor[0]);
        avatarinfo.eye.g = float.Parse(eyecolor[1]);
        avatarinfo.eye.b = float.Parse(eyecolor[2]);
        
        //입
        avatarinfo.mouse.type = int.Parse(info[3].Replace("Mou", ""));

        //머리
        avatarinfo.hair.front_type = int.Parse(info[4].Replace("HaF", ""));
        avatarinfo.hair.back_type = int.Parse(info[4].Replace("HaB", ""));  
        String[] haircolor = info[4].Split(new string[] { "/" }, StringSplitOptions.None);
        avatarinfo.hair.r = float.Parse(haircolor[0]);
        avatarinfo.hair.g = float.Parse(haircolor[1]);
        avatarinfo.hair.b = float.Parse(haircolor[2]);
        
        //옷
        avatarinfo.cloth.top = int.Parse(info[6].Replace("Top", ""));
        avatarinfo.cloth.bottom = int.Parse(info[7].Replace("Bot", ""));
        avatarinfo.cloth.shoes = int.Parse(info[8].Replace("Sho", ""));
        avatarinfo.cloth.acc = int.Parse(info[6].Replace("Ace", ""));
    }
}
