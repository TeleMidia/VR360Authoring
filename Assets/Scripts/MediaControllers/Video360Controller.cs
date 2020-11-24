using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video360Controller : MonoBehaviour
{
    // Start is called before the first frame update

    private VideoPlayer video360;

    public GameObject video360PreviewSphere;

    public Texture Video360Texture;
    public Texture TeleMidiaTexture;
    public Material Material360;

    private List<GameObject> other_media;
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
    public void StartVideo360()
    {
        video360.Prepare();
        Material360.mainTexture = Video360Texture;
        video360.Play();
    }
    

    public void StopVideo360()
    {
        video360.Stop();
        Material360.mainTexture = TeleMidiaTexture;
        AbortOtherMedia(video360);
        video360.Prepare();

    }

    void EndVideo360(VideoPlayer source)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaController>().StopPresentation();
    }
    void StartOtherMedia(VideoPlayer source)
    {
        foreach (GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().PrepareAnchors();
        }
    }   

    private void AbortOtherMedia(VideoPlayer source)
    {
        foreach (GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().AbortMedia();
        }
    }

    
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
