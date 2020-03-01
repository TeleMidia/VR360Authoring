using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MediaControllerAbstract: MonoBehaviour
{
    public float start_time, duration, volume;
    public string file_path;
    public bool loop, follow_camera;
    public GameObject mainCamera;

    private void InvokePlayStop()
    {
        PlayMedia();
        Invoke("StopMedia", duration);        
    }
    public void PrepareAnchors()
    {
        Invoke("InvokePlayStop", start_time);        
    }

    public virtual void Configure(float start_time, float duration, string file_path, float r, float theta, float phi, float volume, bool loop, bool follow_camera)
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        this.start_time = start_time;
        this.duration = duration;
        this.volume = volume;
        this.file_path = file_path;
        this.loop = loop;
        this.follow_camera = follow_camera;

        if (this.follow_camera)
        {
            this.transform.parent = mainCamera.transform;
        }

        Vector3 origin = new Vector3(0, 0, r);

        this.transform.position = Utils.PolarToCartesian(origin, theta, phi);
        this.transform.LookAt(mainCamera.transform.position, Vector3.up);
        this.transform.Rotate(new Vector3(0, 180, 0));

        Load();
    }

    public abstract void Load();

    public abstract void StopMedia();

    public abstract void PlayMedia();   

}