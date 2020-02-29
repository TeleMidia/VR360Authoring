using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audioContainer;
    void Start()
    {
        audioContainer = GetComponent<AudioSource>();
        StartCoroutine(LoadAudio());
    }
    IEnumerator LoadAudio()
    {
        string FullPath = "C:/Users/paulo/Downloads/sample.wav";
        FullPath = "file:///" + FullPath;
        WWW URL = new WWW(FullPath);
        yield return URL;

        audioContainer.clip = URL.GetAudioClip(false, true);
        audioContainer.Play();
    }
}
