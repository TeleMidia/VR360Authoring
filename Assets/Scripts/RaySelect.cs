 using UnityEngine;
 using System.Collections;
using Valve.VR;

public class RaySelect : MonoBehaviour
{
    private LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 10f;
    // a reference to the action
    public SteamVR_Action_Boolean SelectVideo;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    // Start is called before the first frame update


    void Start()
    {
        this.laserLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        SelectVideo.AddOnStateDownListener(TriggerDown, handType);
    }

    void Update()
    {
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth*2;
        ShootLaserFromTargetPosition(transform.position, transform.forward, laserMaxLength);

    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition;

        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, laserMaxLength))
        {
            endPosition = raycastHit.collider.gameObject.transform.position;
        }
        else
        {
            endPosition = targetPosition + (length * direction);
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        RaycastHit hit;
        Debug.Log("Apertou");

        if (Physics.Raycast(transform.position, transform.forward, out hit, laserMaxLength))
        {
            if (hit.collider.gameObject.CompareTag("TargetTransition"))
            {
                hit.collider.gameObject.GetComponent<MediaControllerAbstract>().father.GetComponent<Video360Controller>().StopVideo360();
                hit.collider.gameObject.GetComponent<MediaControllerAbstract>().controller.GetComponent<Video360Controller>().StartVideo360();
            }

            if (hit.collider.gameObject.CompareTag("StartPresentation"))
            {
                Destroy(hit.collider.gameObject);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaController>().StartPresentation();                
            }
            Debug.Log("Colidiu "+ hit.collider.gameObject.tag);
        }
        else
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaController>().StopPresentation();
        }
    }
}