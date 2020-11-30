 using UnityEngine;
 using System.Collections;
using Valve.VR;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Responsible for casting a ray/laser from the controller and checking if a selection is performed.
/// </summary>
public class RaySelect : MonoBehaviour
{
    //line renderer that draws ray
    private LineRenderer laserLineRenderer;
    // width of the laser
    public float laserWidth = 0.1f;
    // maximum size of the laser
    public float laserMaxLength = 10f;
    // a reference to the action
    public SteamVR_Action_Boolean SelectVideo;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    // Start is called before the first frame update

    /// <summary>
    /// Called when the gameobject starts, inherited from MonoBehaviour.
    /// </summary>
    void Start()
    {
        this.laserLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        SelectVideo.AddOnStateDownListener(TriggerDown, handType);
    }
    /// <summary>
    /// Called once per frame, inherited from MonoBehaviour.<br/>
    /// At each time, casts a ray with the line renderer.
    /// </summary>
    void Update()
    {
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth*2;
        ShootLaserFromTargetPosition(transform.position, transform.forward, laserMaxLength);

    }
    /// <summary>
    /// Shoots a laser from a position with a direction.
    /// </summary>
    /// <param name="targetPosition">Initital position of the laser</param>
    /// <param name="direction">Direction of the laser</param>
    /// <param name="length">Length of the laser</param>
    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition;

        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, laserMaxLength) &&
            (raycastHit.collider.gameObject.CompareTag("TargetTransition") ||
            raycastHit.collider.gameObject.CompareTag("StartPresentation")))
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
    /// <summary>
    /// If the trigger is down, verifies if something is selected.
    /// </summary>
    /// <param name="fromAction">Action that called the method</param>
    /// <param name="fromSource">Controller that called the method</param>
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        RaycastHit hit;
        Debug.Log("Apertou");

        if (Physics.Raycast(transform.position, transform.forward, out hit, laserMaxLength))
        {
            if (hit.collider.gameObject.CompareTag("TargetTransition"))
            {
                hit.collider.gameObject.GetComponent<MediaControllerAbstract>().father.GetComponent<Video360Controller>().StopVideo360();
                hit.collider.gameObject.GetComponent<MediaControllerAbstract>().on_select_object.SetActive(true);
                hit.collider.gameObject.GetComponent<MediaControllerAbstract>().on_select_object.GetComponent<Video360Controller>().StartVideo360();
            }

            if (hit.collider.gameObject.CompareTag("StartPresentation"))
            {
                Destroy(hit.collider.gameObject);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaControllerScript>().StartPresentation();                
            }
            Debug.Log("Colidiu "+ hit.collider.gameObject.tag);
        }
        else
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MultimediaControllerScript>().StopPresentation();
        }
    }
}