using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
