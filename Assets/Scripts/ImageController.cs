using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    // Start is called before the first frame update
    public float start_time, duration;

    public void LoadImage(string file_path, float width, float height)
    {
        GetComponent<MeshRenderer>().enabled = false;
        Texture2D texture = new Texture2D(2, 2);
        Material material = new Material(Shader.Find("Unlit/Texture"));

        byte[] fileData = File.ReadAllBytes(file_path);
        texture.LoadImage(fileData);
        material.mainTexture = texture;
        GetComponent<MeshRenderer>().material = material;
        this.transform.localScale = new Vector3(width, height, 1);
        
    }
    public void Play()
    {
        Invoke("ActiveRenderer", start_time);
    }
    void ActiveRenderer()
    {
        GetComponent<MeshRenderer>().enabled = true;
        Invoke("DeactiveRenderer", duration);
    }
    void DeactiveRenderer()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
