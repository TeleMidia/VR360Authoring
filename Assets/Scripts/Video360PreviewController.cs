using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video360PreviewController: MediaControllerAbstract
{    

    public override void Load()
    {
        GetComponent<VideoPlayer>().url = controller.GetComponent<VideoPlayer>().url;
        GetComponent<VideoPlayer>().Play();
        GetComponent<VideoPlayer>().Pause();
    }

    public override void PlayMedia()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }
}
