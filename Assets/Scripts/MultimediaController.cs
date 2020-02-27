using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MultimediaController : MonoBehaviour
{
    // Start is called before the first frame update

    public VideoPlayer video360;
    public GameObject imagePrefab;
    private List<GameObject> other_media;
    void Start()
    {
        other_media = new List<GameObject>();
        video360.started += StartOtherMedia;
        AddImage(5f, 3f, @"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg", 16f, 9f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(video360.time);
        if (Input.GetMouseButtonDown(0))
        {
            video360.Play();
        }
    }
    void StartOtherMedia(VideoPlayer source)
    {
        foreach(GameObject media in other_media)
        {
            media.GetComponent<ImageController>().Play();
        }
    }
    void AddImage(float start_time, float duration, string url, float width, float height)
    {
        GameObject newImage = Instantiate(imagePrefab);
        newImage.GetComponent<ImageController>().start_time = start_time;
        newImage.GetComponent<ImageController>().duration = duration;
        newImage.GetComponent<ImageController>().LoadImage(url, width, height);
        other_media.Add(newImage);
    }
}
