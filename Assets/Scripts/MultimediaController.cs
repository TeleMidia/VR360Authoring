using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Xml;
public class MultimediaController : MonoBehaviour
{
    public delegate void MyHandler();
    // Start is called before the first frame update
    [Header("Video 360")]
    public GameObject video360Prefab, video360PreviewPrefab;

    [Header("Common Media")]
    public GameObject imagePrefab, audioPrefab, videoPrefab, textPrefab;

    public GameObject startPresentation;

    private GameObject video1, video2;

    private event MyHandler Port;
    private event MyHandler End;
    void Start()
    {
        /*video1 = AddVideo360("C:/Users/paulo/Downloads/360_VR Master Series _ Free Download _ Crystal Shower Falls.mp4");
        video2 = AddVideo360("C:/Users/paulo/Downloads/Best VR 360 Video.mp4");

        video1.GetComponent<Video360Controller>().AddSubtitle(textPrefab, r: 10, theta: 25, phi: 0, file_path: @"C:\Users\paulo\Downloads\jumanji-the-next-level_HI_english-2156994\Jumanji.The.Next.Level.2019.BDRip.XviD.AC3-EVO-HI.srt");

        //video1.GetComponent<Video360Controller>().AddMedia(textPrefab, start_time: 0, duration: 3, r: 5, theta: 25, phi: 0, text: "Imagem seguindo camera", follow_camera: true);
        //video1.GetComponent<Video360Controller>().AddMedia(textPrefab, start_time: 3, duration: 5, r: 5, theta: 25, phi: 0, text: "Imagem com som", follow_camera: true);

        video1.GetComponent<Video360Controller>().AddMedia(imagePrefab,
            start_time: 0f, duration: 3, file_path: @"C:\Users\paulo\Pictures\WebMedia\31-10-2019-505.jpg",
            r: 15f, theta: 0f, phi: 0f, follow_camera: true);



        video1.GetComponent<Video360Controller>().AddMedia(imagePrefab,
            start_time: 3f, duration: 10, file_path: @"C:\Users\paulo\Pictures\Saved Pictures\vapor-wave.png",
            r: 15f, theta: -30f, phi: 0f, movement: new LinearMovement(r: 15, theta: 0f, phi: 0, duration: 6f));

        video1.GetComponent<Video360Controller>().AddMedia(audioPrefab,
            start_time: 3f, duration: 10, file_path: "C:/Users/paulo/Downloads/sample.wav",
            r: 15f, theta: -30f, phi: 0f, volume: 1, loop: false, movement: new LinearMovement(r: 15, theta: 0f, phi: 0, duration: 6f));


        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 10f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 60f, volume: 1, loop: false);


        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 15f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f, volume: 1, loop: false);


        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 20f, duration: 5, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: -60f, volume: 1, loop: false);

        video1.GetComponent<Video360Controller>().AddMedia(videoPrefab,
           start_time: 25f, duration: 12, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f, volume: 1, loop: true, movement: new CircularMovement(r: 0f, theta: 0, phi: 720, duration: 10));

        video1.GetComponent<Video360Controller>().AddMedia(video360PreviewPrefab, start_time: 60, duration: 60, r: 4, theta: 0, phi: 60, controller: video2);

        float step = 60;
        for (int i = 0; i < 360 / step; i++)
        {
            video2.GetComponent<Video360Controller>().AddMedia(videoPrefab, start_time: 2f, duration: 60, file_path: "C:/Users/paulo/Downloads/videoplayback.mp4",
           r: 30f, theta: 0f, phi: 0f + i * step, volume: 1, loop: false, movement: new CircularMovement(r: -22f, theta: 0, phi: 360, duration: 10));
        }

        SetAsInitial(this.video1);
        //this.Port();
        */

        LoadXmlFile("C:/Users/paulo/Documents/VR360Video/Assets/360PresentationExample.xml");
        GameObject[] videos360 = GameObject.FindGameObjectsWithTag("Video360");
        foreach(GameObject video360 in videos360)
        {
            this.End += video360.GetComponent<Video360Controller>().StopVideo360;
        }

        StopPresentation();
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
        this.Port += video.GetComponent<Video360Controller>().StartVideo360;
    }

    public void StartPresentation()
    {
        this.Port();
    }
    public void StopPresentation()
    {
        this.End();
        Instantiate(startPresentation);
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
            Dictionary<string, GameObject> video360_objects = new Dictionary<string, GameObject>();
            foreach (XmlNode body_child in body.ChildNodes)
            {
                if (body_child.Name.Equals("video360"))
                {
                    string id = body_child.Attributes.GetNamedItem("id").Value;
                    string src = body_child.Attributes.GetNamedItem("src").Value;
                    float volume = 1f;
                    XmlNode volume_node = body_child.Attributes.GetNamedItem("volume");
                    if(volume_node != null)
                    {
                        volume = float.Parse(volume_node.Value);
                    }
                    GameObject new_video360 = AddVideo360(src:src, volume:volume);                    
                    video360_objects.Add(id, new_video360);
                }
            }
            //adding additional media to video360 files
            foreach (XmlNode body_child in body.ChildNodes)
            {
                if (body_child.Name.Equals("video360"))
                {
                    string id = body_child.Attributes.GetNamedItem("id").Value;
                    GameObject new_video360 = video360_objects[id];
                    AddAdditionalMedia(body_child, new_video360, movements, positions, video360_objects);
                }
            }

            var port = document.DocumentElement.SelectSingleNode("//presentation360/body/port");

            SetAsInitial(video360_objects[port.Attributes.GetNamedItem("component").Value]);
        }
    }

    public void AddAdditionalMedia(XmlNode video360node, GameObject video360, Dictionary<string, Movement> movements, Dictionary<string, Position> positions, Dictionary<string, GameObject> video360_objects)
    {
        foreach (XmlNode mediaNode in video360node.ChildNodes)
        {
            if (mediaNode.Name.Equals("media"))
            {
                string type = mediaNode.Attributes.GetNamedItem("type").Value;
                float start_time = mediaNode.Attributes.GetNamedItem("start_time") != null? float.Parse(mediaNode.Attributes.GetNamedItem("start_time").Value): 0;
                float duration = mediaNode.Attributes.GetNamedItem("duration") != null ? float.Parse(mediaNode.Attributes.GetNamedItem("duration").Value) : 0;
                float volume = mediaNode.Attributes.GetNamedItem("volume") != null ? float.Parse(mediaNode.Attributes.GetNamedItem("volume").Value) : 0;
                bool follow_camera = mediaNode.Attributes.GetNamedItem("follow_camera") != null ? bool.Parse(mediaNode.Attributes.GetNamedItem("follow_camera").Value) : false;
                bool loop = mediaNode.Attributes.GetNamedItem("loop") != null ? bool.Parse(mediaNode.Attributes.GetNamedItem("loop").Value) : false;
                string src = mediaNode.Attributes.GetNamedItem("src") != null ? mediaNode.Attributes.GetNamedItem("src").Value : "";
                string text = mediaNode.Attributes.GetNamedItem("text") != null ? mediaNode.Attributes.GetNamedItem("text").Value : "";
                Movement movement = mediaNode.Attributes.GetNamedItem("movement") != null ? movements[mediaNode.Attributes.GetNamedItem("movement").Value] : null;
                GameObject controller = mediaNode.Attributes.GetNamedItem("anchor") != null ? video360_objects[mediaNode.Attributes.GetNamedItem("anchor").Value] : null;
                Position position = mediaNode.Attributes.GetNamedItem("position") != null ? positions[mediaNode.Attributes.GetNamedItem("position").Value] : null; ;
                float r = 0, theta = 0, phi = 0;                
                
                if(position != null)
                {
                    r = position.r;
                    theta = position.theta;
                    phi = position.phi;
                }

                switch (type)
                {
                    case "image":
                        video360.GetComponent<Video360Controller>().AddMedia(imagePrefab, start_time:start_time, duration:duration,
                            r: r, theta: theta, phi: phi, movement: movement, file_path:src, follow_camera:follow_camera);
                        break;
                    case "audio":
                        video360.GetComponent<Video360Controller>().AddMedia(audioPrefab, start_time: start_time, duration: duration,
                           r: r, theta: theta, phi: phi, movement: movement, file_path: src, follow_camera: follow_camera, 
                           volume:volume, loop:loop);
                        break;
                    case "video":
                        video360.GetComponent<Video360Controller>().AddMedia(videoPrefab, start_time: start_time, duration: duration,
                           r: r, theta: theta, phi: phi, movement: movement, file_path: src, follow_camera: follow_camera,
                           volume: volume, loop: loop);
                        break;
                    case "text":
                        video360.GetComponent<Video360Controller>().AddMedia(textPrefab, start_time: start_time, duration: duration,
                           r: r, theta: theta, phi: phi, movement: movement, follow_camera: follow_camera, text:text);
                        break;
                    case "subtitle":
                        video360.GetComponent<Video360Controller>().AddSubtitle(textPrefab, file_path:src, r: r, theta: theta, phi: phi);
                        break;
                    case "video360anchor":
                        video360.GetComponent<Video360Controller>().AddMedia(video360PreviewPrefab, start_time: start_time, duration: duration,
                           r: r, theta: theta, phi: phi, movement: movement, follow_camera: follow_camera, controller:controller);
                        break;
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
