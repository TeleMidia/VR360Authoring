using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// Responsible for checking wether the immersives controllers are connected.
/// </summary>
public class ControllerChecker : MonoBehaviour
{
    /// <summary>
    /// This function is called when the state of connection of a controller changes. If the controller is 
    /// disconnected, the Ray Select and line renderers are deactivated. The reverse occurs otherwise.
    /// </summary>
    /// <param name="pose">The controller</param>
    /// <param name="sources">The input sources of the controller</param>
    /// <param name="connected">If the controller was connected or disconnected</param>
    public void Change(SteamVR_Behaviour_Pose pose, SteamVR_Input_Sources sources, bool connected)
    {
        if (connected)
        {
            pose.GetComponent<LineRenderer>().enabled = true;
            pose.GetComponent<RaySelect>().enabled = true;
            Debug.Log("Connected");
        }
        else
        {
            pose.GetComponent<LineRenderer>().enabled = false;
            pose.GetComponent<RaySelect>().enabled = false;
            Debug.Log("Disconnected");
        }
    }
}
