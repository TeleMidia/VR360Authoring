using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// Inherits MediaControllerAbstract
/// This class is the script that controls the mirror media object
/// </summary>
public class MirrorController : MediaControllerAbstract
{
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// This media object doesnt load anything in specific besides what is loaded in the MediaControllerAbstract Class
    /// It is empty because the method has to be implemented
    /// </summary>
    public override void Load()
    {
             
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// Playing for the mirror consists in enabling its MeshRenderer
    /// </summary>
    public override void PlayMedia()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// Stopping for the mirror consists in disabling its MeshRenderer
    /// </summary>
    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    /// <summary>
    /// Called once per frame, inherited from MonoBehaviour
    /// Makes the mirror always transmits the content of hotspot.
    /// It internally uses a camera that is always "filming" the hotspot's position
    /// </summary>
    private void Update()
    {
        if (this.src_hotspot_object != null)
        {
            Transform child_camera = this.transform.GetChild(0);
            child_camera.position = Vector3.zero;
            child_camera.LookAt(this.src_hotspot_object.transform.position, Vector3.up);
        }
    }
}
