using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
///  This class is the script that controls the video360.
/// </summary>
public class Video360Controller : MonoBehaviour
{
    //videoPlayer component that plays the video
    private VideoPlayer video360;
    //texture for 360 videos
    public Texture Video360Texture;
    //texture used when no 360 video is playing
    public Texture TeleMidiaTexture;
    //material used for 360 videos
    public Material Material360;
    //list of the media objects that are added to this interactive 360 video
    private List<GameObject> other_media;
    /// <summary>
    /// Loads and prepares to play the 360 video.
    /// </summary>
    /// <param name="file_path">Path of the 360 video</param>
    /// <param name="volume">volume of the 360 video</param>
    public void LoadVideo360(string file_path, float volume)
    {
        this.video360 = GetComponent<VideoPlayer>();
        video360.url = "file:///" + file_path;
        video360.Prepare();
        other_media = new List<GameObject>();
        video360.started += StartOtherMedia;
        video360.loopPointReached += AbortOtherMedia;
        video360.loopPointReached += EndVideo360;
        video360.SetDirectAudioVolume(volume: volume, trackIndex: 0);
    }
    /// <summary>
    /// Starts the 360 video.
    /// </summary>
    public void StartVideo360()
    {
        video360.Prepare();
        Material360.mainTexture = Video360Texture;
        video360.Play();
    }
    
    /// <summary>
    /// Stops the 360 video.
    /// </summary>
    public void StopVideo360()
    {
        video360.Stop();
        Material360.mainTexture = TeleMidiaTexture;
        AbortOtherMedia(video360);
        video360.Prepare();
    }
    /// <summary>
    /// Stops the presentation the video finishes.
    /// </summary>
    /// <param name="source">The video player that is playing the 360 video</param>
    void EndVideo360(VideoPlayer source)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaControllerScript>().StopPresentation();
    }
    /// <summary>
    /// Starts the additional media objects that are added to this 360 video.
    /// </summary>
    /// <param name="source">The video player that is playing the 360 video</param>
    void StartOtherMedia(VideoPlayer source)
    {
        foreach (GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().PrepareAnchors();
        }
    }
    /// <summary>
    /// Aborts the media objects when the video finishes.
    /// </summary>
    /// <param name="source">The video player that is playing the 360 video</param>
    private void AbortOtherMedia(VideoPlayer source)
    {
        foreach (GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().AbortMedia();
        }
    }

    /// <summary>
    /// Adds additional media objects to this 360 video.
    /// </summary>
    /// <param name="id">id of the media object</param>
    /// <param name="mediaPrefab">prefab of the media object that will be added</param>
    /// <param name="begin">start time of the media object in seconds</param>
    /// <param name="r">media object distance from the center </param>
    /// <param name="theta">vertical angle of the media object in degrees</param>
    /// <param name="phi">horizontal angle of the media object in degrees</param>
    /// <param name="file_path">source file url of the media object</param>
    /// <param name="volume">volume of the media object</param>
    /// <param name="loop">wether the media object executes in loop</param>
    /// <param name="duration">duration of the media object in seconds</param>
    /// <param name="follow_camera">wether the media object follows the camera (is fixed to the user)</param>
    /// <param name="text">text of the media object</param>
    /// <param name="on_select_name">media object that will be played when the current is selected</param>
    /// <param name="during_out_of_focus_name">media object that will be played when the current is out of focus</param>
    /// <param name="on_focus_name">media object that will be played when the current is on focus</param>
    /// <param name="clipBegin">initial time of the segment of the media that will be played</param>
    /// <param name="clipEnd">final time of the segment of the media that will be played</param>
    /// <returns>The media object added</returns>
    public GameObject AddMedia(string id, GameObject mediaPrefab, float begin=0, float r=0, float theta=0, float phi=0, string file_path = "", 
                               float volume = 0, bool loop = false, float duration = float.MaxValue, bool follow_camera = false, string text = "", 
                               string on_select_name = "", string during_out_of_focus_name="", string on_focus_name="", float clipBegin = 0, float clipEnd = 5)
    {
        GameObject newMedia = Instantiate(mediaPrefab);
        newMedia.GetComponent<MediaControllerAbstract>().Configure(id:id, father:this.gameObject, start_time:begin, 
                                                                   duration:duration, file_path:file_path, r:r, theta:theta, 
                                                                   phi:phi, volume:volume, loop:loop, follow_camera:follow_camera,
                                                                   text:text, on_select_name:on_select_name,
                                                                   clipBegin: clipBegin, clipEnd: clipEnd, on_focus_name:on_focus_name, 
                                                                   during_out_of_focus_name: during_out_of_focus_name);        
        other_media.Add(newMedia);

        return newMedia;
    }
    /// <summary>
    /// Add subtitles to the current 360 video.
    /// </summary>
    /// <param name="id">id of the subtitles</param>
    /// <param name="mediaPrefab">Prefab of the text</param>
    /// <param name="file_path">path to the srt file</param>
    /// <param name="r">distance from the center</param>
    /// <param name="theta">vertical angle of the media object in degrees</param>
    /// <param name="phi">horizontal angle of the media object in degrees</param>
    /// <param name="on_select_name">media object that will be played when the current is selected</param>
    public void AddSubtitle(string id, GameObject mediaPrefab, string file_path, float r=0, float theta=0, float phi=0, string on_select_name = "")
    {
        SubtitleFragment[] subtitleFragments = SubtitleReader.ReadSubtitles(file_path);
        int i = 0;
        foreach(SubtitleFragment subtitleFragment in subtitleFragments)
        {
            AddMedia(id+"_"+(i++),mediaPrefab: mediaPrefab, begin: subtitleFragment.begin, duration: subtitleFragment.duration, text: subtitleFragment.text,
                r:r, theta:theta, phi:phi, follow_camera:true, on_select_name: on_select_name);
        }
    }
}
