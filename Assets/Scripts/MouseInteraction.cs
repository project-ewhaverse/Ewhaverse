using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
public class MouseInteraction : MonoBehaviour
{
    public GameObject panel;
    private string othername;
    //public RectTransform panel_transform;
    [SerializeField] string url1, url2;
    [SerializeField] GameObject FoneUI, FtwoUI;
    [SerializeField] GameObject MsgUI;
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
                if(hit.collider.tag == "Player" && !hit.collider.gameObject.GetPhotonView().IsMine)
                {
                    othername = hit.collider.name;
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
        else if(Input.GetMouseButtonUp(0))  //나중에 수정 필요
        {
            panel.SetActive(false);
        }
    }
    public void RequestFriend()
    {
        StartCoroutine(FriendCoroutine("reqadd", othername));
        FoneUI.gameObject.SetActive(true);
        FtwoUI.gameObject.SetActive(true);
    }
    IEnumerator FriendCoroutine(string command, string othername)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", command);
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("id2", othername);
        UnityWebRequest www = UnityWebRequest.Post(url1, form);
        yield return www.SendWebRequest();
        string result = UnityWebRequest.UnEscapeURL(www.downloadHandler.text);
        print(result);
    }
    public void RequestMessage()
    {
        string id1 = File.ReadAllText(Application.persistentDataPath + "/Sync.txt");
        string id2 = othername;
        if (id1 != id2)
        {
            StartCoroutine(MessageCoroutine(id2));
            File.WriteAllText(Application.persistentDataPath + "/SyncM1.txt", id2);
            MsgUI.gameObject.SetActive(true);
        }
    }
    IEnumerator MessageCoroutine(string id2)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "m1add");
        form.AddField("id1", File.ReadAllText(Application.persistentDataPath + "/Sync.txt"));
        form.AddField("id2", id2);
        UnityWebRequest www = UnityWebRequest.Post(url2, form);
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        print(result);
    }
}

   
