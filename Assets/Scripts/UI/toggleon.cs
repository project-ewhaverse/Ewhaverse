using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleon : MonoBehaviour
{
    private Toggle toggle;

    public delegate void OnToggle(Toggle toggle);

    public static event OnToggle ToggleValueChanged;

    private void Start()
    {
        this.toggle = this.GetComponent<Toggle>();
        this.toggle.onValueChanged.AddListener(delegate { this.OnToggleValueChanged(); });
    }

    public void OnToggleValueChanged()
    {
        if (ToggleValueChanged != null)
        {
            ToggleValueChanged(this.toggle);
        }
    }
}
