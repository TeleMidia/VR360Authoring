using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Inherits MediaControllerAbstract. This class is the script that controls the image media object.
/// </summary>
public class ImageController : MediaControllerAbstract
{
    /// <summary>
    /// Inherited from MediaControllerAbstract. Loads the image creating a material from a texture and assigning that material to the object mesh renderer.
    /// </summary>
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
    /// <summary>
    /// Inherited from MediaControllerAbstract. Playing for the image consists in enabling its MeshRenderer.
    /// </summary>
    public override void PlayMedia()
    {
        GetComponent<MeshRenderer>().enabled = true;        
    }
    /// <summary>
    /// Inherited from MediaControllerAbstract. Stopping for the image consists in disabling its MeshRenderer.
    /// </summary>
    public override void StopMedia()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
