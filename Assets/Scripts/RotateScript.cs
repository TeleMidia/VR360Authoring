using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// This script is responsible for allowing the movement of the camera thorugh the mouse (when a HMD is not available).
/// </summary>
public class RotateScript : MonoBehaviour
{
    /// <summary>
    /// Speed of the rotation of the camera.
    /// </summary>
    public float speed;

    /// <summary>
    /// Called once per frame, inherited from MonoBehaviour.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * speed * Time.deltaTime, Space.Self);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        if (Input.GetMouseButton(1))
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
