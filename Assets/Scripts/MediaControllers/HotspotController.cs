using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// Inherits MediaControllerAbstract
/// This class is the script that controls the hotspot media object
/// </summary>
public class HotspotController : MediaControllerAbstract
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
    /// This media object doesnt do anything additionaly to play besides what is done in the MediaControllerAbstract Class
    /// It is empty because the method has to be implemented
    /// </summary>
    public override void PlayMedia()
    {
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// This media object doesnt do anything additionaly to stop besides what is done in the MediaControllerAbstract Class
    /// It is empty because the method has to be implemented
    /// </summary>
    public override void StopMedia()
    {
    }
}
