using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
/// <summary>
/// Author: Paulo Renato Conceição Mendes.<br/>
/// Main controller of the project. It reads presentations and controls them.
/// </summary>
public class MultimediaControllerScript : MonoBehaviour
{
    //url of the presentation
    public string presentation;
    //handler used to handle events
    public delegate void MyHandler();
    //prefab of 360 videos
    [Header("Video 360")]
    public GameObject video360Prefab;
    //prefabs of previews
    [Header("Preview 360")]
    public GameObject previewSphere, previewPlain;
    //prefab of hotspot
    [Header("Hotspot 360")]
    public GameObject hotspotPrefab;
    //prefab of mirror
    [Header("Mirror")]
    public GameObject mirrorPrefab;
    //prefabs of image, audio, video and text
    [Header("Common Media")]
    public GameObject imagePrefab, audioPrefab, videoPrefab, textPrefab;
    //game object the when selected starts the presentation
    public GameObject startPresentation;
    //event that calls initial 360 video
    private event MyHandler Entry;
    //event that stops videos 360 when the application ends
    private event MyHandler End;
    //stores the initial 360 video
    private GameObject initialScene;
    //controls the ids that will be assigned to media objects to which the user haven't assigned any id.
    private int globalid = 0;

    /// <summary>
    /// Called when the gameobject starts, inherited from MonoBehaviour.
    /// </summary>
    void Start()
    {   
        LoadXmlFile(presentation);
        GameObject[] videos360 = GameObject.FindGameObjectsWithTag("Video360");
        foreach(GameObject video360 in videos360)
        {
            this.End += video360.GetComponent<Video360Controller>().StopVideo360;
        }
        StopPresentation();

        Invoke("StartPresentation", 1);
        //StartPresentation();
    }
    /// <summary>
    /// Loads a 360 video from a url.
    /// </summary>
    /// <param name="src">url of the 360 video</param>
    /// <param name="volume">volume that the 360 video will be played</param>
    /// <returns></returns>
    GameObject AddVideo360(string src, float volume=1f)
    {
        GameObject video = Instantiate(video360Prefab);
        video.GetComponent<Video360Controller>().LoadVideo360(src, volume:volume);
        return video;        
    }
    /// <summary>
    /// Sets a 360 video as initial.
    /// </summary>
    /// <param name="video">video that will be assigned as initial</param>
    private void SetAsInitial(GameObject video)
    {
        initialScene = video;
        this.Entry += video.GetComponent<Video360Controller>().StartVideo360;
    }
    /// <summary>
    /// Starts the presentation.
    /// </summary>
    public void StartPresentation()
    {
        GameObject[] start_objects = GameObject.FindGameObjectsWithTag("StartPresentation");

        foreach(GameObject start_object in start_objects)
        {
            Destroy(start_object);
        }
        initialScene.SetActive(true);
        this.Entry();
    }
    /// <summary>
    /// Stops the presentation.
    /// </summary>
    public void StopPresentation()
    {
        this.End();
        Instantiate(startPresentation);

        GameObject[] videos360 = GameObject.FindGameObjectsWithTag("Video360");
        foreach (GameObject video360 in videos360)
        {
            video360.SetActive(false);
        }
    }

    /// <summary>
    /// Loads all the 360 videos and their media objects from a url to an xml file.
    /// </summary>
    /// <param name="file_path">url of the xml file</param>
    public void LoadXmlFile(string file_path)
    {
        XmlDocument document = new XmlDocument();
        document.Load(file_path);

        if (document.LastChild.Name.Equals("presentation360"))
        {
            XmlNode head = document.LastChild.FirstChild;
            XmlNode body = document.LastChild.LastChild;
            Dictionary<string, XmlNode> styles = new Dictionary<string, XmlNode>();

            //reading head nodes
            foreach (XmlNode head_child in head.ChildNodes)
            {
                if (head_child.Name.Equals("style"))
                {
                    string id_style = head_child.Attributes.GetNamedItem("id").Value;
                    head_child.Attributes.RemoveNamedItem("id");
                    styles.Add(id_style, head_child);
                }
            }

            //reading video360 body nodes
            Dictionary<string, GameObject> scene_objects = new Dictionary<string, GameObject>();
            foreach (XmlNode body_child in body.ChildNodes)
            {
                if (body_child.Name.Equals("scene360"))
                {
                    string id = body_child.Attributes.GetNamedItem("id").Value;
                    string src = body_child.Attributes.GetNamedItem("src").Value;
                    float volume = 1f;
                    XmlNode volume_node = body_child.Attributes.GetNamedItem("volume");
                    if(volume_node != null)
                    {
                        volume = float.Parse(volume_node.Value);
                    }
                    GameObject new_scene360 = AddVideo360(src:src, volume:volume);
                    scene_objects.Add(id, new_scene360);
                    AddAdditionalMedia(body_child, new_scene360, scene_objects, styles);
                }
            }

            foreach (string media_id in scene_objects.Keys){
                GameObject curMedia;

                curMedia = scene_objects[media_id];
                
                HotspotController hotspotController = curMedia.GetComponent<HotspotController>();
                if(hotspotController != null)
                {
                    if (scene_objects.Keys.Contains(hotspotController.on_focus_name))
                    {
                        hotspotController.on_focus_object = scene_objects[hotspotController.on_focus_name];
                    }
                    if (scene_objects.Keys.Contains(hotspotController.during_out_of_focus_name))
                    {
                        hotspotController.during_out_of_focus_object = scene_objects[hotspotController.during_out_of_focus_name];
                    }
                }
                MirrorController mirrorController = curMedia.GetComponent<MirrorController>();
                if(mirrorController != null)
                {
                    if (scene_objects.Keys.Contains(mirrorController.file_path))
                    {
                        mirrorController.src_hotspot_object = scene_objects[mirrorController.file_path];
                    }
                }
                MediaControllerAbstract mediaControllerAbstract = curMedia.GetComponent<MediaControllerAbstract>();
                if (mediaControllerAbstract != null)
                {
                    if (scene_objects.Keys.Contains(mediaControllerAbstract.on_select_name))
                    {
                        mediaControllerAbstract.on_select_object = scene_objects[mediaControllerAbstract.on_select_name];
                    }
                    mediaControllerAbstract.Load();
                }
            }

            SetAsInitial(scene_objects[body.Attributes.GetNamedItem("entry").Value]);
        }
    }
    /// <summary>
    /// Add aditional media objects to a 360 video.
    /// </summary>
    /// <param name="scene360node">Current scene 360 being read</param>
    /// <param name="video360">360 video to which the media objects will be added</param>
    /// <param name="scene_objects">List of all scene 360 objects</param>
    /// <param name="styles">List of styles defined in the Head. Those styles may be reused by media objects</param>
    public void AddAdditionalMedia(XmlNode scene360node, GameObject video360, Dictionary<string, GameObject> scene_objects, Dictionary<string, XmlNode> styles)
    {
        foreach (XmlNode mediaNode in scene360node.ChildNodes)
        {
            string[] mediaTypes = {"image", "audio", "video", "text", "preview", "subtitle", "hotspot", "mirror" };
            if (mediaTypes.Contains(mediaNode.Name)){

                Dictionary<string, string> media_attributes = new Dictionary<string, string>();

                //original attributes
                foreach (XmlAttribute at in mediaNode.Attributes)
                {
                    media_attributes.Add(at.Name.ToLower(), at.Value);
                }
                //adding style attributes to media
                XmlNode media_style = mediaNode.Attributes.GetNamedItem("style");
                if(media_style != null)
                {
                    foreach(XmlAttribute at in styles[media_style.Value].Attributes){
                        media_attributes.Add(at.Name.ToLower(), at.Value);
                    }
                }                

                string aux;
                var teste = media_attributes.TryGetValue("begin", out aux);
                //common attributes
                float begin = media_attributes.TryGetValue("begin", out aux)? float.Parse(aux.Replace("s", "")): -1;
                float duration = media_attributes.TryGetValue("duration", out aux) ? float.Parse(aux.Replace("s", "")) : float.MaxValue;
                float volume = media_attributes.TryGetValue("volume", out aux) ? float.Parse(aux) : 0;
                bool follow_camera = media_attributes.TryGetValue("followcamera", out aux)? bool.Parse(aux) : false;
                bool loop = media_attributes.TryGetValue("loop", out aux)? bool.Parse(aux) : false;
                string src = media_attributes.TryGetValue("src", out aux)? aux : "";
                string text = media_attributes.TryGetValue("text", out aux)? aux : "";
                string on_select_name = media_attributes.TryGetValue("onselect", out aux)? aux: "";
                string id = media_attributes.TryGetValue("id", out aux)? aux : "id"+(globalid++);
                float r = media_attributes.TryGetValue("r", out aux)? float.Parse(aux) : 0;
                float theta = media_attributes.TryGetValue("theta", out aux)? float.Parse(aux) : 0;
                float phi = media_attributes.TryGetValue("phi", out aux)? float.Parse(aux) : 0;

                GameObject mediaObject = null;
                switch (mediaNode.Name)
                {
                    case "image":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, imagePrefab, begin: begin, duration:duration,
                            r: r, theta: theta, phi: phi, file_path:src, follow_camera:follow_camera,
                            on_select_name: on_select_name);
                        break;
                    case "audio":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, audioPrefab, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, file_path: src, follow_camera: follow_camera, 
                            volume:volume, loop:loop);
                        break;
                    case "video":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, videoPrefab, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, file_path: src, follow_camera: follow_camera,
                            volume: volume, loop: loop, on_select_name: on_select_name);
                        break;
                    case "text":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, textPrefab, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, follow_camera: follow_camera, text:text.Replace("\\n","\n"),
                            on_select_name: on_select_name);
                        break;
                    case "subtitle":
                        video360.GetComponent<Video360Controller>().AddSubtitle(id, textPrefab, file_path:src, r: r, theta: theta, phi: phi, on_select_name: on_select_name);
                        break;
                    case "preview":
                        float clipBegin = media_attributes.TryGetValue("clipbegin", out aux) ? float.Parse(aux.Replace("s", "")) : 0;
                        float clipEnd = media_attributes.TryGetValue("clipend", out aux) ? float.Parse(aux.Replace("s", "")) : 5;

                        string shape = media_attributes.TryGetValue("shape", out aux)? aux : "";
                        GameObject preview = shape.Equals("sphere")?previewSphere:previewPlain;
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, preview, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, follow_camera: follow_camera, on_select_name: src, clipBegin: clipBegin, clipEnd:clipEnd);
                        break;
                    case "hotspot":
                        string duringNotLookingAt = media_attributes.TryGetValue("duringnotlookingat", out aux)? aux: "";
                        string onLookAt = media_attributes.TryGetValue("onlookat", out aux)? aux: "";

                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, hotspotPrefab, begin:begin, duration:duration, r:r, theta:theta, phi:phi, on_focus_name: onLookAt, during_out_of_focus_name: duringNotLookingAt);
                        break;
                    case "mirror":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, mirrorPrefab, begin: begin, duration: duration, r: r, theta: theta, phi: phi, on_select_name: on_select_name, follow_camera:follow_camera,file_path:src);
                        break;
                } 
                if(mediaObject != null)
                {
                    scene_objects.Add(id, mediaObject);
                }
            }
        }
    }
    
}
