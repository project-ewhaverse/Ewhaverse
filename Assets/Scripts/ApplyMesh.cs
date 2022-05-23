using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ApplyMesh : MonoBehaviour
{
    PhotonView photonview;
    PlayerInfo info;

    [Header("Avatar Mesh")]
    [SerializeField] private SkinnedMeshRenderer mesh_eye;
    [SerializeField] private SkinnedMeshRenderer mesh_hair_front;
    [SerializeField] private SkinnedMeshRenderer mesh_hair_back;
    //[SerializeField] private SkinnedMeshRenderer mesh_mouse;
    [SerializeField] private SkinnedMeshRenderer mesh_top;
    [SerializeField] private SkinnedMeshRenderer mesh_bottom;
    [SerializeField] private SkinnedMeshRenderer mesh_shoes;
    //[SerializeField] private SkinnedMeshRenderer mesh_acc;

    [Header("Mesh")]
    [SerializeField] private Mesh[] eye;
    [SerializeField] private Mesh[] hair_front;
    [SerializeField] private Mesh[] hair_back;
    //[SerializeField] private Mesh[] mouse;
    [SerializeField] private Mesh[] top;
    [SerializeField] private Mesh[] bottom;
    [SerializeField] private Mesh[] shoes;
    //[SerializeField] private Mesh[] acc;



    void Start()
    {
        photonview = PhotonView.Get(this);
        info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        info.ReadAvatarInfo();
        Avatar();
    }

    public void Avatar()
    {
        if (!photonview.IsMine)
            return;

        AvatarInfo avatar = info.avatarinfo;
        //밑의 것처럼 쓰려면 직렬화 등록 필요 --> 나중에
        //photonview.RPC("Apply", RpcTarget.AllBuffered, avatar); 
        photonview.RPC("Apply", RpcTarget.AllBuffered, 
            avatar.eye.type, avatar.hair.front_type, avatar.hair.back_type, avatar.cloth.top, avatar.cloth.bottom, avatar.cloth.shoes);

        //(front_idx, back_idx) = info.Get();
        //Debug.Log((front_idx, back_idx));
        //photonview.RPC("Apply", RpcTarget.AllBuffered, front_idx, back_idx);
    }

    [PunRPC]
    //void Apply(AvatarInfo avatar)
    void Apply(int e, int f, int b, int t, int bot, int s)
    {
        //피부색

        //눈
        //mesh_eye.sharedMesh = eye[avatar.eye.type];
        mesh_eye.sharedMesh = eye[e];

        //색

        //입
        //mesh_mouse.sharedMesh = mouse[avatar.mouse.type];

        //머리
        //mesh_hair_front.sharedMesh = hair_front[avatar.hair.front_type];
        //mesh_hair_back.sharedMesh = hair_back[avatar.hair.back_type];
        mesh_hair_front.sharedMesh = hair_front[f];
        mesh_hair_back.sharedMesh = hair_back[b];

        //옷
        //mesh_top.sharedMesh = top[avatar.cloth.top];
        //mesh_bottom.sharedMesh = top[avatar.cloth.bottom];
        //mesh_shoes.sharedMesh = top[avatar.cloth.shoes];
        //mesh_acc.sharedMesh = top[avatar.cloth.acc];
        mesh_top.sharedMesh = top[t];
        mesh_bottom.sharedMesh = bottom[bot];
        mesh_shoes.sharedMesh = shoes[s];

        Debug.Log(e);
        Debug.Log(f);
        Debug.Log(b);
        Debug.Log(t);
        Debug.Log(bot);
        Debug.Log(s);
    }
}
