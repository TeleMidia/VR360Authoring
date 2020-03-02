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
    public GameObject imagePrefab, audioPrefab, videoPrefab, textPrefab;

    private List<GameObject> other_media;
    void Start()
    {
        other_media = new List<GameObject>();
        video360.Prepare();
        video360.started += StartOtherMedia;
        video360.loopPointReached += StopOtherMedia;


        AddMedia(textPrefab, start_time: 0, duration: 3, r: 5, theta: 25, phi: 0, text:"Imagem seguindo camera", follow_camera:true);
        AddMedia(textPrefab, start_time: 3, duration: 5, r: 5, theta: 25, phi: 0, text: "Imagem com som", follow_camera: true);
        AddMedia(textPrefab, start_time: 38, duration: 20, r: 5, theta: 25, phi: 0, text: "Ameno Dorime all around you", follow_camera: true);

        AddMedia(imagePrefab, 
            start_time:0f, duration:3, file_path:@"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg", 
            r:15f, theta:0f, phi:0f, follow_camera: true);

        

        AddMedia(imagePrefab,
            start_time: 3f, duration: 10, file_path: @"C:\Users\paulo\Pictures\Saved Pictures\vapor-wave.png",
            r: 15f, theta: -30f, phi: 0f, movement: new LinearMovement(r: 15, theta: 0f, phi: 0, duration:6f));

        AddMedia(audioPrefab,
            start_time: 3f, duration: 10, file_path: "C:/Users/paulo/Downloads/sample.wav",
            r: 5f, theta: -30f, phi: 0f, volume:1, loop:false, movement: new LinearMovement(r: 15, theta: 0f, phi: 0, duration:6f));


        AddMedia(videoPrefab,
           start_time: 10f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 60f, volume: 1, loop: false);


        AddMedia(videoPrefab,
           start_time: 15f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f, volume: 1, loop: false);


        AddMedia(videoPrefab,
           start_time: 20f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: -60f, volume: 1, loop: false);

        AddMedia(videoPrefab,
           start_time: 25f, duration: 13, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f, volume: 1, loop: true, movement: new CircularMovement(r:0f, theta:0, phi:360, duration:10));

        float step = 60;
        for(int i = 0; i < 360/step; i++)
        {
            AddMedia(videoPrefab, start_time: 38f, duration: 60, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f+i*step, volume: 1, loop: true, movement: new CircularMovement(r:-22f, theta: 0, phi: 360, duration: 10));
        }

        //StartOtherMedia(video360);


    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            if (video360.isPrepared)
            {
                video360.Play();
            }
        }
         
    }
    void StartOtherMedia(VideoPlayer source)
    {
        foreach(GameObject media in other_media)
        {
            media.GetComponent<MediaControllerAbstract>().PrepareAnchors();
        }
    }

    void StartVideo360(VideoPlayer source)
    {
        video360.Play();
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

    
    void AddMedia(GameObject mediaPrefab, float start_time, float r, float theta, float phi, string file_path = "", float volume = 0, bool loop = false, float duration = float.MaxValue, bool follow_camera = false, string text = "", Movement movement = null)
    {
        GameObject newMedia = Instantiate(mediaPrefab);
        newMedia.GetComponent<MediaControllerAbstract>().Configure(start_time, duration, file_path, r, theta, phi, volume, loop, follow_camera, text, movement);        
        other_media.Add(newMedia);        
    }    
}
