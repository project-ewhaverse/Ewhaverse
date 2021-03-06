using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class VideoController : MonoBehaviour
{
    VideoPlayer videoPlayer;
    static string time = "01:00" ;
    static double runningTime = 24*60;

    DateTime comp = Convert.ToDateTime(time);
    double diffsec;
    float delay = 0.3f;
    bool on = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }


    public void onClickPlay()
    {
        if (on)
        {
            videoPlayer.Stop();
            on = false;
        }
        else
        {
            print("상영 시간 :" + time + "~ (" + runningTime + "분)");
            TimeSpan diff = DateTime.Now - comp;
            diffsec = diff.TotalSeconds;
            if (diffsec >= 0 && diffsec < runningTime * 60)
            {
                videoPlayer.Play();
                Invoke("sync", delay);
                sync();
                print(runningTime);
                on = true;
            }
        }

    }
    void sync()
    {
        videoPlayer.frame = Convert.ToInt64((diffsec + delay) * videoPlayer.frameRate);
    }
}