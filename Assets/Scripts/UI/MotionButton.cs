using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionButton : MonoBehaviour
{
    private Button button;
    public int num;

    public delegate void ButtonClick(int num);
    public static event ButtonClick ButtonClicked;

    void Start()
    {
        this.button = this.GetComponent<Button>();
        this.button.onClick.AddListener(delegate { this.OnButtonClick(); });
    }

    public void OnButtonClick()
    {
        if (ButtonClicked != null)
        {
            ButtonClicked(num);
        }
    }
}
