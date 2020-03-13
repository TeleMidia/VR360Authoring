using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookROIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] u_roi_objects;

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 100);
        GameObject object_looked;
        
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("ROIObject"))
                {
                    Debug.Log("Olhou");
                    GameObject target = hit.collider.gameObject.GetComponent<RoiController>().target;
                    if (target != null)
                    {
                        Debug.Log("Chamou");
                        target.GetComponent<MediaControllerAbstract>().InvokePlayStop();
                    }
                    Destroy(hit.collider.gameObject);

                    return;
                }
            }
        }
        this.u_roi_objects = GameObject.FindGameObjectsWithTag("u_ROIObject");
        foreach (GameObject u_roi in this.u_roi_objects)
        {
            if (u_roi.GetComponent<RoiController>().IsPlaying)
            {
                GameObject target = u_roi.GetComponent<RoiController>().target; ;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.gameObject.Equals(u_roi))
                    {
                        if (target != null && target.GetComponent<MediaControllerAbstract>().IsPlaying){
                            target.GetComponent<MediaControllerAbstract>().AbortMedia();
                        }
                        return;
                    }

                }
                if (target != null)
                {
                    Debug.Log("Chamou");
                    if (!target.GetComponent<MediaControllerAbstract>().IsPlaying)
                    {
                        target.GetComponent<MediaControllerAbstract>().InvokePlayStop();
                    }
                }
                //Destroy(u_roi);
            }
        }
    }
}
