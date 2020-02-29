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
    public GameObject imagePrefab;

    private List<GameObject> other_media;
    void Start()
    {
        other_media = new List<GameObject>();
        video360.started += StartOtherMedia;
        AddMedia(imagePrefab, 0f, 3, @"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg", 15f, 0f, 0f);
        AddMedia(imagePrefab, 3f, 2, @"C:\Users\paulo\Pictures\Saved Pictures\vapor-wave.png", 15f, -30f, 0f);
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
            media.GetComponent<ImageController>().PrepareAnchors();
        }
    }
    void AddMedia(GameObject mediaPrefab, float start_time, float duration, string file_path, float r, float theta, float phi, float volume=0)
    {
        GameObject newMedia = Instantiate(mediaPrefab);
        newMedia.GetComponent<MediaControllerAbstract>().Configure(start_time, duration, file_path, r, theta, phi, volume);        
        other_media.Add(newMedia);        
    }    
}
