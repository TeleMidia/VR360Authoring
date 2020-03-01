using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageController : MediaControllerAbstract
{
    public override void Load()
    {
        float width, height;
        Texture2D texture = new Texture2D(2, 2);
        Material material = new Material(Shader.Find("Unlit/Texture"));

        byte[] fileData = File.ReadAllBytes(this.file_path);
        texture.LoadImage(fileData);
        width = 10;
        height = width * texture.height / texture.width;
        material.mainTexture = texture;
        GetComponent<MeshRenderer>().material = material;
        this.transform.localScale = new Vector3(width, height, 1);
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
