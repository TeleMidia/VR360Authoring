using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookROIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
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
            }
        }
    }
}
