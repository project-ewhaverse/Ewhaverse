using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class UI : MonoBehaviour
{
    public Recorder recorder;

    /*
    public Button voiceSwitch;
    private PhotonVoiceNetwork punVoiceNetwork; 
    
    private void Awake()
    {
        this.punVoiceNetwork = PhotonVoiceNetwork.Instance;
    }

     */





    private void OnEnable()
    {
        toggleon.ToggleValueChanged += this.BetterToggle_ToggleValueChanged;
    }
    private void OnDisable()
    {
        toggleon.ToggleValueChanged -= this.BetterToggle_ToggleValueChanged;
    }

    /*
    private void Start()
	{
        if (this.voiceSwitch != null) { 
            this.voiceSwitch.onClick.AddListener(this.VoiceSwitchOnClick);
        }

            
    }     
     */



    private void BetterToggle_ToggleValueChanged(Toggle toggle)
    {
        switch (toggle.name)
        {
            case "VoiceChat":
                if (this.recorder)
                {
                    this.recorder.TransmitEnabled = !toggle.isOn;
                }
                break;
        }
    }
    /*
    private void VoiceSwitchOnClick()
    {
        if (this.punVoiceNetwork.ClientState == Photon.Realtime.ClientState.Joined)
        {
            this.punVoiceNetwork.Disconnect();
            print("Voice Disconnected");
        } 
        else if (this.punVoiceNetwork.ClientState == Photon.Realtime.ClientState.PeerCreated
                 || this.punVoiceNetwork.ClientState == Photon.Realtime.ClientState.Disconnected)
        {
            this.punVoiceNetwork.ConnectAndJoinRoom();
            print("Voice Connected");
        }
    }
     */


}
