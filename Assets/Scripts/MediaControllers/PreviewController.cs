using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Inherits MediaControllerAbstract. This class is the script that controls the preview media object.
/// </summary>
public class PreviewController : MediaControllerAbstract
{   
    //time of the clipBegin. The time that the segment that is played in the preview starts 
    private float startpreview;
    //time of the clipEnd. The time that the segment that is played in the preview ends 
    private float stoppreview;

    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  It loads the video on the video player from the url, prepare it and seek it to the time of startPreview
    /// </summary>
    public override void Load()
    {
        GetComponent<VideoPlayer>().url = on_select_object.GetComponent<VideoPlayer>().url;
        GetComponent<VideoPlayer>().Prepare();
        
        startpreview = clipBegin;
        stoppreview = clipEnd;
        GetComponent<VideoPlayer>().time = startpreview;
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Playing for the preview consists in starting the loop of the preview and enabling its MeshRenderer.
    /// </summary>
    public override void PlayMedia()
    {
        PlayVideoPreview();
        GetComponent<MeshRenderer>().enabled = true;
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Stopping for the preview consists in stopping the video player and disabling its MeshRenderer.
    /// </summary>
    public override void StopMedia()
    {
        GetComponent<VideoPlayer>().Stop();
        GetComponent<MeshRenderer>().enabled = false;
    }
    /// <summary>
    /// Starts the loop of playing the segment of the video.
    /// </summary>
    private void PlayVideoPreview()
    {
        GetComponent<VideoPlayer>().Prepare();        
        GetComponent<VideoPlayer>().Play();
        Invoke("PauseVideoPreview", stoppreview - startpreview);
    }
    /// <summary>
    /// Ends the loop of playing the segment of the video and call the start again.
    /// </summary>
    private void PauseVideoPreview()
    {
        GetComponent<VideoPlayer>().Pause();
        GetComponent<VideoPlayer>().time = startpreview;
        PlayVideoPreview();
    }
}
