using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Inherits MediaControllerAbstract. This class is the script that controls the audio media object.
/// </summary>
public class AudioController : MediaControllerAbstract
{
    //it is an audio source used to play the audio
    private AudioSource audioContainer;
    /// <summary>
    /// Coroutine that loads the audio from the file path.
    /// </summary>
    /// <returns>IEnumerator to coroutine</returns>
    IEnumerator LoadAudio()
    {
        string FullPath;
        FullPath = "file:///" + this.file_path;
        WWW URL = new WWW(FullPath);
        yield return URL;

        audioContainer.clip = URL.GetAudioClip(false, true);        
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract. Configures the audioContainer to load the audio with its configurations.
    /// </summary>
    public override void Load()
    {
        audioContainer = GetComponent<AudioSource>();
        audioContainer.volume = this.volume;
        audioContainer.loop = this.loop;
        StartCoroutine(LoadAudio());
    }
    
    /// <summary>
    /// Inherited from MediaControllerAbstract. Playing for the audio is to play the audioContainer.
    /// </summary>
    public override void PlayMedia()
    {
        audioContainer.Play();
    }

    /// <summary>
    /// Inherited from MediaControllerAbstract. Stopping for the audio is to stop the audioContainer.
    /// </summary>
    public override void StopMedia()
    {
        audioContainer.Stop();
    }
}
