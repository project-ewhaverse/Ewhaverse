using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if(hit.collider.name == "cube")
                {
                    Vector3 mousePos = Input.mousePosition + new Vector3(10.0f, 10.0f, 0);
                    panel.SetActive(true);
                    panel.transform.position = mousePos;
                }
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    void RequestFriend()
    {

    }


    
}

   
