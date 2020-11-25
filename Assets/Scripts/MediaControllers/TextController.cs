using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes
/// Inherits MediaControllerAbstract
/// This class is the script that controls the text media object
/// </summary>
public class TextController : MediaControllerAbstract
{
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// Loads the text to the TextMesh component
    /// </summary>
    public override void Load()
    {
        GetComponent<TextMesh>().text = this.text;
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// Playing for the text consists in enabling its MeshRenderer
    /// </summary>
    public override void PlayMedia()
    {
        GetComponent<MeshRenderer>().enabled = true;        
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract
    /// Stopping for the text consists in disabling its MeshRenderer
    /// </summary>
    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
