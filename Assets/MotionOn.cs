using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionOn : MonoBehaviour
{

    public void MotionEnable()
    {
        gameObject.SetActive(!gameObject.activeSelf);

    }
}
