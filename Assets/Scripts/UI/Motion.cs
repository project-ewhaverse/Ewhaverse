using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Motion : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        MotionButton.ButtonClicked += this.MotionStateChanged;
    }
    private void OnDisable()
    {
        MotionButton.ButtonClicked -= this.MotionStateChanged;
    }




    private void MotionStateChanged(int num)
    {
        animator.SetInteger("motion", num);
    }
}
