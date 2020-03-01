using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MediaControllerAbstract
{
    // Start is called before the first frame update

    private AudioSource audioContainer;

    IEnumerator LoadAudio()
    {
        string FullPath;
        FullPath = "file:///" + this.file_path;
        WWW URL = new WWW(FullPath);
        yield return URL;

        audioContainer.clip = URL.GetAudioClip(false, true);        
    }

    public override void Load()
    {
        audioContainer = GetComponent<AudioSource>();
        audioContainer.volume = this.volume;
        audioContainer.loop = this.loop;
        StartCoroutine(LoadAudio());
    }

    public override void StopMedia()
    {
        audioContainer.Stop();
    }

    public override void PlayMedia()
    {
        audioContainer.Play();
    }
}
