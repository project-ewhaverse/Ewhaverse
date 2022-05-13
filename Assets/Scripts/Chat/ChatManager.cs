using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    private PhotonView photonView;
    public InputField chatInput;

    public GameObject textBox;
    public Text boxText;

    void Awake()
    {
        photonView = PhotonView.Get(this);
        chatInput = GameObject.Find("ChatUI").transform.Find("InputField_Chat").GetComponent<InputField>();
    }

    void Start()
    {
        chatInput.onEndEdit.AddListener(delegate { synchTextBubble(chatInput);});
    }


    void synchTextBubble(InputField input)
	{
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            if (input.text != "")            
            {
                photonView.RPC("send", RpcTarget.All, input.text);
                textBox.SetActive(true);
            }
        }
    }

    [PunRPC]
    void send(string input)
	{
        boxText.text = input;
        textBox.SetActive(true);

        CancelInvoke();
        Invoke("disappear", 4.0f);

    }

    void disappear()
	{
        textBox.SetActive(false);
	}

    /*
     void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(textBox.active);
            stream.SendNext(boxText);
        }
        else if (stream.IsReading)
        {
            textBox.SetActive((bool)stream.ReceiveNext());
            boxText.text = (string)stream.ReceiveNext();
        }
    }
    */

}
