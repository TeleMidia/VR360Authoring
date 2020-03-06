using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class MultimediaController : MonoBehaviour
{
    public delegate void MyHandler();
    // Start is called before the first frame update
    [Header("Video 360")]
    public GameObject video360Prefab, video360PreviewPrefab;

    [Header("Common Media")]
    public GameObject imagePrefab, audioPrefab, videoPrefab, textPrefab;

    public GameObject startPresentation;

    private GameObject video1, video2;

    private event MyHandler Port;
    private event MyHandler End;
    void Start()
    {
        video1 = AddVideo360("C:/Users/paulo/Downloads/360_VR Master Series _ Free Download _ Crystal Shower Falls.mp4");
        video2 = AddVideo360("C:/Users/paulo/Downloads/Best VR 360 Video.mp4");

        video1.GetComponent<Video360Controller>().AddSubtitle(textPrefab, r: 10, theta: 25, phi: 0, file_path: @"C:\Users\paulo\Downloads\jumanji-the-next-level_HI_english-2156994\Jumanji.The.Next.Level.2019.BDRip.XviD.AC3-EVO-HI.srt");

        //video1.GetComponent<Video360Controller>().AddMedia(textPrefab, start_time: 0, duration: 3, r: 5, theta: 25, phi: 0, text: "Imagem seguindo camera", follow_camera: true);
        //video1.GetComponent<Video360Controller>().AddMedia(textPrefab, start_time: 3, duration: 5, r: 5, theta: 25, phi: 0, text: "Imagem com som", follow_camera: true);

        video1.GetComponent<Video360Controller>().AddMedia(imagePrefab,
            start_time: 0f, duration: 3, file_path: @"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg",
            r: 15f, theta: 0f, phi: 0f, follow_camera: true);



        video1.GetComponent<Video360Controller>().AddMedia(imagePrefab,
            start_time: 3f, duration: 10, file_path: @"C:\Users\paulo\Pictures\Saved Pictures\vapor-wave.png",
            r: 15f, theta: -30f, phi: 0f, movement: new LinearMovement(r: 15, theta: 0f, phi: 0, duration: 6f));

        video1.GetComponent<Video360Controller>().AddMedia(audioPrefab,
            start_time: 3f, duration: 10, file_path: "C:/Users/paulo/Downloads/sample.wav",
            r: 15f, theta: -30f, phi: 0f, volume: 1, loop: false, movement: new LinearMovement(r: 15, theta: 0f, phi: 0, duration: 6f));


        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 10f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 60f, volume: 1, loop: false);


        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 15f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f, volume: 1, loop: false);


        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 20f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: -60f, volume: 1, loop: false);

        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 25f, duration: 12, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f, volume: 1, loop: true, movement: new CircularMovement(r: 0f, theta: 0, phi: 720, duration: 10));

        video1.GetComponent<Video360Controller>().AddMedia(video360PreviewPrefab, start_time: 60, duration: 60, r: 4, theta: 0, phi: 60, controller: video2);

        float step = 60;
        for (int i = 0; i < 360 / step; i++)
        {
            video2.GetComponent<Video360Controller>().AddMedia(videoPrefab, start_time: 2f, duration: 60, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f + i * step, volume: 1, loop: false, movement: new CircularMovement(r: -22f, theta: 0, phi: 360, duration: 10));
        }

        SetAsInitial(this.video1);
        //this.Port();

        GameObject[] videos360 = GameObject.FindGameObjectsWithTag("Video360");
        foreach(GameObject video360 in videos360)
        {
            this.End += video360.GetComponent<Video360Controller>().StopVideo360;
        }

        StopPresentation();
        //StartPresentation();
    }
    
    GameObject AddVideo360(string url)
    {
        GameObject video = Instantiate(video360Prefab);
        video.GetComponent<Video360Controller>().LoadVideo360(url);
        return video;        
    }

    private void SetAsInitial(GameObject video)
    {
        this.Port += video.GetComponent<Video360Controller>().StartVideo360;
    }

    public void StartPresentation()
    {
        this.Port();
    }
    public void StopPresentation()
    {
        this.End();
        Instantiate(startPresentation);
    }
}
