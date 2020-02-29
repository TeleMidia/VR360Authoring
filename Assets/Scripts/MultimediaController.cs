using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MultimediaController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Video 360")]
    public VideoPlayer video360;

    [Header("Common Media")]
    public GameObject imagePrefab, audioPrefab;

    private List<GameObject> other_media;
    void Start()
    {
        other_media = new List<GameObject>();
        video360.started += StartOtherMedia;
        video360.loopPointReached += StopOtherMedia;

        AddMedia(imagePrefab, 
            start_time:0f, duration:3, file_path:@"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg", 
            r:15f, theta:0f, phi:0f);
        AddMedia(imagePrefab,
            start_time: 3f, duration: 2, file_path: @"C:\Users\paulo\Pictures\Saved Pictures\vapor-wave.png",
            r: 15f, theta: -30f, phi: 0f);

        AddMedia(audioPrefab,
            start_time: 3f, duration: 2, file_path: "C:/Users/paulo/Downloads/sample.wav",
            r: 15f, theta: -30f, phi: 0f, volume:1, loop:false);
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            video360.Play();
        }
    }
    void StartOtherMedia(VideoPlayer source)
    {
        foreach(GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().PrepareAnchors();
        }
    }

    void StopOtherMedia(VideoPlayer source)
    {
        Debug.Log("Reached Loop");
        foreach (GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().StopMedia();
            media.GetComponent<MediaControllerAbstract>().PrepareAnchors();
        }
    }
    void AddMedia(GameObject mediaPrefab, float start_time, string file_path, float r, float theta, float phi, float volume = 0, bool loop = false, float duration = float.MaxValue)
    {
        GameObject newMedia = Instantiate(mediaPrefab);
        newMedia.GetComponent<MediaControllerAbstract>().Configure(start_time, duration, file_path, r, theta, phi, volume, loop);        
        other_media.Add(newMedia);        
    }    
}
