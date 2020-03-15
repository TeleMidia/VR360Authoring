using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MediaControllerAbstract
{
    public override void Load()
    {
             
    }

    public override void PlayMedia()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void Update()
    {
        if (this.src_roi_object != null)
        {
            Transform child_camera = this.transform.GetChild(0);
            child_camera.position = Vector3.zero;
            child_camera.LookAt(this.src_roi_object.transform.position, Vector3.up);
        }
    }
}
