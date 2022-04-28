using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverThePlayer : MonoBehaviour
{
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        this.canvas = this.GetComponent<Canvas>();
    }

    private void LateUpdate()
    {
        if (this.canvas.worldCamera == null) { this.canvas.worldCamera = Camera.main; return; } 
        // 타인의 시선에서 봤을때 해당 카메라의 각도와 무관하게 내 위의 아이콘이 정면으로 보이게끔 transform시킴
        this.transform.rotation = Quaternion.Euler(0f, this.canvas.worldCamera.transform.eulerAngles.y, 0f); //canvas.worldCamera.transform.rotation;

    }
}
