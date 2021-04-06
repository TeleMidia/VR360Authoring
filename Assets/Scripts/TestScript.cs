using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Threading.Tasks;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// This class performs the unit tests on the player.
/// </summary>
public class TestScript : MonoBehaviour
{
    /// <summary>
    /// The multimedia controller script that contains the prefabs of media objects to be tested.
    /// </summary>
    MultimediaControllerScript controller;
    private int waitTime = 1;
    Dictionary<string, GameObject> scene_objects;

    /// Start is called before the first frame update. It starts the tests if the player is on test mode
    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaControllerScript>();

        if (controller.testMode)
        {
            StartCoroutine(RunTests());
        }
    }

    /// <summary>
    /// This method runs the test modules
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunTests()
    {
        Debug.Log("Tests beginning...");

        scene_objects = new Dictionary<string, GameObject>();
        //Test if video is being loaded, started and stopped correctly
        yield return StartCoroutine(TestLoadStartStop360Video(@"D:\Movies\360Video\ipanema.mp4"));
        //Test additional media if it is starting and stopping at correct timing
        yield return StartCoroutine(TestAdditionalMedia360Video(video360_path: @"D:\Movies\360Video\ipanema.mp4", 
            mediaPrefab: controller.imagePrefab, media_path: @"C:\Users\paulo\Pictures\Imagem1.png", begin:1f, duration:5f));
        //Test if hotspot events are working: onLookAt and duringNotLookingAt
        yield return StartCoroutine(TestHotSpot_OnLookAt_DuringNotLookingAt(@"D:\Movies\360Video\ipanema.mp4",
            mediaPrefab: controller.imagePrefab, media1_path: @"C:\Users\paulo\Pictures\Imagem1.png", duration:3, media2_path: @"C:\Users\paulo\Pictures\qr-code.png"));
        //Test if navigation is working: we should move from one video to another by selecting a preview
        yield return TestNavigation(@"D:\Movies\360Video\ipanema.mp4", @"D:\Movies\360Video\video_aha.mp4", controller.previewPlain, 15);

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
        Debug.Log("Testing if video is being loaded, started and stopped correctly");
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
    /// <summary>
    /// Test additional media, especially if it is starting and stopping at correct timing
    /// </summary>
    /// <param name="video360_path">Path to 360 video</param>
    /// <param name="mediaPrefab">Prefab of the media added to 360 video</param>
    /// <param name="media_path">Path to the media</param>
    /// <param name="begin">Begin time of the media</param>
    /// <param name="duration">Duration of the media</param>
    /// <returns></returns>
    public IEnumerator TestAdditionalMedia360Video(string video360_path, GameObject mediaPrefab, string media_path, float begin, float duration)
    {
        Debug.Log("Testing additional media, especially if it is starting and stopping at correct timing");
        GameObject myVideo360 = controller.AddVideo360(video360_path);

        //test if additional media is loaded
        GameObject media = myVideo360.GetComponent<Video360Controller>().AddMedia(scene_objects, "m1", mediaPrefab, file_path: media_path, begin: begin, duration: duration);
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
    /// <summary>
    /// Test if hotspot events are working: onLookAt and duringNotLookingAt
    /// </summary>
    /// <param name="video360_path">Path to the 360 video</param>
    /// <param name="mediaPrefab">Prefab of the media</param>
    /// <param name="media1_path">Path to media1</param>
    /// <param name="duration">duration of media1</param>
    /// <param name="media2_path">Path to media2</param>
    /// <returns></returns>
    public IEnumerator TestHotSpot_OnLookAt_DuringNotLookingAt(string video360_path, GameObject mediaPrefab, string media1_path, float duration, string media2_path)
    {
        Debug.Log("Test if hotspot events are working: onLookAt and duringNotLookingAt");
        GameObject myVideo360 = controller.AddVideo360(video360_path);

        GameObject media1 = myVideo360.GetComponent<Video360Controller>().AddMedia(scene_objects, "media1", mediaPrefab, duration: duration);
        GameObject media2 = myVideo360.GetComponent<Video360Controller>().AddMedia(scene_objects, "media2", mediaPrefab);
        GameObject hotspot = myVideo360.GetComponent<Video360Controller>().AddMedia(scene_objects, "hot1", controller.hotspotPrefab, r:0.7f, phi:20, theta:0);

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
    /// <summary>
    /// Test if navigation is working: we should move from one video to another by selecting a preview and coming back to the first video
    /// </summary>
    /// <param name="video360_1_path">Path to the first video</param>
    /// <param name="video360_2_path">Path to the second video</param>
    /// <param name="previewPrefab">Prefab of preview object</param>
    /// <param name="duration">duaration of the previews</param>
    /// <returns></returns>
    public IEnumerator TestNavigation(string video360_1_path, string video360_2_path, GameObject previewPrefab, float duration)
    {
        Debug.Log("Test if navigation is working: we should move from one video to another by selecting a preview and coming back to the first video");
        GameObject myVideo360_1 = controller.AddVideo360(video360_1_path);
        GameObject myVideo360_2 = controller.AddVideo360(video360_2_path);

        GameObject prev1 = myVideo360_1.GetComponent<Video360Controller>().AddMedia(scene_objects, "prev1", previewPrefab, duration: duration);
        GameObject prev2 = myVideo360_2.GetComponent<Video360Controller>().AddMedia(scene_objects, "prev2", previewPrefab, duration: duration);

        //on select prev1, we should navigate to video2
        prev1.GetComponent<PreviewController>().on_select_object = myVideo360_2;
        //on select prev2, we should navigate to video1
        prev2.GetComponent<PreviewController>().on_select_object = myVideo360_1;

        myVideo360_1.GetComponent<Video360Controller>().StartVideo360();
        yield return new WaitForSeconds(waitTime*10);

        //selecting previ1 and testing if we navigate to video myVideo360_2
        prev1.GetComponent<PreviewController>().OnSelectMedia();
        yield return new WaitForSeconds(waitTime);

        bool video1_stopped = !myVideo360_1.GetComponent<VideoPlayer>().isPlaying;
        bool video2_started = myVideo360_2.GetComponent<VideoPlayer>().isPlaying;
        bool navigated = video1_stopped && video2_started;
        Debug.Assert(navigated, "Navigation to video 2 has not worked");

        //selecting prev2 and testing if we navigate back to myVideo360_1
        yield return new WaitForSeconds(waitTime * 10);

        prev2.GetComponent<PreviewController>().OnSelectMedia();
        yield return new WaitForSeconds(waitTime);

        bool video2_stopped = !myVideo360_2.GetComponent<VideoPlayer>().isPlaying;
        bool video1_started = myVideo360_1.GetComponent<VideoPlayer>().isPlaying;
        bool navigated_back = video2_stopped && video1_started;
        Debug.Assert(navigated_back, "Navigation to video 1 has not worked");

        myVideo360_1.GetComponent<Video360Controller>().StopVideo360();
        Destroy(prev1);
        Destroy(prev2);
        Destroy(myVideo360_1);
        Destroy(myVideo360_2);
    }

}
