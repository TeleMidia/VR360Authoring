using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Responsible for controlling when the camera is looking or not at the defined hotspots.
/// </summary>
public class LookHotspotScript : MonoBehaviour
{
    ///list of hotspot objects
    public GameObject[] hotspot_objects;

    /// <summary>
    /// Called once per frame, inherited from MonoBehaviour.<br/>
    /// At each time it checks for each hotspot wether it is being seen or not.
    /// </summary>
    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 100);
        //on_focus_check
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("ROIObject"))
                {
                    GameObject on_focus_object = hit.collider.gameObject.GetComponent<HotspotController>().on_focus_object;
                    if (on_focus_object != null && !on_focus_object.GetComponent<MediaControllerAbstract>().IsPlaying)
                    {
                        //Debug.Log("Chamou");
                        on_focus_object.GetComponent<MediaControllerAbstract>().InvokePlayStop();
                    }

                    break;
                }
            }
        }
        //duringOutOfFocurs_check
        this.hotspot_objects = GameObject.FindGameObjectsWithTag("ROIObject");
        foreach (GameObject u_roi in this.hotspot_objects)
        {
            if (u_roi.GetComponent<HotspotController>().IsPlaying)
            {
                GameObject during_out_of_focus_object = u_roi.GetComponent<HotspotController>().during_out_of_focus_object;
                if (during_out_of_focus_object != null)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject.Equals(u_roi))
                        {
                            if (during_out_of_focus_object.GetComponent<MediaControllerAbstract>().IsPlaying)
                            {
                                during_out_of_focus_object.GetComponent<MediaControllerAbstract>().AbortMedia();
                            }
                            return;
                        }

                    }
                    if (!during_out_of_focus_object.GetComponent<MediaControllerAbstract>().IsPlaying)
                    {
                        during_out_of_focus_object.GetComponent<MediaControllerAbstract>().InvokePlayStop();
                    }
                }
            }
        }
    }
}
