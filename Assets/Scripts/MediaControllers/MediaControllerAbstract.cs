using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MediaControllerAbstract: MonoBehaviour
{
    public float start_time, duration, volume;
    private float theta, phi, r;
    public string file_path, text;
    public bool loop, follow_camera;
    public GameObject mainCamera;
    private Vector3 origin;
    private Movement movement;
    private bool isMoving;
    private Vector3 start_pos;

    private void InvokePlayStop()
    {
        PlayMedia();
        PlayMovement();
        Invoke("StopMedia", duration);
        Invoke("StopMovement", duration);
    }

    private void PlayMovement()
    {
        this.isMoving = true;
    }

    private void StopMovement()
    {
        this.isMoving = false;
    }

    public void PrepareAnchors()
    {        
        Invoke("InvokePlayStop", start_time);        
    }

    private void Update()
    {
        if(this.isMoving && this.movement != null)
        {
            this.transform.localPosition = this.movement.Move(start_r:this.r, start_theta: this.theta, start_phi: this.phi);
            this.transform.LookAt(Vector3.zero, Vector3.up);
            this.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    public virtual void Configure(float start_time, float duration, string file_path, float r, float theta, float phi, float volume, bool loop, bool follow_camera, string text, Movement movement)
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        this.start_time = start_time;
        this.duration = duration;
        this.volume = volume;
        this.file_path = file_path;
        this.loop = loop;
        this.follow_camera = follow_camera;
        this.text = text;
        this.theta = theta;
        this.phi = phi;
        this.r = r;
        this.movement = movement;
        this.isMoving = false;    


        this.origin = new Vector3(0, 0, r);
        this.start_pos = Utils.PolarToCartesian(this.origin, theta, phi);

        if (this.follow_camera)
        {
            this.transform.parent = mainCamera.transform;
            this.transform.localPosition = this.start_pos;
            this.transform.LookAt(mainCamera.transform.position, mainCamera.transform.up);
        }
        else
        {
            this.transform.localPosition = this.start_pos;
            this.transform.LookAt(Vector3.zero,Vector3.up);
        }
               
        
        this.transform.Rotate(new Vector3(0, 180, 0));

        Load();
    }

    public abstract void Load();

    public abstract void StopMedia();

    public abstract void PlayMedia();   

}