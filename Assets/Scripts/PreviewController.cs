using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;

public class PreviewController : MediaControllerAbstract
{    
    private float startpreview;
    private float stoppreview;
    public override void Load()
    {
        GetComponent<VideoPlayer>().url = controller.GetComponent<VideoPlayer>().url;
        GetComponent<VideoPlayer>().Prepare();
        string[] numbers = Regex.Split(previewTime, @"\D+");
        Debug.Log("Quantidade: " + numbers.Length);
        int i = 0;
        float[] times = new float[2];
        foreach (string value in numbers)
        {
            if (!string.IsNullOrEmpty(value))
            {
                float n = float.Parse(value);
                times[i++] = n;
            }
        }
        startpreview = times[0];
        stoppreview = times[1];
    }

    public override void PlayMedia()
    {
        PlayVideoPreview();
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void PlayVideoPreview()
    {
        GetComponent<VideoPlayer>().Prepare();
        GetComponent<VideoPlayer>().time = startpreview;
        GetComponent<VideoPlayer>().Play();
        Invoke("PauseVideoPreview", stoppreview - startpreview);
    }
    private void PauseVideoPreview()
    {
        GetComponent<VideoPlayer>().Pause();
        PlayVideoPreview();
    }

    public override void StopMedia()
    {
        //CancelInvoke();
        GetComponent<VideoPlayer>().Stop();
        GetComponent<MeshRenderer>().enabled = false;
    }
}
