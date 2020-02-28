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
        AddImage(2f, 2, @"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg", 15f, 0f, 0f);
        AddImage(3f, 2, @"C:\Users\paulo\Pictures\Saved Pictures\vapor-wave.png", 15f, -30f, 0f);
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
            media.GetComponent<ImageController>().Play();
        }
    }
    void AddImage(float start_time, float duration, string url, float r, float theta, float phi)
    {
        GameObject newImage = Instantiate(imagePrefab);
        newImage.GetComponent<ImageController>().start_time = start_time;
        newImage.GetComponent<ImageController>().duration = duration;
        newImage.GetComponent<ImageController>().LoadImage(url);

        Vector3 origin = new Vector3(0,0,r);

        newImage.transform.position = PolarToCartesian(origin, theta, phi);
        newImage.transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform.position, Vector3.up);
        newImage.transform.Rotate(new Vector3(0,180,0));
        other_media.Add(newImage);        
    }

    private Vector3 PolarToCartesian(Vector3 origin, float theta, float phi) {
        var rotation = Quaternion.Euler(theta, phi, 0);
        Vector3 point = rotation * origin;
        return point;
    }
}
