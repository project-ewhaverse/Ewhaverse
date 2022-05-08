using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MouseInteraction : MonoBehaviour
{
    public GameObject panel;
    //public RectTransform panel_transform;

    private void Update()
    {
        GetMouseInput();
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                PhotonView pv = hit.collider.gameObject.GetPhotonView();
                if(hit.collider.name == "Player" && !pv.IsMine)
                {
                    Vector3 mousePos = Input.mousePosition;
                    panel.SetActive(true);
                    panel.transform.position = mousePos;
                }
                else
                {
                    panel.SetActive(false);
                }
            }
            else
            {
                panel.SetActive(false);
            }
            
        }
        else if(Input.GetMouseButtonUp(0))  //���߿� ���� �ʿ�
        {
            panel.SetActive(false);
        }
    }

    

    //ģ�� ��û ��ư ����
    public void RequestFriend(Player player)
    {
        string userid = player.UserId;
        Debug.Log(userid);
    }

    //1:1 �޽��� ��û ��ư ����
}

   
