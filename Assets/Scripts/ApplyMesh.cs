using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ApplyMesh : MonoBehaviour
{
    PhotonView photonview;
    PlayerInfo info;

    //mesh
    public SkinnedMeshRenderer mesh_eye;
    public SkinnedMeshRenderer mesh_hair_front;
    public SkinnedMeshRenderer mesh_hair_back;
    public SkinnedMeshRenderer mesh_top;
    public SkinnedMeshRenderer mesh_bottom;
    public SkinnedMeshRenderer mesh_shoes;
    public SkinnedMeshRenderer mesh_acc;

    int front_idx;
    int back_idx;

    [Header("Avatar")]
    [SerializeField] private Mesh[] eye;
    [SerializeField] private Mesh[] hair_front;
    [SerializeField] private Mesh[] hair_back;
    [SerializeField] private Mesh[] top;
    [SerializeField] private Mesh[] bottom;
    [SerializeField] private Mesh[] shoes;
    [SerializeField] private Mesh[] acc;



    void Start()
    {
        photonview = PhotonView.Get(this);
        info = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        Avatar();
    }

    public void Avatar()
    {
        if (!photonview.IsMine)
            return;

        (front_idx, back_idx) = info.Get();
        Debug.Log((front_idx, back_idx));
        photonview.RPC("Apply", RpcTarget.AllBuffered, front_idx, back_idx);
    }

    [PunRPC]
    void Apply(int f, int b)
    {
        mesh_hair_front.sharedMesh = hair_front[f];
        mesh_hair_back.sharedMesh = hair_back[b];
    }
}
