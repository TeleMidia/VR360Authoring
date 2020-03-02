using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MediaControllerAbstract
{
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    public override void Load()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.started += PlayAfter;
        videoPlayer.loopPointReached += Ended;
        audioSource = GetComponent<AudioSource>();

        videoPlayer.url = "file:///" + this.file_path;
        videoPlayer.isLooping = this.loop;
        audioSource.volume = this.volume;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += LoadVideo;        
    }

    public void LoadVideo(VideoPlayer source) {
        float width, height;
        width = 10;
        height = width * source.height / source.width;
        this.transform.localScale = new Vector3(width, height, 1);
    }

    public override void PlayMedia()
    {
        videoPlayer.Play();
    }

    public override void StopMedia()
    {
        videoPlayer.Stop();
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void PlayAfter(VideoPlayer source)
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void Ended(VideoPlayer source)
    {
        if (this.loop == false)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
