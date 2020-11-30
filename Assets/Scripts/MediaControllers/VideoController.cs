using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Inherits MediaControllerAbstract. This class is the script that controls the video media object.
/// </summary>
public class VideoController : MediaControllerAbstract
{
    /// Video Player component that is used to play the video.
    private VideoPlayer videoPlayer;
    /// Audio source that is used to stores and play the audio of the video.
    private AudioSource audioSource;

    /// <summary>
    /// Inherited from MediaControllerAbstract.
    /// It loads the video on the video player from the url, prepare it and call the LoadVideo routine when the video is prepared.
    /// </summary>
    public override void Load()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += Ended;
        audioSource = GetComponent<AudioSource>();

        videoPlayer.url = "file:///" + this.file_path;
        videoPlayer.isLooping = this.loop;
        audioSource.volume = this.volume;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += LoadVideo;        
    }

    /// <summary>
    /// Called when the video file has been loaded and the video player is prepared.
    ///  Additional configurations are made.
    /// </summary>
    /// <param name="source">Video Player</param>
    public void LoadVideo(VideoPlayer source)
    {
        float width, height;
        width = 10;
        height = width * source.height / source.width;
        this.transform.localScale = new Vector3(width, height, 1);
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Playing for the video consists in playing the videoPlayer and enabling its mesh renderer
    /// </summary>
    public override void PlayMedia()
    {
        videoPlayer.Play();
        GetComponent<MeshRenderer>().enabled = true;
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract.
    ///  Stopping for the video consists in stopping the videoPlayer, prepare it to play again, and disabling its mesh renderer
    /// </summary>
    public override void StopMedia()
    {
        videoPlayer.Stop();
        videoPlayer.Prepare();
        GetComponent<MeshRenderer>().enabled = false;
    }
 
    /// <summary>
    /// Defines that when the video reaches the loop point, the mesh renderer is disabled if the video is not 
    /// to play in loop
    /// </summary>
    /// <param name="source">Video Player</param>
    private void Ended(VideoPlayer source)
    {
        if (this.loop == false)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
