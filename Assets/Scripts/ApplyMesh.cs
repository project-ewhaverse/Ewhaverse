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
        photonview.RPC("Apply", RpcTarget.AllBuffered, avatar);
        
        //(front_idx, back_idx) = info.Get();
        //Debug.Log((front_idx, back_idx));
        //photonview.RPC("Apply", RpcTarget.AllBuffered, front_idx, back_idx);
    }

    [PunRPC]
    void Apply(AvatarInfo avatar)
    {
        //ÇÇºÎ»ö

        //´«
        mesh_eye.sharedMesh = eye[avatar.eye.type];
        //»ö

        //ÀÔ
        //mesh_mouse.sharedMesh = mouse[avatar.mouse.type];

        //¸Ó¸®
        mesh_hair_front.sharedMesh = hair_front[avatar.hair.front_type];
        mesh_hair_back.sharedMesh = hair_back[avatar.hair.back_type];

        //¿Ê
        mesh_top.sharedMesh = top[avatar.cloth.top];
        mesh_bottom.sharedMesh = top[avatar.cloth.bottom];
        mesh_shoes.sharedMesh = top[avatar.cloth.shoes];
        //mesh_acc.sharedMesh = top[avatar.cloth.acc];

        Debug.Log(avatar.eye.type);
        Debug.Log(avatar.mouse.type);
        Debug.Log(avatar.hair.front_type);
        Debug.Log(avatar.hair.back_type);
        Debug.Log(avatar.cloth.top);
        Debug.Log(avatar.cloth.bottom);
    }
}
