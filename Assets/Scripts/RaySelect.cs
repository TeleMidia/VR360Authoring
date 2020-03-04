 using UnityEngine;
 using System.Collections;
 
 public class RaySelect : MonoBehaviour
{
    private LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 10f;

    void Start()
    {
        this.laserLineRenderer = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
          
    }

    void Update()
    {
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;
        ShootLaserFromTargetPosition(transform.position, transform.forward, laserMaxLength);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserMaxLength))
        {
            if (hit.collider.gameObject.CompareTag("VideoPreview"))
            {
                Debug.Log("Colidiu");
            }
        }
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }
}