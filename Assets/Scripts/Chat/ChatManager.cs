using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ChatManager : MonoBehaviour
{
    public PhotonView photonView;
    public GameObject textBox;
    public Text boxText;

    public InputField chatInput;

    void Awake()
    {
        photonView = PhotonView.Get(this);
        chatInput = GameObject.Find("InputField_Chat").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
		if (photonView.IsMine)
		{
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) 
			{
                photonView.RPC("send", RpcTarget.All, chatInput.text);
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

        chatInput.text = "";
        chatInput.Select();
        chatInput.ActivateInputField();
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
