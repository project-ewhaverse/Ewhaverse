using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class UI : MonoBehaviour
{
    public Recorder recorder;

    private void OnEnable()
    {
        toggleon.ToggleValueChanged += this.BetterToggle_ToggleValueChanged;
    }
    private void OnDisable()
    {
        toggleon.ToggleValueChanged -= this.BetterToggle_ToggleValueChanged;
    }

    private void BetterToggle_ToggleValueChanged(Toggle toggle)
    {
        switch (toggle.name)
        {
            case "VoiceChat":
                if (this.recorder)
                {
                    this.recorder.TransmitEnabled = toggle.isOn;
                }
                break;
        }
    }

}
