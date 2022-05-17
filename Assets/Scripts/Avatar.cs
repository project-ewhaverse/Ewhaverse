using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    [Header("Hair")]
    [SerializeField] private Mesh[] hairs;

    //public GameObject avatar_item;
    
    private SkinnedMeshRenderer hair;

    private void Start()
    {
        hair = transform.Find("hair-back01").gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    public void chanege()
    {
        hair.sharedMesh = hairs[Random.Range(0, hairs.Length)];
    }
}
