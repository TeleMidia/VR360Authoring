using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextController : MediaControllerAbstract
{
    public override void Load()
    {
        GetComponent<TextMesh>().text = this.text;
    }
    
    public override void PlayMedia()
    {
        GetComponent<MeshRenderer>().enabled = true;        
    }
    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
