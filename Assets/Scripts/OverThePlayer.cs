using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.Unity;
using Photon.Voice.PUN;

public class OverThePlayer : MonoBehaviour
{
    private Canvas canvas;
    public Image recorderSprite; // 말할때 내 위에 뜨는 이미지
    public Image speakerSprite; // 나에게 들릴때 상대방 위에 뜨는 이미지
    public PhotonVoiceView photonVoiceView;

    // Start is called before the first frame update
    void Awake()
    {
        this.canvas = this.GetComponent<Canvas>();        
    }
    void Start()
	{
        this.canvas.worldCamera = Camera.main;
    }
    private void Update()
    {
        //this.recorderSprite.enabled = this.photonVoiceView.IsRecording; // 내가 말하는 중일때 아이콘 띄우기
        this.speakerSprite.enabled = this.photonVoiceView.IsSpeaking; // 상대방으로서 말하는 중일때 아이콘 띄우기
    }
    private void LateUpdate()
    {
        //if (this.canvas.worldCamera == null) { this.canvas.worldCamera = Camera.main; return; } 
        // 타인의 시선에서 봤을때 해당 카메라의 각도와 무관하게 내 위의 아이콘이 정면으로 보이게끔 transform시킴
        this.transform.rotation = Quaternion.Euler(this.canvas.worldCamera.transform.eulerAngles.x, this.canvas.worldCamera.transform.eulerAngles.y, 0f); //canvas.worldCamera.transform.rotation;

    }
}
