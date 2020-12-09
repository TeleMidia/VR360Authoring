using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Threading.Tasks;

public class TestScript : MonoBehaviour
{
    /// <summary>
    /// The multimedia controller script that contains the prefabs of media objects to be tested.
    /// </summary>
    MultimediaControllerScript controller;
    private int waitTime = 1;

    /// Start is called before the first frame update
    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaControllerScript>();

        if (controller.testMode)
        {
            StartCoroutine(RunTests());
        }
    }


    public IEnumerator RunTests()
    {
        Debug.Log("Tests beginning...");


        ///Test if video is being loaded, started and stopped correctly
        yield return StartCoroutine(TestLoadStartStop360Video(@"D:\Movies\ipanema.mp4"));
        ///test additional media if it is starting and stopping at correct timing
        yield return StartCoroutine(TestAdditionalMedia360Video(video360_path: @"D:\Movies\ipanema.mp4", 
            mediaPrefab: controller.imagePrefab, media_path: @"C:\Users\paulo\Pictures\Imagem1.png", begin:1f, duration:5f));
        ///test if hotspot events are working: onFocus and duringOutOfFocus
        yield return StartCoroutine(TestHotSpot_OnFocus_DuringOutOfFocus(@"D:\Movies\ipanema.mp4",
            mediaPrefab: controller.imagePrefab, media1_path: @"C:\Users\paulo\Pictures\Imagem1.png", duration:3, media2_path: @"C:\Users\paulo\Pictures\qr-code.png"));


        Debug.Log("Tests runned sucessfully!!");
    }
    /// <summary>
    /// Test if video is being loaded, started and stopped correctly
    /// </summary>
    /// <param name="url">url of the video</param>
    /// <param name="waitTime">time waited to test</param>
    /// <returns></returns>
    public IEnumerator TestLoadStartStop360Video(string video360_path)
    {
        //testin if video loaded
        GameObject myVideo360 = null;
        controller.AddVideo360(video360_path);
            
        GameObject[] videos360 = GameObject.FindGameObjectsWithTag("Video360");

        foreach (GameObject v in videos360)
        {
            if (v.GetComponent<VideoPlayer>().url.Contains(video360_path))
            {
                myVideo360 = v;
                break;
            }
        }

        yield return new WaitForSeconds(waitTime);

        bool video_loaded = myVideo360 != null && myVideo360.GetComponent<VideoPlayer>().isPrepared;
        Debug.Assert(video_loaded, "360 Video have not loaded!");

        // testing if video started

        myVideo360.GetComponent<Video360Controller>().StartVideo360();
        yield return new WaitForSeconds(waitTime);

        bool video_started = myVideo360.GetComponent<VideoPlayer>().isPlaying;
        Debug.Assert(video_started, "360 video has not started");

        // testing if video stopped

        myVideo360.GetComponent<Video360Controller>().StopVideo360();
        yield return new WaitForSeconds(waitTime);

        bool video_stopped = !myVideo360.GetComponent<VideoPlayer>().isPlaying;
        Debug.Assert(video_stopped, "360 video has not stopped");
     
        Destroy(myVideo360);
    }

    public IEnumerator TestAdditionalMedia360Video(string video360_path, GameObject mediaPrefab, string media_path, float begin, float duration)
    {
        GameObject myVideo360 = controller.AddVideo360(video360_path);

        //test if additional media is loaded
        GameObject media = myVideo360.GetComponent<Video360Controller>().AddMedia("m1", mediaPrefab, file_path: media_path, begin: begin, duration: duration);
        yield return new WaitForSeconds(waitTime);

        bool media_loaded = media.Equals(GameObject.Find(media.name));
        Debug.Assert(media_loaded, "media has not been loaded");

        //test if media is started after begin time
        myVideo360.GetComponent<Video360Controller>().StartVideo360();
        yield return new WaitForSeconds(begin+waitTime);

        bool media_playing = media.GetComponent<MediaControllerAbstract>().IsPlaying;
        Debug.Assert(media_playing, "media is not playing");

        //test if media stopped after duration is over
        yield return new WaitForSeconds(duration);
        bool media_not_playing = !media.GetComponent<MediaControllerAbstract>().IsPlaying;
        Debug.Assert(media_not_playing, "media has not stopped");

        myVideo360.GetComponent<Video360Controller>().StopVideo360();
        Destroy(myVideo360);
        Destroy(media);
    }

    public IEnumerator TestHotSpot_OnFocus_DuringOutOfFocus(string video360_path, GameObject mediaPrefab, string media1_path, float duration, string media2_path)
    {
        GameObject myVideo360 = controller.AddVideo360(video360_path);

        GameObject media1 = myVideo360.GetComponent<Video360Controller>().AddMedia("media1", mediaPrefab, duration: duration);
        GameObject media2 = myVideo360.GetComponent<Video360Controller>().AddMedia("media2", mediaPrefab);
        GameObject hotspot = myVideo360.GetComponent<Video360Controller>().AddMedia("hot1", controller.hotspotPrefab, r:0.7f, phi:20, theta:0);

        // media1 should start when hotspot is on sight
        hotspot.GetComponent<HotspotController>().on_focus_object = media1;

        // media2 should start when hotspot is not on sight and stop when it is on focus again
        hotspot.GetComponent<HotspotController>().during_out_of_focus_object = media2;

        myVideo360.GetComponent<Video360Controller>().StartVideo360();

        //looking at hotspot and checking if media1 started
        GameObject main_camera = GameObject.FindGameObjectWithTag("MainCamera");
        main_camera.transform.LookAt(hotspot.transform.position, Vector3.up);
        yield return new WaitForSeconds(waitTime);

        bool media1_started = media1.GetComponent<MediaControllerAbstract>().IsPlaying;
        Debug.Assert(media1_started, "Media1 has not started");

        //looking away from the hotspot and checking if media2 started;
        main_camera.transform.Rotate(new Vector3(0, 180, 0));
        yield return new WaitForSeconds(waitTime);
        bool media2_started = media2.GetComponent<MediaControllerAbstract>().IsPlaying;
        Debug.Assert(media2_started, "Media2 has not started");

        //looking at hotspot again and checking if media2 stopped;
        main_camera.transform.LookAt(hotspot.transform.position, Vector3.up);
        yield return new WaitForSeconds(waitTime);
        bool media2_stopped = !media2.GetComponent<MediaControllerAbstract>().IsPlaying;
        Debug.Assert(media2_stopped, "Media2 has not stopped");

        myVideo360.GetComponent<Video360Controller>().StopVideo360();
        Destroy(media1);
        Destroy(media2);
        Destroy(myVideo360);

    }

}
