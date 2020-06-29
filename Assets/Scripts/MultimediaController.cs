using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
public class MultimediaController : MonoBehaviour
{
    public delegate void MyHandler();
    // Start is called before the first frame update
    [Header("Video 360")]
    public GameObject video360Prefab;

    [Header("Preview 360")]
    public GameObject previewSphere, previewPlain;

    [Header("Hotspot 360")]
    public GameObject hotspotPrefab;
    [Header("Mirror")]
    public GameObject mirrorPrefab;

    [Header("Common Media")]
    public GameObject imagePrefab, audioPrefab, videoPrefab, textPrefab;

    public GameObject startPresentation;

    private GameObject video1, video2;

    private event MyHandler Port;
    private event MyHandler End;

    private GameObject initialScene;
    void Start()
    {   
        

        LoadXmlFile(@"C:\Users\paulo\Documents\VR360Authoring\Assets\concert_example.xml");
        GameObject[] videos360 = GameObject.FindGameObjectsWithTag("Video360");
        foreach(GameObject video360 in videos360)
        {
            this.End += video360.GetComponent<Video360Controller>().StopVideo360;
        }
        StopPresentation();

        Invoke("StartPresentation", 5);
        //StartPresentation();
    }
    
    GameObject AddVideo360(string src, float volume=1f)
    {
        GameObject video = Instantiate(video360Prefab);
        video.GetComponent<Video360Controller>().LoadVideo360(src, volume:volume);
        return video;        
    }

    private void SetAsInitial(GameObject video)
    {
        initialScene = video;
        this.Port += video.GetComponent<Video360Controller>().StartVideo360;
    }

    public void StartPresentation()
    {
        GameObject[] start_objects = GameObject.FindGameObjectsWithTag("StartPresentation");

        foreach(GameObject start_object in start_objects)
        {
            Destroy(start_object);
        }
        initialScene.SetActive(true);
        this.Port();
    }
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


    public void LoadXmlFile(string file_path)
    {
        XmlDocument document = new XmlDocument();
        document.Load(file_path);

        if (document.LastChild.Name.Equals("presentation360"))
        {
            XmlNode head = document.LastChild.FirstChild;
            XmlNode body = document.LastChild.LastChild;

            Dictionary<string, Movement> movements = null;
            Dictionary<string, Position> positions = null;
            
            //reading head nodes
            foreach (XmlNode head_child in head.ChildNodes)
            {
                if (head_child.Name.Equals("movementBase"))
                {
                    movements = ReadMovements(head_child);
                }
                if (head_child.Name.Equals("positionBase"))
                {
                    positions = ReadPositions(head_child);
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
                    AddAdditionalMedia(body_child, new_scene360, movements, positions, scene_objects);
                }
            }
            //adding additional media to video360 files
            /*foreach (XmlNode body_child in body.ChildNodes)
            {
                if (body_child.Name.Equals("scene360"))
                {
                    string id = body_child.Attributes.GetNamedItem("id").Value;
                    GameObject new_scene360 = scene_objects[id];
                    
                }
            }*/

            //adding targets to media

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
                        mirrorController.src_roi_object = scene_objects[mirrorController.file_path];
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

            var port = document.DocumentElement.SelectSingleNode("//presentation360/body/port");

            SetAsInitial(scene_objects[port.Attributes.GetNamedItem("component").Value]);
        }
    }

    public void AddAdditionalMedia(XmlNode scene360node, GameObject video360, Dictionary<string, Movement> movements, Dictionary<string, Position> positions, Dictionary<string, GameObject> scene_objects)
    {
        foreach (XmlNode mediaNode in scene360node.ChildNodes)
        {
            string[] mediaTypes = {"image", "audio", "video", "text", "preview", "subtitle", "hotspot", "mirror" };
            if (mediaTypes.Contains(mediaNode.Name)){
                float begin = mediaNode.Attributes.GetNamedItem("begin") != null? float.Parse(mediaNode.Attributes.GetNamedItem("begin").Value.Replace("s", "")): -1;
                float duration = mediaNode.Attributes.GetNamedItem("duration") != null ? float.Parse(mediaNode.Attributes.GetNamedItem("duration").Value.Replace("s", "")) : float.MaxValue;
                float volume = mediaNode.Attributes.GetNamedItem("volume") != null ? float.Parse(mediaNode.Attributes.GetNamedItem("volume").Value) : 0;
                bool follow_camera = mediaNode.Attributes.GetNamedItem("follow_camera") != null ? bool.Parse(mediaNode.Attributes.GetNamedItem("follow_camera").Value) : false;
                bool loop = mediaNode.Attributes.GetNamedItem("loop") != null ? bool.Parse(mediaNode.Attributes.GetNamedItem("loop").Value) : false;
                string src = mediaNode.Attributes.GetNamedItem("src") != null ? mediaNode.Attributes.GetNamedItem("src").Value : "";
                string text = mediaNode.Attributes.GetNamedItem("text") != null ? mediaNode.Attributes.GetNamedItem("text").Value : "";
                Movement movement = mediaNode.Attributes.GetNamedItem("movement") != null ? movements[mediaNode.Attributes.GetNamedItem("movement").Value] : null;
                string on_select_name = mediaNode.Attributes.GetNamedItem("on_select") != null ? mediaNode.Attributes.GetNamedItem("on_select").Value : "";
                string id = mediaNode.Attributes.GetNamedItem("id") != null ? mediaNode.Attributes.GetNamedItem("id").Value : "";
                Position position = mediaNode.Attributes.GetNamedItem("position") != null ? positions[mediaNode.Attributes.GetNamedItem("position").Value] : null; 
                float r = 0, theta = 0, phi = 0;                
                
                if(position != null)
                {
                    r = position.r;
                    theta = position.theta;
                    phi = position.phi;
                }
                GameObject mediaObject = null;
                switch (mediaNode.Name)
                {
                    case "image":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, imagePrefab, begin: begin, duration:duration,
                            r: r, theta: theta, phi: phi, movement: movement, file_path:src, follow_camera:follow_camera,
                            on_select_name: on_select_name);

                        break;
                    case "audio":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, audioPrefab, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, movement: movement, file_path: src, follow_camera: follow_camera, 
                            volume:volume, loop:loop);
                        break;
                    case "video":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, videoPrefab, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, movement: movement, file_path: src, follow_camera: follow_camera,
                            volume: volume, loop: loop, on_select_name: on_select_name);
                        break;
                    case "text":
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, textPrefab, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, movement: movement, follow_camera: follow_camera, text:text,
                            on_select_name: on_select_name);
                        break;
                    case "subtitle":
                        video360.GetComponent<Video360Controller>().AddSubtitle(id, textPrefab, file_path:src, r: r, theta: theta, phi: phi, on_select_name: on_select_name);
                        break;
                    case "preview":
                        string previewTime = mediaNode.Attributes.GetNamedItem("preview_time") != null ? mediaNode.Attributes.GetNamedItem("preview_time").Value : "0s,5s";
                        string shape = mediaNode.Attributes.GetNamedItem("shape") != null ? mediaNode.Attributes.GetNamedItem("shape").Value : "";
                        GameObject preview = shape.Equals("sphere")?previewSphere:previewPlain;
                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, preview, begin: begin, duration: duration,
                            r: r, theta: theta, phi: phi, movement: movement, follow_camera: follow_camera, on_select_name: on_select_name, previewTime:previewTime);
                        break;
                    case "hotspot":
                        string during_out_of_focus_name = mediaNode.Attributes.GetNamedItem("during_out_of_focus") != null ? mediaNode.Attributes.GetNamedItem("during_out_of_focus").Value : "";
                        string on_focus_name = mediaNode.Attributes.GetNamedItem("on_focus") != null ? mediaNode.Attributes.GetNamedItem("on_focus").Value : "";

                        mediaObject = video360.GetComponent<Video360Controller>().AddMedia(id, hotspotPrefab, begin:begin, duration:duration, r:r, theta:theta, phi:phi, on_focus_name:on_focus_name, during_out_of_focus_name:during_out_of_focus_name);
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


    public Dictionary<string, Movement> ReadMovements(XmlNode movementBase)
    {
        Dictionary<string, Movement> movements = new Dictionary<string, Movement>();

        foreach (XmlNode mov_node in movementBase.ChildNodes)
        {
            if (mov_node.Name.Equals("movement"))
            {
                Movement new_mov;
                var id = mov_node.Attributes.GetNamedItem("id").Value;
                var type = mov_node.Attributes.GetNamedItem("type").Value;
                float r=0, theta=0, phi=0, duration=1;
                foreach (XmlAttribute att in mov_node.Attributes)
                {
                    switch (att.Name)
                    {
                        case "r":                            
                            r = float.Parse(att.Value);                            
                            break;
                        case "theta":
                            theta = float.Parse(att.Value);
                            break;
                        case "phi":
                            phi= float.Parse(att.Value);
                            break;
                        case "duration":
                            duration = float.Parse(att.Value);
                            break;
                    }
                }
                switch (type)
                {
                    case "circular":
                        new_mov = new CircularMovement(r:r, theta:theta, phi:phi, duration:duration);
                        break;
                    default:
                        new_mov = new LinearMovement(r: r, theta: theta, phi: phi, duration: duration);
                        break;
                }

                
                movements.Add(id, new_mov);
            }
        }

        return movements;
    }
    public Dictionary<string, Position> ReadPositions(XmlNode positionBase)
    {
        Dictionary<string, Position> positions = new Dictionary<string, Position>();

        foreach (XmlNode pos_node in positionBase.ChildNodes)
        {
            if (pos_node.Name.Equals("position"))
            {
                Position new_pos;
                var id = pos_node.Attributes.GetNamedItem("id").Value;
                new_pos = new Position();

                foreach (XmlAttribute att in pos_node.Attributes)
                {
                    switch (att.Name)
                    {
                        case "r":
                            new_pos.r = float.Parse(att.Value);
                            //Debug.Log(att.Value + " r: " + new_pos.r);
                            break;
                        case "theta":
                            new_pos.theta = float.Parse(att.Value);
                            break;
                        case "phi":
                            new_pos.phi = float.Parse(att.Value);
                            break;
                    }
                }
                positions.Add(id, new_pos);
            }
        }

        return positions;
    }
}
