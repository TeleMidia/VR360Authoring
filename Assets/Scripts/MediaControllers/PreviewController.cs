using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;

public class PreviewController : MediaControllerAbstract
{    
    private float startpreview;
    private float stoppreview;
    public override void Load()
    {
        GetComponent<VideoPlayer>().url = on_select_object.GetComponent<VideoPlayer>().url;
        GetComponent<VideoPlayer>().Prepare();
        //Debug.Log("Quantidade: " + numbers.Length);
        
        startpreview = clipBegin;
        stoppreview = clipEnd;
        //Debug.Log("Start: "+startpreview+" Stop: "+stoppreview);
        GetComponent<VideoPlayer>().time = startpreview;
    }

    public override void PlayMedia()
    {
        PlayVideoPreview();
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void PlayVideoPreview()
    {
        GetComponent<VideoPlayer>().Prepare();        
        GetComponent<VideoPlayer>().Play();
        Invoke("PauseVideoPreview", stoppreview - startpreview);
    }
    private void PauseVideoPreview()
    {
        GetComponent<VideoPlayer>().Pause();
        GetComponent<VideoPlayer>().time = startpreview;
        PlayVideoPreview();
    }

    public override void StopMedia()
    {
        //CancelInvoke();
        GetComponent<VideoPlayer>().Stop();
        GetComponent<MeshRenderer>().enabled = false;
    }
}
