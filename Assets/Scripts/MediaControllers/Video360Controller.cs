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

    
    public void AddMedia(GameObject mediaPrefab, float start_time=0, float r=0, float theta=0, float phi=0, string file_path = "", float volume = 0, bool loop = false, float duration = float.MaxValue, bool follow_camera = false, string text = "", Movement movement = null, GameObject controller = null)
    {
        GameObject newMedia = Instantiate(mediaPrefab);
        newMedia.GetComponent<MediaControllerAbstract>().Configure(this.gameObject, start_time, duration, file_path, r, theta, phi, volume, loop, follow_camera, text, movement, controller);        
        other_media.Add(newMedia);        
    }    

    public void AddSubtitle(GameObject mediaPrefab, string file_path, float r=0, float theta=0, float phi=0)
    {
        SubtitleFragment[] subtitleFragments = SubtitleReader.ReadSubtitles(file_path);

        foreach(SubtitleFragment subtitleFragment in subtitleFragments)
        {
            AddMedia(mediaPrefab: mediaPrefab, start_time: subtitleFragment.start_time, duration: subtitleFragment.duration, text: subtitleFragment.text,
                r:r, theta:theta, phi:phi, follow_camera:true);
        }
    }
}
